using Symbioz.DofusProtocol.Types;
using Symbioz.World.Records.Alliances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Messages;
using Symbioz.World.Records.Guilds;
using Symbioz.Network.Clients;
using Symbioz.World.Models.Guilds;
using Symbioz.Network.Servers;
using Symbioz.World.Records;
using Symbioz.Helper;
using Symbioz.Enums;
using Shader.Helper;
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.ORM;

namespace Symbioz.World.Models.Alliances
{
    public class AllianceProvider
    {

        public static byte EMOTE_ID = 98;

        public static AllianceRecord GetAlliance(int allianceId)
        {
            return AllianceRecord.Alliances.Find(x => x.Id == allianceId);
        }

        public static bool CanCreateAlliance(WorldClient client, AllianceCreationValidMessage message)
        {
            if (!client.Character.HasGuild)
            {
                client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_LEAVE));
                return false;
            }
            if (client.Character.HasAlliance)
            {
                client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD));
                return false;
            }
            if (message.allianceTag.Length < 3 || message.allianceTag.Length > 5)
            {
                client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET));
                return false;
            }

            if (AllianceRecord.NameTaked(message.allianceName) || AllianceRecord.TagTaked(message.allianceTag))
            {
                client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_ALREADY_EXISTS));
                return false;
            }

            if (GuildRecord.GetGuild(client.Character.GuildId).GetLeader().CharacterId != client.Character.Id)
            {
                client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET));
                return false;
            }
            return true;
        }

        public static void CreateAlliance(GuildRecord creator, string tag, string name, GuildEmblem emblem)
        {
            AllianceRecord newAlliance = new AllianceRecord(AllianceRecord.PopNextId(), name, tag, emblem.symbolColor, emblem.symbolShape, emblem.backgroundColor, emblem.backgroundShape, creator.Id, DateTime.Now, string.Empty);
            SaveTask.AddElement(newAlliance, false);
            JoinAlliance(creator, newAlliance, null);
        }

        public static void DeleteAlliance(int id)
        {
            AllianceRecord currentAlliance = AllianceProvider.GetAlliance(id);
            List<GuildAllianceRecord> allianceMembers = GuildAllianceRecord.GuildsAlliances.FindAll(x => x.AllianceId == id);
            foreach(GuildAllianceRecord member in allianceMembers)
            {
                LeaveAlliance(member.GuildId);
                member.RemoveElement(false);
            }
            currentAlliance.RemoveElement(false);
        }

        public static void JoinAlliance(GuildRecord Guild, AllianceRecord Alliance, WorldClient Inviter = null)
        {
            GuildAllianceRecord guildAllianceMember = new GuildAllianceRecord(Guild.Id, Alliance.Id);
            guildAllianceMember.AddElement(false);
            if(Inviter != null)
                Inviter.Send(new AllianceInvitationAnswerMessage(true));
            foreach(CharacterGuildRecord member in CharacterGuildRecord.CharactersGuilds.FindAll(x => x.GuildId == Guild.Id))
            {
                AllianceRecord.OnCharacterJoinAlliance(CharacterRecord.GetCharacterRecordById(member.CharacterId), Alliance);
            }
        }

        public static void LeaveAlliance(int guildId)
        {
            GuildAllianceRecord AllianceMember = GuildAllianceRecord.GuildsAlliances.Find(x => x.GuildId == guildId);
            List<CharacterGuildRecord> guildMembers = CharacterGuildRecord.CharactersGuilds.FindAll(x=>x.GuildId == guildId);
            foreach(CharacterGuildRecord guildMember in guildMembers)
            {
                Character record = WorldServer.Instance.GetOnlineClient(guildMember.CharacterId).Character;
                AllianceRecord.OnCharacterLeftAlliance(record);
            }
            AllianceMember.RemoveElement(false);
        }

        public static IEnumerable<GuildInsiderFactSheetInformations> GetGuildsInsiderFactSheetInformations(int allianceId)
        {
            List<GuildInsiderFactSheetInformations> guilds = new List<GuildInsiderFactSheetInformations>();

            GuildInsiderFactSheetInformations leaderGuildInformations = null;
            var allianceGuildLeader = AllianceProvider.GetLeader(allianceId);

            List<GuildAllianceRecord> records = GuildAllianceRecord.GuildsAlliances.FindAll(x => x.AllianceId == allianceId);
            foreach(GuildAllianceRecord record in records)
            {
                GuildRecord guild = GuildRecord.GetGuild(record.GuildId);
                GuildInsiderFactSheetInformations guildInsiderFactSheetInformations = new GuildInsiderFactSheetInformations((uint)guild.Id, guild.Name,
                    guild.GetEmblemObject(), (uint)guild.GetLeader().CharacterId, (byte)guild.Level, (ushort)GuildProvider.GetMembers(guild.Id).Length,
                    CharacterRecord.GetCharacterRecordById(guild.GetLeader().CharacterId).Name, (ushort)GuildProvider.Instance.ConnectedMembersCount(guild.Id),
                    0, 0, true);

                if(allianceGuildLeader.Id == guild.Id)
                {
                    leaderGuildInformations = guildInsiderFactSheetInformations;
                }
                else
                {
                    guilds.Add(guildInsiderFactSheetInformations);
                }
            }

            if(leaderGuildInformations != null)
            {
                guilds.Insert(0, leaderGuildInformations);
            }

            return guilds;
        }

        public static IEnumerable<GuildInAllianceInformations> GetGuildsInAllianceInformations(int allianceId)
        {
            List<GuildInAllianceInformations> guildsInAllianceInformations = new List<GuildInAllianceInformations>();
            List<GuildAllianceRecord> records = GuildAllianceRecord.GuildsAlliances.FindAll(x => x.AllianceId == allianceId);
            foreach (GuildAllianceRecord record in records)
            {
                GuildRecord guild = GuildRecord.GetGuild(record.GuildId);
                GuildInAllianceInformations guildInAllianceInformations = new GuildInAllianceInformations((uint)guild.Id, guild.Name,
                    guild.GetEmblemObject(), (byte)guild.Level, (byte)CharacterGuildRecord.GetMembers(guild.Id).Length, true);
                guildsInAllianceInformations.Add(guildInAllianceInformations);
            }
            return guildsInAllianceInformations;
        }

        public static AllianceRecord GetAllianceFromGuild(int guildId)
        {
            GuildAllianceRecord allianceMember = GuildAllianceRecord.GetCharacterAlliance(guildId);
            if (allianceMember == null)
                return null;
           return AllianceProvider.GetAlliance(allianceMember.AllianceId);
        }

        public static GuildRecord GetLeader(int allianceId)
        {
            AllianceRecord alliance = GetAlliance(allianceId);
            return GuildRecord.GetGuild(alliance.LeaderGuildId);
        }

        public static bool IsLeader(int characterId, int guildId, int allianceId)
        {
            bool isLeader = false;
            var guildLeader = GetLeader(allianceId);
            if(guildLeader != null)
            {
                var characterGuild = GuildRecord.GetGuild(guildId);
                if (guildLeader == characterGuild && GuildProvider.IsLeader(characterId, characterGuild.Id))
                {
                    isLeader = true;
                }
            }
            return isLeader;
        }

        public static AllianceInsiderInfoMessage GetAllianceInsiderInfoMessage(AllianceRecord alliance)
        {
            return new AllianceInsiderInfoMessage(new DofusProtocol.Types.AllianceFactSheetInformations((uint)alliance.Id, alliance.Tag, alliance.Name, new DofusProtocol.Types.GuildEmblem(alliance.SymbolShape, alliance.SymbolColor, alliance.BackgroundShape, alliance.BackgroundColor), DateTimeUtils.GetEpochFromDateTime(alliance.CreationDate)), AllianceProvider.GetGuildsInsiderFactSheetInformations(alliance.Id), new List<PrismSubareaEmptyInfo>());
        }

        public static AllianceFactsMessage GetAllianceFactsMessage(AllianceRecord alliance)
        {
            var leaderCharacterId = GuildProvider.GetLeader(alliance.LeaderGuildId).CharacterId;
            var leaderCharacterName = CharacterRecord.GetCharacterRecordById(leaderCharacterId).Name;
            List<ushort> controlledSubAreaIds = new List<ushort>();
            PrismRecord.GetAlliancePrisms(alliance.Id).ForEach(x => controlledSubAreaIds.Add((ushort)x.SubAreaId));
            return new AllianceFactsMessage(new AllianceFactSheetInformations((uint)alliance.Id, alliance.Tag, alliance.Name, alliance.GetEmblemObject(),
                DateTimeUtils.GetEpochFromDateTime(alliance.CreationDate)),
                AllianceProvider.GetGuildsInAllianceInformations(alliance.Id),
                controlledSubAreaIds, (uint)leaderCharacterId, leaderCharacterName);
        }

        public static int GetGuildsCount(int allianceId)
        {
            return GuildAllianceRecord.GuildsAlliances.FindAll(x => x.AllianceId == allianceId).Count;
        }

        public static int GetMembersCount(int allianceId)
        {
            int membersCount = 0;
            List<GuildAllianceRecord> records = GuildAllianceRecord.GuildsAlliances.FindAll(x => x.AllianceId == allianceId);
            foreach (GuildAllianceRecord record in records)
            {
                membersCount = (membersCount + CharacterGuildRecord.GetMembers(record.GuildId).Length);
            }
            return membersCount;
        }

        public static List<WorldClient> GetClients(int allianceId)
        {
            List<WorldClient> clients = new List<WorldClient>();
            List<GuildAllianceRecord> records = GuildAllianceRecord.GuildsAlliances.FindAll(x => x.AllianceId == allianceId);
            foreach (GuildAllianceRecord record in records)
            {
                foreach (GuildMember guildMember in CharacterGuildRecord.GetMembers(record.GuildId))
                {
                    var client = WorldServer.Instance.GetOnlineClient((int)guildMember.id);
                    if (client != null)
                    {
                        clients.Add(client);
                    }
                }
            }
            return clients;
        }

        public static bool GuildIsInAlliance(int guildId, int allianceId)
        {
            bool isInAlliance = false;
            if(GuildAllianceRecord.GuildsAlliances.FirstOrDefault(x => x.AllianceId == allianceId && x.GuildId == guildId) != null)
            {
                isInAlliance = true;
            }
            return isInAlliance;
        }
    }
}
