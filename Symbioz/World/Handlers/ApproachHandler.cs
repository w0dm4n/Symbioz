using Symbioz;
using Symbioz.Auth;
using Symbioz.Auth.Handlers;
using Symbioz.Auth.Models;
using Symbioz.Core;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.ORM;
using Symbioz.Utils;
using Symbioz.World.Models;
using Symbioz.World.Models.Fights;
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
            bool reco = false;
            Character areco = null;

            foreach (CharacterRecord c in client.Characters)
            {
                if (c == null)
                    continue;
                if (CharactersDisconnected.is_disconnected(c.Name)
                    && CharactersDisconnected.get_Character_disconnected(c.Name).FighterInstance != null
                    && CharactersDisconnected.get_Character_disconnected(c.Name).FighterInstance.Fight != null)
                {
                    reco = true;
                    areco = CharactersDisconnected.get_Character_disconnected(c.Name);
                    break;
                }
            }
            if (reco == true)
            {
                client.Character = areco;
                client.Character.FighterInstance.Client = client;
                client.Character.Client = client;
                ProcessSelection(client);
                return;
            }
            if (ConfigurationManager.Instance.ServerId == 22)
                client.Send(new CharactersListMessage(client.Characters.ConvertAll<CharacterHardcoreOrEpicInformations>(x => x.GetHardcoreOrEpicInformations()), false));
            else
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
            CharacterStatsRecord.Create(client.Character);
            client.Character.SetLevel(ConfigurationManager.Instance.StartLevel);
            client.Character.Record.CurrentLifePoint = client.Character.CurrentStats.LifePoints;
            client.Character.UpdateBreedSpells();
            client.Character.LearnAllJobs();

            /* GIVE KEYRING AT CHARACTER CREATION */
            client.Character.Inventory.Add(10207, 1);

            /* Panoplie aventurier */
            client.Character.Inventory.Add(2473, 1);
            client.Character.Inventory.Add(2474, 1);
            client.Character.Inventory.Add(2475, 1);
            client.Character.Inventory.Add(2476, 1);
            client.Character.Inventory.Add(2477, 1);
            client.Character.Inventory.Add(2478, 1);

            Logger.Log("Character " + newCharacter.Name + " created!");
            SaveTask.AddElement(client.Character.Record, client.CharacterId);
            ProcessSelection(client);
            byte[] data = Convert.FromBase64String(ConfigurationManager.Instance.WelcomeSystemMessage);
            string decodedString = Encoding.UTF8.GetString(data);
            client.Send(new SystemMessageDisplayMessage(true, 61, new List<string> { decodedString }));
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
            Logger.Log("Character " + replayCharacter.Name + " replay!");
            ProcessSelection(client);
        }
        [MessageHandler]
        public static void HandleCharacterDeletion(CharacterDeletionRequestMessage message, WorldClient client) // finish this
        {
            CharacterRecord deletedCharacter = CharacterRecord.GetCharacterRecordById(message.characterId);
            if (deletedCharacter == null)
                return;
            CharacterStatsRecord.GetCharacterStatsRecord(message.characterId).RemoveElementWithoutDelay();
            CharacterRecord.Characters.Remove(deletedCharacter);
            client.Characters.Remove(deletedCharacter);
            deletedCharacter.RemoveElementWithoutDelay();
            CharacterItemRecord.RemoveAll(message.characterId);
            GeneralShortcutRecord.RemoveAll(message.characterId);
            SpellShortcutRecord.RemoveAll(message.characterId);
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
            if (client.Character.FighterInstance != null && client.Character.FighterInstance.Fight != null)
            {
                client.Character.Inventory.Refresh();
                client.Character.RefreshShortcuts();
                client.Character.RefreshEmotes();
                client.Character.InitializeCosmetics();
                client.Character.RefreshJobs();
                client.Character.RefreshSpells();
                client.Character.RefreshArenasInfos();
                client.Character.RefreshPvPInfos();
                client.Character.OnConnectedNotifications();
                client.Send(new CharacterCapabilitiesMessage(4095));
                client.Send(new CharacterLoadingCompleteMessage());
                return;
            }
            CharacterStatsRecord.InitializeCharacter(client.Character);
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
        public static void HandleGameContextReady(GameContextReadyMessage message, WorldClient client)
        {
            Logger.Write("[" + client.Character.Record.Name + "] vien de se reconnecter en combats sur la map " + client.Character.FighterInstance.Fight.Map.Id, ConsoleColor.DarkCyan);
            client.Character.ReCreateFighter(client.Character.FighterInstance.Team);
            client.Send(new GameFightJoinMessage(false, true, false, Fight.FIGHT_PREPARATION_TIME, (sbyte)client.Character.FighterInstance.Fight.FightType));
            if (!client.Character.FighterInstance.Fight.Started)
                client.Character.FighterInstance.Fight.ShowPlacementCells();
            client.Send(new GameFightShowFighterMessage(client.Character.FighterInstance.FighterInformations));
            client.Send(new GameEntitiesDispositionMessage(client.Character.FighterInstance.Fight.GetAllFighters().ConvertAll<IdentifiedEntityDispositionInformations>(x => x.GetIdentifiedEntityDisposition())));
            client.Send(new GameFightResumeMessage(new List<FightDispellableEffectExtendedInformations>(), new List<GameActionMark>(), (ushort)client.Character.FighterInstance.Fight.TimeLine.m_round, client.Character.FighterInstance.Fight.Started ? 1 : 0, new List<Idol>(), new List<GameFightSpellCooldown>(), 0, 0));
            client.Send(new GameFightTurnListMessage(client.Character.FighterInstance.Fight.TimeLine.GenerateTimeLine(), new int[0]));
            client.Send(new GameFightSynchronizeMessage(client.Character.FighterInstance.Fight.GetAllFighters().ConvertAll<GameFightFighterInformations>(x => x.FighterInformations)));
            client.Character.FighterInstance.Fight.FighterReconnect(client.Character.FighterInstance);
            client.Send(new MapComplementaryInformationsDataMessage(client.Character.SubAreaId, client.Character.FighterInstance.Fight.Map.Id, new List<HouseInformations>(),
            new List<GameRolePlayActorInformations>(), client.Character.FighterInstance.Fight.Map.Instance.GetInteractiveElements(), new List<StatedElement>(),
            new List<MapObstacle>(), new List<FightCommonInformations>()));
            if (client.Character.FighterInstance.IsPlaying)
                client.Send(new GameFightTurnStartMessage(client.Character.FighterInstance.ContextualId, FightTurnEngine.TURN_TIMEMOUT / 100));
            else
                client.Send(new GameFightTurnEndMessage(client.Character.FighterInstance.ContextualId));
            client.Send(new GameFightStartMessage(new Idol[0])); // see
            client.Send(new GameFightTurnListMessage(client.Character.FighterInstance.Fight.TimeLine.GenerateTimeLine(), new int[0]));
            CharactersDisconnected.remove(client.Character.Record.Name);
        }

        [MessageHandler]
        public static void HandleGameContextCreate(GameContextCreateRequestMessage message, WorldClient client)
        {
            if (client.Character.FighterInstance != null && client.Character.FighterInstance.Fight != null)
            {
                client.Character.RefreshStats();
                client.Send(new GameContextDestroyMessage());
                client.Send(new GameContextCreateMessage((sbyte)GameContextEnum.FIGHT));
                MapsHandler.SendCurrentMapMessage(client, client.Character.FighterInstance.Fight.Map.Id);
                return;
            }
            client.Send(new GameContextDestroyMessage());
            client.Send(new GameContextCreateMessage((sbyte)GameContextEnum.ROLE_PLAY));
            client.Character.RefreshStats();
            client.Character.OnConnectedBasicActions();
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
