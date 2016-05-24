using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Guilds
{
    [Table("CharactersGuilds")]
    public class CharacterGuildRecord : ITable
    {
        public static List<CharacterGuildRecord> CharactersGuilds = new List<CharacterGuildRecord>();

        [Primary]
        public int CharacterId;

        public int GuildId;
        [Update]
        public ushort Rank;
        [Update]
        public ulong GivenExperience;
        [Update]
        public sbyte ExperienceGivenPercent;
        [Update]
        public uint Rights;

        [Ignore]
        public GuildRightsBitEnum RealRights
        {
            get
            {
                return (GuildRightsBitEnum)this.Rights;
            }
            set
            {
                this.Rights = (uint)value;
            }
        }
        public CharacterGuildRecord(int characterId, int guildId, ushort rank, ulong givenExperience, sbyte experienceGivenPercent, uint rights)
        {
            this.CharacterId = characterId;
            this.GuildId = guildId;
            this.Rank = rank;
            this.GivenExperience = givenExperience;
            this.ExperienceGivenPercent = experienceGivenPercent;
            this.Rights = rights;
        }

        public static GuildMember[] GetMembers(int guildId)
        {
            return CharactersGuilds.FindAll(x => x.GuildId == guildId).ConvertAll<GuildMember>(x => x.GetGuildMember()).ToArray();
        }
        public GuildMember GetGuildMember()
        {
            sbyte connected = (sbyte)(WorldServer.Instance.IsConnected(CharacterId) ? 1 : 0);
            sbyte status = (sbyte)(WorldServer.Instance.IsConnected(CharacterId) ? WorldServer.Instance.GetOnlineClient(CharacterId).Character.PlayerStatus.statusId : 0);
            CharacterRecord cRecord = CharacterRecord.GetCharacterRecordById(CharacterId);
            return new GuildMember((uint)CharacterId, cRecord.Level, cRecord.Name, cRecord.Breed, cRecord.Sex, Rank, GivenExperience, ExperienceGivenPercent,
                Rights, connected, cRecord.AlignmentSide, 0, 0, cRecord.AccountId, 0, new PlayerStatus(status));
        }
        public void ChangeParameters(WorldClient changer, ushort rank, sbyte experienceGivenPercent, uint rights)
        {
            GuildRecord guild = changer.Character.GetGuild();
            if (guild == null || guild.Id != this.GuildId)
                return;
            CharacterGuildRecord modifier = CharacterGuildRecord.GetCharacterGuild(changer.Character.Id);
            if (modifier != this)
            {
                if (modifier.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_XP_CONTRIBUTION))
                {
                    this.ExperienceGivenPercent = experienceGivenPercent;
                }
            }
            else
            {
                if (this.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_MY_XP_CONTRIBUTION))
                {
                    this.ExperienceGivenPercent = experienceGivenPercent;
                }
            }
            if (modifier.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_RANKS))
            {
                this.Rank = rank;
            }
            if (modifier.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_RIGHTS))
            {
                this.Rights = rights;
            }
            SaveTask.UpdateElement(modifier);
        }
        public bool HasRight(GuildRightsBitEnum right)
        {
            return this.RealRights == GuildRightsBitEnum.GUILD_RIGHT_BOSS || this.RealRights.HasFlag(right);
        }
        public static bool HasGuild(int characterId)
        {
            return CharactersGuilds.Find(x => x.CharacterId == characterId) != null;
        }
        public static CharacterGuildRecord GetCharacterGuild(int characterId)
        {
            return CharactersGuilds.Find(x => x.CharacterId == characterId);
        }
        public static int MembersCount(int guildId)
        {
            return GetMembers(guildId).Length;
        }
        public static void RemoveAll(int characterId)
        {
            CharactersGuilds.FindAll(x => x.CharacterId == characterId).ForEach(x => x.RemoveElement());
        }
       
    }
}
