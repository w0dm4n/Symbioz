using Symbioz.Auth;
using Symbioz.Auth.Handlers;
using Symbioz.Auth.Models;
using Symbioz.Core;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.ORM;
using Symbioz.Utils;
using Symbioz.World.Models;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Records;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class ApproachHandler
    {
        public static char[] UnauthorizedNameContent = new char[] { '[', '{', '}', ']', '\'' };

        [MessageHandler]
        public static void HandleAuthentificationTicket(AuthenticationTicketMessage message, WorldClient client)
        {
            var msg = (AuthenticationTicketMessage)message;
            var reader = new BigEndianReader(Encoding.ASCII.GetBytes(msg.ticket));
            var count = reader.ReadByte();
            var ticket = reader.ReadUTFBytes(count);
            client.Account = ServersManager.GetAccount(ticket);
            client.Characters = CharacterRecord.GetAccountCharacters(client.Account.Id);
            client.Send(new AuthenticationTicketAcceptedMessage());
            client.Send(new AccountCapabilitiesMessage(true, true, client.Account.Id, BreedRecord.AvailableBreedsFlags, BreedRecord.AvailableBreedsFlags, 1));
            client.Send(new TrustStatusMessage(true, true));
            client.Send(new ServerOptionalFeaturesMessage(new sbyte[] { 1, 2, 3, 4, 5, 6 }));
        }
        [MessageHandler]
        public static void HandleCharacterList(CharactersListRequestMessage message, WorldClient client)
        {
            client.Send(new CharactersListMessage(client.Characters.ConvertAll<CharacterBaseInformations>(x => x.GetBaseInformation()), false)); // StartupActions ?
        }
        [MessageHandler]
        public static void HandleCharacterNameSuggestion(CharacterNameSuggestionRequestMessage message, WorldClient client)
        {
            client.Send(new CharacterNameSuggestionSuccessMessage(StringUtils.RandomName()));
        }
        [MessageHandler]
        public static void HandleCharacterSelection(CharacterSelectionMessage message, WorldClient client)
        {
            client.Character = new Character(CharacterRecord.GetCharacterRecordById(message.id), client);
            ProcessSelection(client);
        }
        [MessageHandler]
        public static void HandleCharacterCreationRequest(CharacterCreationRequestMessage message, WorldClient client)
        {
            if (client.Characters.Count() == client.Account.MaxCharactersCount)
            {
                client.Send(new CharacterCreationResultMessage((sbyte)CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS));
                return;
            }
            if (CharacterRecord.CheckCharacterNameExist(message.name))
            {

                client.Send(new CharacterCreationResultMessage((sbyte)CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }
            if (client.Account.Role <= ServerRoleEnum.MODERATOR)
            {
                foreach (var value in message.name)
                {
                    if (UnauthorizedNameContent.Contains(value))
                    {
                        client.Send(new CharacterCreationResultMessage((sbyte)CharacterCreationResultEnum.ERR_INVALID_NAME));
                        return;
                    }
                }
            }
            string look = BreedRecord.GetBreedEntityLook((int)message.breed, message.sex, (int)message.cosmeticId, message.colors).ConvertToString();

            CharacterRecord newCharacter = CharacterRecord.Default(message.name, client.Account.Id, look, message.breed, message.sex);

            client.Character = new Character(newCharacter, client);
            client.Character.IsNew = true;
            StatsRecord.Create(client.Character);
            client.Character.SetLevel(ConfigurationManager.Instance.StartLevel);
            client.Character.Record.AddElement();
            client.Character.UpdateBreedSpells();
            client.Character.LearnAllJobs();
            Logger.Log("Character " + newCharacter.Name + " created!");
            ProcessSelection(client);
        }
        [MessageHandler]
        public static void HandleCharacterDeletion(CharacterDeletionRequestMessage message, WorldClient client) // finish this
        {
            CharacterRecord deletedCharacter = CharacterRecord.GetCharacterRecordById(message.characterId);
            if (deletedCharacter == null)
                return;
            StatsRecord.GetStatsRecord(message.characterId).RemoveElement();
            CharacterRecord.Characters.Remove(deletedCharacter);
            client.Characters.Remove(deletedCharacter);
            deletedCharacter.RemoveElement();
            CharacterItemRecord.RemoveAll(message.characterId);
            GeneralShortcutRecord.RemoveAll(message.characterId);
            CharacterSpellRecord.RemoveAll(message.characterId);
            CharacterJobRecord.RemoveAll(message.characterId);
            BidShopGainRecord.RemoveAll(message.characterId);
            CharacterGuildRecord.RemoveAll(message.characterId); // Si il est meneur de guilde?
            BidShopItemRecord.RemoveAll(message.characterId);
            Logger.Log("Character " + deletedCharacter.Name + " deleted");

            client.Send(new CharactersListMessage(client.Characters.ConvertAll<CharacterBaseInformations>(x => x.GetBaseInformation()), false));
        }

        static void ProcessSelection(WorldClient client)
        {
            client.Send(new CharacterSelectedSuccessMessage(new CharacterBaseInformations((uint)client.Character.Id, (byte)client.Character.Record.Level, client.Character.Record.Name, client.Character.Look.ToEntityLook(), (sbyte)client.Character.Record.Breed, client.Character.Record.Sex), false));
            StatsRecord.InitializeCharacter(client.Character);

            client.Character.Inventory.Refresh();
            client.Character.RefreshShortcuts();
            client.Character.RefreshEmotes();
            client.Character.InitializeCosmetics();
            client.Character.RefreshJobs();
            client.Character.RefreshSpells();
            client.Character.RefreshArenasInfos();
            client.Character.RefreshPvPInfos();
            client.Character.OnConnectedGuildInformations();
            client.Character.OnConnectedAllianceInformations();
            client.Character.OnConnectedNotifications();
            client.Send(new CharacterCapabilitiesMessage(4095));
            client.Send(new CharacterLoadingCompleteMessage());

        }
        [MessageHandler]
        public static void HandleGameContextCreate(GameContextCreateRequestMessage message, WorldClient client)
        {
            client.Send(new GameContextDestroyMessage());
            client.Send(new GameContextCreateMessage((sbyte)GameContextEnum.ROLE_PLAY));
            client.Character.RefreshStats();
            client.Character.Teleport(client.Character.Record.MapId);
            #region Dofus Cinematic
            if (client.Character.IsNew)
            {
                client.Send(new CinematicMessage(10));
                client.Character.IsNew = false;
            }
            #endregion
        }
    }
}
