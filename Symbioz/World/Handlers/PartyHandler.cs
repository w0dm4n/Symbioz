using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.World.Models;
using Symbioz.World.Models.Parties;
using Symbioz.World.Models.Parties.Dungeon;
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
            Party p;
            if (client.Character.PartyMember == null)
            {
                WorldServer.Instance.Parties.OrderBy(x => x.Id);
                int partyId = 0;
                if (WorldServer.Instance.Parties.Count > 0)
                {
                    partyId = WorldServer.Instance.Parties.Last().Id + 1;
                }
                else
                {
                    partyId = 1;
                }
                p = new Party(partyId, client.Character.Id, "");
            }
            else
            {
                p = WorldServer.Instance.Parties.Find(x => x.Id == client.Character.PartyMember.Party.Id);
            }
            if (p == null)
                return;
            WorldClient to = WorldServer.Instance.WorldClients.Find(x => x.Character.Record.Name == message.name);
            p.CreateInvitation(client, to);
            if (p.Members.Count == 0)
            {
                if (to.Character.PartyMember != null && to.Character.PartyMember.Loyal)
                    return;
                p.BossCharacterId = client.Character.Id;
                p.NewMember(client);
            }
            
        }
        [MessageHandler]
        public static void PartyAcceptInvitation(PartyAcceptInvitationMessage message, WorldClient client)
        {
            Party p = WorldServer.Instance.GetPartyById((int)message.partyId);
            if (p != null)
            {
                p.AcceptInvitation(client);
            }
        }
        [MessageHandler]
        public static void PartyRefusedInvitation(PartyRefuseInvitationMessage message, WorldClient client)
        {
            Party p = WorldServer.Instance.GetPartyById((int)message.partyId);
            if (p != null)
            {
                p.RefuseInvitation(client);
            }
        }
        [MessageHandler]
        public static void PartyCancelInvitation(PartyCancelInvitationMessage message, WorldClient client)
        {
            Party p = WorldServer.Instance.GetPartyById((int)message.partyId);
            if (p != null)
            {
                p.CancelInvitation(client, WorldServer.Instance.WorldClients.Find(x => x.Character.Id == message.guestId));
            }
        }
        [MessageHandler]
        public static void PartyLeaveRequest(PartyLeaveRequestMessage message, WorldClient client)
        {
            Party p = WorldServer.Instance.Parties.Find(x => x.Id == message.partyId);
            if (p != null)
            {
                p.QuitParty(client);
            }
        }
        [MessageHandler]
        public static void PartyAbdicateRequest(PartyAbdicateThroneMessage message, WorldClient client)
        {
            Party p = WorldServer.Instance.Parties.Find(x => x.Id == message.partyId);
            if (p == null)
                return;
            if (p.BossCharacterId == client.Character.Id)
            {
                p.ChangeLeader((int)message.playerId);
            }
            else
            {
                client.Character.Reply("Vous devez être chef de groupe pour nommer votre successeur");
            }
        }
        [MessageHandler]
        public static void PartyKickRequest(PartyKickRequestMessage message, WorldClient client)
        {
            Party p = WorldServer.Instance.Parties.Find(x => x.Id == message.partyId);
            if (p != null)
            {
                p.PlayerKick((int)message.playerId, client);
            }
        }
        [MessageHandler]
        public static void PartyGetInvitationDetailsRequest(PartyInvitationDetailsRequestMessage message, WorldClient client)
        {
            //client.Character.Reply("Cette fonctionnalité n'est pas encore implémentée");
            //return;

            Party p = WorldServer.Instance.Parties.Find(x => x.Id == message.partyId);
            if (p != null)
            {
                p.SendInvitationDetail(client);
            }
        }
        [MessageHandler]
        public static void PartyFollowMemberRequest(PartyFollowMemberRequestMessage message, WorldClient client)
        {
            client.Character.Reply("Cette fonctionnalité n'est pas encore implémentée");
        }
        [MessageHandler]
        public static void PartyFollowThisMemberRequest(PartyFollowThisMemberRequestMessage message, WorldClient client)
        {
            client.Character.Reply("Cette fonctionnalité n'est pas encore implémentée");
        }
        [MessageHandler]
        public static void PartyStopFollowMemberRequest(PartyStopFollowRequestMessage message, WorldClient client)
        {
            client.Character.Reply("Cette fonctionnalité n'est pas encore implémentée");
        }
        [MessageHandler]
        public static void PartyNameUpdateRequest(PartyNameSetRequestMessage message, WorldClient client)
        {
            Party p = WorldServer.Instance.Parties.Find(x => x.Id == message.partyId);
            if (p != null)
            {
                p.SetName(message.partyName, client);
            }
        }
        [MessageHandler]
        public static void PartyPledgeLoyaltyRequest(PartyPledgeLoyaltyRequestMessage message, WorldClient client)
        {
            if (client.Character.PartyMember != null)
            {
                client.Character.PartyMember.SetLoyalty(message.loyal);
            }
        }
        [MessageHandler]
        public static void HandleDungeonPartyFinder(DungeonPartyFinderAvailableDungeonsRequestMessage message, WorldClient client)
        {
            List<DungeonsIdRecord> record = DungeonsIdRecord.DungeonsId;
            List<ushort> ids = new List<ushort>();
            foreach(DungeonsIdRecord dj in record)
            {
                ids.Add((ushort)dj.Id);
            }
            client.Send(new DungeonPartyFinderAvailableDungeonsMessage((IEnumerable<ushort>)ids));
        }
        [MessageHandler]
        public static void HandleDungeonPartyFinderRegister(DungeonPartyFinderRegisterRequestMessage message, WorldClient client)
        {
            if (DungeonPartyProvider.Instance.GetDPCByCharacterId(client.Character.Id) != null)
            {
                DungeonPartyProvider.Instance.UpdateCharacter(client.Character, message.dungeonIds.ToList());
            }
            else
            {
                DungeonPartyProvider.Instance.AddCharacter(client.Character, message.dungeonIds.ToList());
            }
            client.Send(new DungeonPartyFinderRegisterSuccessMessage((IEnumerable<ushort>)message.dungeonIds));
        }
        [MessageHandler]
        public static void HandleDungeonPartyFinderListenRequest(DungeonPartyFinderListenRequestMessage message, WorldClient client)
        {
            var players = DungeonPartyProvider.Instance.GetCharactersForDungeon(message.dungeonId);
            client.Send(new DungeonPartyFinderRoomContentMessage(message.dungeonId, (IEnumerable<DungeonPartyFinderPlayer>)players));
        }
        [MessageHandler]
        public static void HandlePartyInvitationDungeonRequest(PartyInvitationDungeonRequestMessage message, WorldClient client)
        {
            WorldClient target = WorldServer.Instance.GetOnlineClient(message.name);
            Party p;
            if (client.Character.PartyMember == null)
            {
                WorldServer.Instance.Parties.OrderBy(x => x.Id);
                int partyId = 0;
                if (WorldServer.Instance.Parties.Count > 0)
                {
                    partyId = WorldServer.Instance.Parties.Last().Id + 1;
                }
                else
                {
                    partyId = 1;
                }
                p = new Party(partyId, client.Character.Id, "");
            }
            else
            {
                p = WorldServer.Instance.Parties.Find(x => x.Id == client.Character.PartyMember.Party.Id);
            }
            if (p == null)
                return;
            p.SetName(DungeonsIdRecord.DungeonsId.Find(x => x.Id == message.dungeonId).Name, client);
            p.CreateInvitation(client, target, PartyTypeEnum.PARTY_TYPE_DUNGEON, message.dungeonId);
            if (p.Members.Count == 0)
            {
                p.BossCharacterId = client.Character.Id;
                p.NewMember(client);
            }
        }
    }
}
