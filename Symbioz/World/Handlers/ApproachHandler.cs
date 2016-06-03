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
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.Utils;
using Symbioz.World.Models;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Records;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections;
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
            if (ConfigurationManager.Instance.ServerId == 22)
                client.Send(new CharactersListMessage(client.Characters.ConvertAll<CharacterHardcoreOrEpicInformations>(x => x.GetHardcoreOrEpicInformations()), false));
            else
                client.Send(new CharactersListMessage(client.Characters.ConvertAll<CharacterBaseInformations>(x => x.GetBaseInformation()), false)); // StartupActions ?
            Boolean reco = false;
            CharacterRecord areco = null;
            foreach (CharacterRecord c in client.Characters)
            {
                if (c.InFight != -1)
                {
                    reco = true;
                    areco = c;
                }
            }
            if (reco == true)
            {
                Logger.Write("RECO IN FIGHT ID=" + areco.InFight, ConsoleColor.DarkCyan);
                client.Character = new Character(areco,client);
                ProcessSelection(client);
                //client.Character.FighterInstance.Fight.FighterReconnect(client.Character.FighterInstance);
                return;
            }
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
            client.Character.AddElement(client.Character.Record);
            client.Character.UpdateBreedSpells();
            client.Character.LearnAllJobs();
            Logger.Log("Character " + newCharacter.Name + " created!");
            client.Character.Save();
            ProcessSelection(client);
        }
        [MessageHandler]
        public static void HandleCharacterReplayRequest(CharacterReplayRequestMessage message, WorldClient client)
        {
            CharacterRecord replayCharacter = CharacterRecord.GetCharacterRecordById(message.characterId);

            if (replayCharacter == null)
                return;
            CharacterItemRecord.RemoveAll(message.characterId);
            GeneralShortcutRecord.RemoveAll(message.characterId);
            CharacterSpellRecord.RemoveAll(message.characterId);
            CharacterJobRecord.RemoveAll(message.characterId);
            BidShopGainRecord.RemoveAll(message.characterId);
            replayCharacter.Energy = 10000;
            replayCharacter.Level = ConfigurationManager.Instance.StartLevel;
            replayCharacter.MapId = ConfigurationManager.Instance.StartMapId;
            replayCharacter.CellId = ConfigurationManager.Instance.StartCellId;
            replayCharacter.SpellPoints = ConfigurationManager.Instance.StartLevel;
            replayCharacter.StatsPoints = ConfigurationManager.Instance.StartLevel;
            replayCharacter.StatsPoints *= 5;
            replayCharacter.Kamas = ConfigurationManager.Instance.StartKamas;
            replayCharacter.Honor = 0;
            replayCharacter.AlignmentValue = 0;
            client.Character = new Character(CharacterRecord.GetCharacterRecordById(message.characterId), client);
            client.Character.Look = ContextActorLook.Parse(client.Character.Record.OldLook);
            //string look = BreedRecord.GetBreedEntityLook((int)replayCharacter.Breed, replayCharacter.Sex, (int)0, null).ConvertToString();
            //client.Send(new CharactersListMessage(client.Characters.ConvertAll<CharacterHardcoreOrEpicInformations>(x => x.GetHardcoreOrEpicInformations()), false));
            Logger.Log("Character " + replayCharacter.Name + " replay!");
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

            if (ConfigurationManager.Instance.ServerId == 22)
                client.Send(new CharactersListMessage(client.Characters.ConvertAll<CharacterHardcoreOrEpicInformations>(x => x.GetHardcoreOrEpicInformations()), false));
            else
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
            if (client.Character.Record.InFight != -1)
            {
                Character old = null;
                foreach (WorldClient c in WorldServer.Instance.GetAllClientsOnline())
                {
                    if (c.Character == null)
                        continue;
                    if (c.Character.Id == client.Character.Id && c != client)
                    {
                        old = c.Character;
                        break;
                    }
                }
                if (old != null && old.FighterInstance != null)
                {
                    /*client.Character.ReCreateFighter(old.FighterInstance.Team);
                    client.Character.FighterInstance.CellId = old.FighterInstance.CellId;
                    client.Character.FighterInstance.Fight = old.FighterInstance.Fight;
                    client.Character.FighterInstance.IsPlaying = old.FighterInstance.IsPlaying;
                    client.Character.FighterInstance.Dead = old.FighterInstance.Dead;
                    client.Character.FighterInstance.Buffs = old.FighterInstance.Buffs;
                    client.Character.FighterInstance.BuffIdProvider = old.FighterInstance.BuffIdProvider;
                    client.Character.FighterInstance.Direction = old.FighterInstance.Direction;
                    client.Character.FighterInstance.FighterInformations = old.FighterInstance.FighterInformations;
                    client.Character.FighterInstance.FighterStats = old.FighterInstance.FighterStats;
                    if (client.Character.FighterInstance.Fight.BlueTeam != null
                        && old.FighterInstance.Team.Id == client.Character.FighterInstance.Fight.BlueTeam.Id)
                    {
                        client.Character.FighterInstance.Fight.BlueTeam.AddFighter(client.Character.FighterInstance);
                        client.Character.FighterInstance.Fight.BlueTeam.RemoveFighter(old.FighterInstance);
                        client.Character.FighterInstance.Fight.Fighterdeleted(old.FighterInstance);
                    }
                    else
                    {
                        client.Character.FighterInstance.Fight.RedTeam.AddFighter(client.Character.FighterInstance);
                        client.Character.FighterInstance.Fight.RedTeam.RemoveFighter(old.FighterInstance);
                        client.Character.FighterInstance.Fight.Fighterdeleted(old.FighterInstance);
                    }*/
                    client.Character = null;
                    client.Character = old;
                    old.Client = client;
                    client.Character.FighterInstance.Client = client;
                    WorldServer.Instance.RemoveClient(old.Client);
                    client.Character.Client = client;
                    client.Character.ReCreateFighter(client.Character.FighterInstance.Team);
                    client.Character.FighterInstance.Fight.FighterReconnect(client.Character.FighterInstance);
                }
            }
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
