


















// Generated on 06/04/2015 18:44:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyInvitationDetailsMessage : AbstractPartyMessage
{

public const ushort Id = 6263;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte partyType;
        public string partyName;
        public uint fromId;
        public string fromName;
        public uint leaderId;
        public IEnumerable<Types.PartyInvitationMemberInformations> members;
        public IEnumerable<Types.PartyGuestInformations> guests;
        

public PartyInvitationDetailsMessage()
{
}

public PartyInvitationDetailsMessage(uint partyId, sbyte partyType, string partyName, uint fromId, string fromName, uint leaderId, IEnumerable<Types.PartyInvitationMemberInformations> members, IEnumerable<Types.PartyGuestInformations> guests)
         : base(partyId)
        {
            this.partyType = partyType;
            this.partyName = partyName;
            this.fromId = fromId;
            this.fromName = fromName;
            this.leaderId = leaderId;
            this.members = members;
            this.guests = guests;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(partyType);
            writer.WriteUTF(partyName);
            writer.WriteVarUhInt(fromId);
            writer.WriteUTF(fromName);
            writer.WriteVarUhInt(leaderId);
            writer.WriteUShort((ushort)members.Count());
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)guests.Count());
            foreach (var entry in guests)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            partyType = reader.ReadSByte();
            if (partyType < 0)
                throw new Exception("Forbidden value on partyType = " + partyType + ", it doesn't respect the following condition : partyType < 0");
            partyName = reader.ReadUTF();
            fromId = reader.ReadVarUhInt();
            if (fromId < 0)
                throw new Exception("Forbidden value on fromId = " + fromId + ", it doesn't respect the following condition : fromId < 0");
            fromName = reader.ReadUTF();
            leaderId = reader.ReadVarUhInt();
            if (leaderId < 0)
                throw new Exception("Forbidden value on leaderId = " + leaderId + ", it doesn't respect the following condition : leaderId < 0");
            var limit = reader.ReadUShort();
            members = new Types.PartyInvitationMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (members as Types.PartyInvitationMemberInformations[])[i] = new Types.PartyInvitationMemberInformations();
                 (members as Types.PartyInvitationMemberInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            guests = new Types.PartyGuestInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guests as Types.PartyGuestInformations[])[i] = new Types.PartyGuestInformations();
                 (guests as Types.PartyGuestInformations[])[i].Deserialize(reader);
            }
            

}


}


}