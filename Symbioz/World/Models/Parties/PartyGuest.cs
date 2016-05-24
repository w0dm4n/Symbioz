using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Parties
{
    public class PartyGuest
    {
        public Party Party;

        public Character Character;

        public Character InvitedBy;

        public PartyGuest(Party p, Character i, Character invitedBy)
        {
            this.Party = p;
            this.Character = i;
            this.InvitedBy = invitedBy;
        }

        public PartyGuestInformations GetPartyGuestInformations()
        {
            var c = this.Character;
            PartyCompanionMemberInformations[] memberInformationsArray = new PartyCompanionMemberInformations[0];
            return new PartyGuestInformations(c.Id, this.Party.Id, c.Record.Name, c.Look.ToEntityLook(), c.Record.Breed, c.Record.Sex, new PlayerStatus((sbyte)0), (IEnumerable<PartyCompanionMemberInformations>)memberInformationsArray);
        }
    }
}