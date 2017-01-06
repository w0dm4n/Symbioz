using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Parties
{
    public class PartyInvitation
    {
        public Character Source;
        public Character Target;
        public Party Party;

        public PartyInvitation(Character source, Character target, Party party)
        {
            Source = source;
            Target = target;
            Party = party;
        }

        public void Accept()
        {
            if(Party.MembersCount > 0)
            {
                if (Target.Party != null)
                    Target.QuitParty(false);
                Target.RemoveInvitation(this);
                Party.AddMember(Target);
                Target.Party = Party;
            }
            else
            {
                Target.Client.Send(new PartyCannotJoinErrorMessage(0, (sbyte)PartyJoinErrorEnum.PARTY_JOIN_ERROR_PARTY_NOT_FOUND));
                Target.RemoveInvitation(this);
            }
        }

        public void Refuse()
        {
            Target.RemoveInvitation(this);
            Party.RemoveGuest(Target);
            Target.Client.Send(new PartyInvitationCancelledForGuestMessage(Party.Id, (uint)Target.Id));
            for (int i = 0; i < Party.Members.Count; i++)
            {
                Party.Members[i].Client.Send(new PartyRefuseInvitationNotificationMessage(Party.Id, (uint)Target.Id));
            }
        }

        public void Cancel()
        {
            Target.RemoveInvitation(this);
            Party.RemoveGuest(Target);
            Target.Client.Send(new PartyInvitationCancelledForGuestMessage(Party.Id, (uint)Target.Id));
        }

        public void Show()
        {
            Target.Client.Send(new PartyInvitationMessage(Party.Id, (sbyte)Party.Type, "", Party.MAX_MEMBER_COUNT, (uint)Source.Id, Source.Name, (uint)Target.Id));
            Target.AddInvitation(this);
        }
    }
}
