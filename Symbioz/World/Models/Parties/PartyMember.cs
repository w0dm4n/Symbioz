using Symbioz.DofusProtocol.Types;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.World.PathProvider;
using Symbioz.World.Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Parties
{
    public class PartyMember
    {
        public Character Character;
        public Party Party;
        public bool Loyal = false;
        public PartyMember(Character c, Party p)
        {
            this.Party = p;
            this.Character = c;
        }
        public PartyMemberInformations GetPartyMemberInformations()
        {
            CharacterRecord record = this.Character.Record;
            StatsRecord stats = this.Character.StatsRecord;
            BasicStats current = this.Character.CurrentStats;
            int id = record.Id;
            int level = (int)record.Level;
            string name = record.Name;
            EntityLook entityLook = this.Character.Look.ToEntityLook();
            int breed = (int)(sbyte)record.Breed;
            bool sex = record.Sex;
            int hp = (int)this.Character.CurrentStats.LifePoints;
            int maxhp = stats.LifePoints;
            int regen = (int)1;
            int align = (int)(sbyte)record.AlignmentSide;
            int mapid = (int)(short)record.MapId;
            PlayerStatus status = this.Character.PlayerStatus;
            PartyCompanionMemberInformations[] memberInformationsArray = new PartyCompanionMemberInformations[0];
            return new PartyMemberInformations((uint)id, (byte)level, name, entityLook, 
                (sbyte)breed, sex, (uint)hp, (uint)maxhp, (ushort)stats.Prospecting,
                (byte)regen, (ushort)this.Character.Initiative, (sbyte)align, (short)this.Character.Map.Position.x, (short)this.Character.Map.Position.y, this.Character.Map.Id, (ushort)this.Character.SubAreaId, status, (IEnumerable<PartyCompanionMemberInformations>)memberInformationsArray);
        }
        public PartyInvitationMemberInformations GetPartyInvitationMemberInformations()
        {
            CharacterRecord record = this.Character.Record;
            StatsRecord stats = this.Character.StatsRecord;
            BasicStats current = this.Character.CurrentStats;
            int id = record.Id;
            int level = (int)record.Level;
            string name = record.Name;
            EntityLook entityLook = this.Character.Look.ToEntityLook();
            int breed = (int)(sbyte)record.Breed;
            bool sex = record.Sex;
            int mapid = (int)(short)record.MapId;
            PartyCompanionMemberInformations[] memberInformationsArray = new PartyCompanionMemberInformations[0];

            return new PartyInvitationMemberInformations((uint)id, (byte)level, 
                name, entityLook, (sbyte)breed, sex,(short)this.Character.Map.Position.x, (short)this.Character.Map.Position.y, this.Character.Map.Id,
                (ushort)this.Character.SubAreaId, memberInformationsArray);
        }

        public void SetLoyalty(bool value)
        {
            this.Loyal = value;
            this.Character.Client.Send(new PartyLoyaltyStatusMessage((uint)this.Party.Id, value));
        }
    }
}
