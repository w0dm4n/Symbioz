using Symbioz.DofusProtocol.Messages;
using Symbioz.Helper;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Enums;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Network.Servers;
using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Clients;
using Symbioz.World.Records.Alliances;
using Symbioz.World.Models.Alliances;
using Shader.Helper;
using Symbioz.World.Records;
using Symbioz.ORM;

namespace Symbioz.World.Models.Guilds
{
    public class GuildProvider : Singleton<GuildProvider>
    {
        public static GuildRightsBitEnum DEFAULT_JOIN_RANK = GuildRightsBitEnum.GUILD_RIGHT_NONE;
        public static byte EMOTE_ID = 97;

        public void CreateGuild(Character owner, GuildCreationValidMessage message)
        {
            GuildRecord guild = new GuildRecord(GuildRecord.PopNextId(), message.guildName, message.guildEmblem.symbolShape,
                   message.guildEmblem.symbolColor, message.guildEmblem.backgroundShape, message.guildEmblem.backgroundColor, 1, 0, 1, DateTime.Now, string.Empty);
            SaveTask.AddElement(guild, false);
            JoinGuild(guild, owner, GuildRightsBitEnum.GUILD_RIGHT_BOSS, (ushort)GuildRightsBitEnum.GUILD_RIGHT_BOSS);
        }

        public void JoinGuild(GuildRecord guild, Character character, GuildRightsBitEnum rights, ushort rank)
        {
            CharacterGuildRecord characterGuild = new CharacterGuildRecord(character.Id, guild.Id, rank, 0, 0, (uint)rights);
            characterGuild.AddElement(false);
            character.HumanOptions.Add(new HumanOptionGuild(guild.GetGuildInformations()));
            character.Client.Send(new GuildJoinedMessage(guild.GetGuildInformations(), (uint)rights, true));
            character.SetGuildLook();
            character.RefreshOnMapInstance();
            character.LearnEmote(GuildProvider.EMOTE_ID);

            if (character.HasAlliance)
            {
                character.HumanOptions.Add(new HumanOptionAlliance(character.GetAlliance().GetAllianceInformations(), (sbyte)0));
                character.Client.Send(new AllianceJoinedMessage(character.GetAlliance().GetAllianceInformations(), true));
                character.RefreshOnMapInstance();
                character.LearnEmote(AllianceProvider.EMOTE_ID);
            }
        }

        public void LeaveGuild(Character character)
        {
            CharacterGuildRecord.GetCharacterGuild(character.Id).RemoveElement(false);
            character.HumanOptions.RemoveAll(x => x is HumanOptionGuild);
            character.Client.Send(new GuildLeftMessage());
            AllianceRecord.OnCharacterLeftAlliance(character);
            character.RefreshOnMapInstance();
            character.ForgetEmote(GuildProvider.EMOTE_ID);
        }

        public bool HasGuild(int characterId)
        {
            return CharacterGuildRecord.HasGuild(characterId);
        }

        public static WorldClient[] GetMembers(int guildId)
        {
            List<WorldClient> clients = new List<WorldClient>();
            foreach (var member in CharacterGuildRecord.GetMembers(guildId))
            {
                clients.Add(WorldServer.Instance.GetOnlineClient((int)member.id));
            }
            clients.RemoveAll(x => x == null);
            return clients.ToArray();
        }

        public int ConnectedMembersCount(int guildId)
        {
            int count = 0;
            foreach (var member in CharacterGuildRecord.GetMembers(guildId))
            {
                if (WorldServer.Instance.IsConnected((int)member.id))
                    count++;
            }
            return count;
        }

        public static CharacterGuildRecord GetLeader(int guildId)
        {
            CharacterGuildRecord leader = null;
            foreach (CharacterGuildRecord member in CharacterGuildRecord.CharactersGuilds.FindAll(x => x.GuildId == guildId))
            {
                if (member.Rights == (uint)GuildRightsBitEnum.GUILD_RIGHT_BOSS)
                {
                    leader = member;
                }
            }
            return leader;
        }

        public static bool IsLeader(int characterId, int guildId)
        {
            bool isLeader = false;
            var characterLeader = GetLeader(guildId);
            if (characterLeader != null)
            {
                if (characterLeader.CharacterId == characterId)
                {
                    isLeader = true;
                }
            }
            return isLeader;

        }

        public static GuildFactSheetInformations GetGuildFactSheetInformations(GuildRecord guildRecord)
        {
            return new GuildFactSheetInformations((uint)guildRecord.Id, guildRecord.Name, guildRecord.GetEmblemObject(), (uint)GuildProvider.GetLeader(guildRecord.Id).CharacterId, (byte)guildRecord.Level, (ushort)CharacterGuildRecord.GetMembers(guildRecord.Id).Length);
        }

        public static IEnumerable<CharacterMinimalInformations> GetMembersInformations(int guildId)
        {
            List<CharacterMinimalInformations> characterMinimalInformations = new List<CharacterMinimalInformations>();
            var guildMembers = CharacterGuildRecord.GetMembers(guildId);
            if (guildMembers != null && guildMembers.Count() > 0)
            {
                foreach (var guildMember in guildMembers)
                {
                    var characterRecord = CharacterRecord.GetCharacterRecordById((int)guildMember.id);
                    characterMinimalInformations.Add(new CharacterMinimalInformations((uint)characterRecord.Id, characterRecord.Level, characterRecord.Name));
                }
            }
            return characterMinimalInformations;
        }

        public static GuildFactsMessage GetGuildFactsMessage(GuildRecord guildRecord)
        {
            //TODO: TaxCollectors
            if (guildRecord.IsInAlliance)
            {
                var guildAlliance = AllianceProvider.GetAllianceFromGuild(guildRecord.Id);
                return new GuildInAllianceFactsMessage(GetGuildFactSheetInformations(guildRecord), DateTimeUtils.GetEpochFromDateTime(guildRecord.CreationDate), 0, true, GetMembersInformations(guildRecord.Id), new BasicNamedAllianceInformations((uint)guildAlliance.Id, guildAlliance.Tag, guildAlliance.Name));
            }
            else
            {
                return new GuildFactsMessage(GetGuildFactSheetInformations(guildRecord), DateTimeUtils.GetEpochFromDateTime(guildRecord.CreationDate), 0, true, GetMembersInformations(guildRecord.Id));
            }
        }

        public List<WorldClient> GetClients(int guildId)
        {
            List<WorldClient> clients = new List<WorldClient>();
            foreach (var member in GetMembers(guildId))
            {
                var client = WorldServer.Instance.GetOnlineClient(member.Character.Id);
                if (client != null)
                {
                    clients.Add(member);
                }
            }
            return clients;
        }
    }
}
