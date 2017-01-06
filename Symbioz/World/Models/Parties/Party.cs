using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.World.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Parties
{
    public class Party
    {
        public sbyte MAX_MEMBER_COUNT = 8;
        public PartyTypeEnum Type = PartyTypeEnum.PARTY_TYPE_CLASSICAL;

        public uint Id { get; set; }

        public Character Leader { get; set; }

        public List<Character> Members = new List<Character>();
        public List<Character> Guests = new List<Character>();
        private Dictionary<Character, List<Character>> Followed;

        public int MembersCount { get { return Members.Count(); } }

        public int Count { get { return Members.Count() + Guests.Count(); } }

        public bool IsFull { get { return MembersCount >= MAX_MEMBER_COUNT; } }

        public Party(Character leader)
        {
            var id = PartyProvider.GetIdParty();
            Id = (uint)id;
            Leader = leader;
            AddMember(leader);
            Followed = new Dictionary<Character, List<Character>>();
        }

        public void AddMember(Character character)
        {
            if(Count < MAX_MEMBER_COUNT)
            {
                if (Guests.Contains(character))
                    Guests.Remove(character);
                Members.Add(character);
                for (int i = 0; i < Members.Count; i++)
                {
                    Members[i].Client.Send(new PartyNewMemberMessage(Id, character.GetPartyMemberInformations()));
                }
                PartyHandler.SendPartyJoinMessage(character.Client, this);
            }       
        }

        public void RemoveMember(Character character, bool isKicked)
        {
            if(isKicked)
            {
                character.Client.Send(new PartyKickedByMessage(Id, (uint)Leader.Id));
            }
            else
            {
                character.Client.Send(new PartyLeaveMessage(Id));
            }
            if(Members.Contains(character))
            {
                RemoveFollowers(character);
                Members.Remove(character); 
                for (int i = 0; i < Members.Count; i++)
                {
                    Members[i].Client.Send(new PartyMemberRemoveMessage(Id, (uint)character.Id));
                }
                if (character == Leader && Members.Count > 0)
                    ChangeLeader(Members.First());
                if (this.Count < 2 && Guests.Count < 1)
                    Disband();
            }
        }
        
        public void Kick(Character kicked)
        {
            if (Members.Contains(kicked))
            {
                RemoveMember(kicked, true);
                if (MembersCount <= 1 && Guests.Count < 1)
                {
                    Disband();
                }
                else
                {
                    if (Leader == kicked)
                        ChangeLeader(Members.First());
                }
            }   
        }

        public void AddGuest(Character character)
        {
            if(!Members.Contains(character) && Count < MAX_MEMBER_COUNT)
            {
                Guests.Add(character);
                foreach(var member in Members)
                    member.Client.Send(new PartyNewGuestMessage(Id, character.GetPartyGuestInformations(this)));
            }
        }

        public void RemoveGuest(Character character)
        {
            if(Guests.Contains(character))
            {
                Guests.Remove(character);
                if(Count < 2 && Guests.Count() <= 2)
                {
                    Disband();
                }
            }
        }

        public void ChangeLeader(Character newLeader)
        {
            if(newLeader.Party == this && newLeader != Leader)
            {
                Leader = newLeader;
                for (int i = 0; i < Members.Count; i++)
                {
                    Members[i].Client.Send(new PartyLeaderUpdateMessage(Id, (uint)newLeader.Id));
                }
            }
        }

        public void StartFollowPlayer(Character follower, int playerId)
        {
            var target = Members.FirstOrDefault(x => x.Id == playerId);
            if(target != null)
            {
                if (this.Followed.ContainsKey(target))
                    this.Followed[target].Add(follower);
                else
                    this.Followed.Add(target, new List<Character> { follower });
                follower.Client.Send(new CompassUpdatePartyMemberMessage((sbyte)CompassTypeEnum.COMPASS_TYPE_PARTY, new MapCoordinates((short)target.Map.WorldX, (short)target.Map.WorldY), (uint)playerId));
                follower.Client.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 368, new string[] { target.Name }));
                target.Client.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 52, new string[] { follower.Name }));
                follower.Client.Send(new PartyFollowStatusUpdateMessage(this.Id, target != null, target == null ? 0 : (uint)target.Id));
            }
        }

        public void StopFollowingPlayer(Character character)
        {
            var clients = new List<Character>();
            foreach (var pair in this.Followed)
            {
                if (pair.Value.Contains(character))
                {
                    pair.Value.Remove(character);
                    if (pair.Value.Count <= 0)
                        clients.Add(pair.Key);
                    pair.Key.Client.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 53, new List<string> { character.Name }));
                    character.Client.Send(new CompassResetMessage((sbyte)CompassTypeEnum.COMPASS_TYPE_PARTY));
                    character.Client.Send(new PartyFollowStatusUpdateMessage(this.Id, true, 0));
                    character.Client.Send(new PartyFollowStatusUpdateMessage(this.Id, false, 0));
                }
            }
            clients.ForEach(x => this.Followed.Remove(x));
        }

        private void RemoveFollowers(Character character)
        {
            if (this.Followed.ContainsKey(character))
            {
                this.Followed[character].ForEach(x =>
                {
                    x.Client.Send(new PartyFollowStatusUpdateMessage(this.Id, true, 0));
                    x.Client.Send(new PartyFollowStatusUpdateMessage(this.Id, false, 0));
                    x.Client.Send(new CompassResetMessage((sbyte)CompassTypeEnum.COMPASS_TYPE_PARTY));
                });

                this.Followed.Remove(character);
            }
            foreach (var pair in this.Followed)
            {
                if (pair.Value.Contains(character))
                {
                    pair.Value.Remove(character);
                    pair.Key.Client.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 53,
                        new List<string> { character.Name }));
                }
            }
            this.Followed.Remove(character);
        }

        public void UpdateFollowingMap(Character character)
        {
            if (!this.Followed.ContainsKey(character))
                return;
            this.Followed[character].ForEach(x => x.Client.Send(new CompassUpdatePartyMemberMessage(
                (sbyte)CompassTypeEnum.COMPASS_TYPE_PARTY,
               new MapCoordinates((short)character.Map.WorldX, (short)character.Map.WorldY), (uint)character.Id)));
        }

        public void StartFollowingThisPlayer(Character character, int playerId)
        {
            var target = this.Members.FirstOrDefault(x => x.Id == playerId);
            var members = this.Members.Where(x => x.Id != playerId);
            if (target != null)
            {
                foreach (var member in members)
                {
                    StartFollowPlayer(member, playerId);
                }
            }
        }
        public void StopFollowingPlayerThis(int playerId)
        {
            var target = this.Members.FirstOrDefault(x => x.Id == playerId);
            var members = this.Members.Where(x => x.Id != playerId);
            if (target != null)
            {
                foreach (var member in members)
                {
                    StopFollowingPlayer(member);
                }
            }
        }

        public void Disband()
        {
            for (int i = 0; i < Members.Count; i++)
            {
                Members[i].Client.Send(new PartyDeletedMessage(Id));
                Members[i].Party = null;
            }
            for (int i = 0; i < Guests.Count; i++)
            {
                Guests[i].Client.Send(new PartyDeletedMessage(Id));
                Guests[i].Client.Send(new PartyInvitationCancelledForGuestMessage(Id, (uint)Guests[i].Id));
                Guests[i].RemoveInvitation(this);
            }
            PartyProvider.Remove(this);
        }
    }
}
