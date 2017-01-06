using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.World.Models;
using Symbioz.World.Models.Parties;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class PartyHandler
    {
        [MessageHandler]
        public static void RequestParty(PartyInvitationRequestMessage message, WorldClient client)
        {
            WorldClient target = WorldServer.Instance.GetOnlineClient(message.name);
            if(target.Character == null)
            {
                client.Send(new PartyCannotJoinErrorMessage(0, (sbyte)PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_NOT_FOUND));
                return;
            }
            else
            {
                if (target.Character.IsIgnoring(client.Character.Record.AccountId))
                {
                    client.Send(new TextInformationMessage(1, 370, new string[1] { target.Character.Record.Name }));
                    return;
                }
                if (client.Character.Party == null)
                {
                    client.Character.Party = new Party(client.Character);
                }
                var invitation = new PartyInvitation(client.Character, target.Character, client.Character.Party);
                invitation.Show();
            }
        }
        [MessageHandler]
        public static void PartyAcceptInvitation(PartyAcceptInvitationMessage message, WorldClient client)
        {
            PartyInvitation partyInvitation = client.Character.GetPartyInvitation(message.partyId);
            if (partyInvitation != null)
                partyInvitation.Accept();
        }
        [MessageHandler]
        public static void PartyRefusedInvitation(PartyRefuseInvitationMessage message, WorldClient client)
        {
            PartyInvitation partyInvitation = client.Character.GetPartyInvitation(message.partyId);
            if (partyInvitation != null)
                partyInvitation.Refuse();
        }
        [MessageHandler]
        public static void PartyCancelInvitation(PartyCancelInvitationMessage message, WorldClient client)
        {
            var target = client.Character.Party.Guests.Find(x => x.Id == message.guestId);
            if (target != null)
            {
                var partyInvitation = target.GetPartyInvitation(message.partyId);
                if (partyInvitation != null)
                    partyInvitation.Cancel();
            }
        }
        [MessageHandler]
        public static void PartyLeaveRequest(PartyLeaveRequestMessage message, WorldClient client)
        {
            if(client.Character.IsInParty())
            {
                client.Character.QuitParty(false);
            }
        }
        [MessageHandler]
        public static void PartyAbdicateRequest(PartyAbdicateThroneMessage message, WorldClient client)
        {
            if(client.Character.IsInParty() && client.Character.IsPartyLeader())
            {
                client.Character.Party.ChangeLeader(client.Character.Party.Members.Find(x => x.Id == message.playerId));
            }
        }
        [MessageHandler]
        public static void PartyKickRequest(PartyKickRequestMessage message, WorldClient client)
        {
            if(client.Character.IsInParty() && client.Character.IsPartyLeader())
            {
                client.Character.Party.Kick(client.Character.Party.Members.Find(x => x.Id == message.playerId));
            }
        }
        [MessageHandler]
        public static void PartyGetInvitationDetailsRequest(PartyInvitationDetailsRequestMessage message, WorldClient client)
        {
            PartyInvitation partyInvitation = client.Character.GetPartyInvitation(message.partyId);
            if (partyInvitation != null)
            {
                client.Send(new PartyInvitationDetailsMessage(partyInvitation.Party.Id, (sbyte)PartyTypeEnum.PARTY_TYPE_CLASSICAL, "", (uint)partyInvitation.Source.Id, partyInvitation.Source.Name, (uint)partyInvitation.Party.Leader.Id, partyInvitation.Party.Members.Select(entry => entry.GetPartyInvitationMemberInformations()), partyInvitation.Party.Guests.Select(entry => entry.GetPartyGuestInformations(partyInvitation.Party))));
            }
        }
        [MessageHandler]
        public static void PartyFollowMemberRequest(PartyFollowMemberRequestMessage message, WorldClient client)
        {
            if(client.Character.Party != null)
            {
                client.Character.Party.StartFollowPlayer(client.Character, (int)message.playerId);
            }
        }
        [MessageHandler]
        public static void PartyFollowThisMemberRequest(PartyFollowThisMemberRequestMessage message, WorldClient client)
        {
            if (client.Character.Party != null && client.Character.IsPartyLeader())
            {
                if (message.enabled)
                {
                    client.Character.Party.StartFollowingThisPlayer(client.Character, (int)message.playerId);
                }
                else
                {
                    client.Character.Party.StopFollowingPlayerThis((int)message.playerId);
                }
            }
        }
        [MessageHandler]
        public static void PartyStopFollowMemberRequest(PartyStopFollowRequestMessage message, WorldClient client)
        {
            if (client.Character.Party != null)
            {
                client.Character.Party.StopFollowingPlayer(client.Character);
            }
        }

        public static void SendPartyJoinMessage(WorldClient client, Party party)
        {
            client.Send(new PartyJoinMessage(party.Id, (sbyte)party.Type, (uint)party.Leader.Id, party.MAX_MEMBER_COUNT, party.Members.Select(x => x.GetPartyMemberInformations()), party.Guests.Select(x => x.GetPartyGuestInformations(party)), true, ""));
        }
    }
}
