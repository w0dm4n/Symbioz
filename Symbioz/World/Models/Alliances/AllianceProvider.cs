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

namespace Symbioz.World.Models.Alliances
{
    public class AllianceProvider : Singleton<AllianceProvider>
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

        public static AllianceRecord CreateAlliance(GuildRecord creator, string tag, string name, GuildEmblem emblem)
        {
            AllianceRecord Alliance = new AllianceRecord(AllianceRecord.PopNextId(), name, tag, emblem.symbolColor, emblem.symbolShape, emblem.backgroundShape, emblem.backgroundColor, creator.Id);
            Alliance.AddElement();
            JoinAlliance(creator, Alliance, null);
            return Alliance;
        }

        public static void DeleteAlliance(int id)
        {
            AllianceRecord Alliance = AllianceProvider.GetAlliance(id);
            List<GuildAllianceRecord> AllianceMembers = GuildAllianceRecord.GuildsAlliances.FindAll(x => x.AllianceId == id);
            foreach(GuildAllianceRecord member in AllianceMembers)
            {
                LeaveAlliance(member.GuildId);
                member.RemoveElement();
            }
            Alliance.RemoveElement();
        }

        public static void JoinAlliance(GuildRecord Guild, AllianceRecord Alliance, WorldClient Inviter = null)
        {
            GuildAllianceRecord Gmember = new GuildAllianceRecord(Guild.Id, Alliance.Id);
            Gmember.AddElement();
            if(Inviter != null)
                Inviter.Send(new AllianceInvitationAnswerMessage(true));
            foreach(CharacterGuildRecord member in CharacterGuildRecord.CharactersGuilds.FindAll(x=>x.GuildId == Guild.Id))
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
            AllianceMember.RemoveElement();
        }

        public static IEnumerable<GuildInsiderFactSheetInformations> GetGuildsInsiderFactSheetInformations(int AllianceId)
        {
            List<GuildInsiderFactSheetInformations> guilds = new List<GuildInsiderFactSheetInformations>();
            List<GuildAllianceRecord> records = GuildAllianceRecord.GuildsAlliances.FindAll(x => x.AllianceId == AllianceId);
            foreach(GuildAllianceRecord record in records)
            {
                GuildRecord guild = GuildRecord.GetGuild(record.GuildId);
                GuildInsiderFactSheetInformations a = new GuildInsiderFactSheetInformations((uint)guild.Id, guild.Name, guild.GetEmblemObject(), (uint)guild.GetLeader().CharacterId, (byte)guild.Level, (ushort)GuildProvider.GetMembers(guild.Id).Length, CharacterRecord.GetCharacterRecordById(guild.GetLeader().CharacterId).Name, (ushort)GuildProvider.Instance.ConnectedMembersCount(guild.Id), 0, 0, true);
                guilds.Add(a);
            }
            return (IEnumerable<GuildInsiderFactSheetInformations>)guilds;
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

        public static AllianceInsiderInfoMessage GetAllianceInsiderInfoMessage(AllianceRecord alliance)
        {
            return new AllianceInsiderInfoMessage(new DofusProtocol.Types.AllianceFactSheetInformations((uint)alliance.Id, alliance.Tag, alliance.Name, new DofusProtocol.Types.GuildEmblem(alliance.SymbolShape, alliance.SymbolColor, alliance.BackgroundShape, alliance.BackgroundColor), DateTime.Now.Second), AllianceProvider.GetGuildsInsiderFactSheetInformations(alliance.Id), (IEnumerable<PrismSubareaEmptyInfo>)new List<PrismSubareaEmptyInfo>());
        }
    }
}
