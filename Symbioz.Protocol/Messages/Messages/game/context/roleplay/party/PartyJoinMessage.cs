


















// Generated on 06/04/2015 18:44:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyJoinMessage : AbstractPartyMessage
{

public const ushort Id = 5576;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte partyType;
        public uint partyLeaderId;
        public sbyte maxParticipants;
        public IEnumerable<Types.PartyMemberInformations> members;
        public IEnumerable<Types.PartyGuestInformations> guests;
        public bool restricted;
        public string partyName;
        

public PartyJoinMessage()
{
}

public PartyJoinMessage(uint partyId, sbyte partyType, uint partyLeaderId, sbyte maxParticipants, IEnumerable<Types.PartyMemberInformations> members, IEnumerable<Types.PartyGuestInformations> guests, bool restricted, string partyName)
         : base(partyId)
        {
            this.partyType = partyType;
            this.partyLeaderId = partyLeaderId;
            this.maxParticipants = maxParticipants;
            this.members = members;
            this.guests = guests;
            this.restricted = restricted;
            this.partyName = partyName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(partyType);
            writer.WriteVarUhInt(partyLeaderId);
            writer.WriteSByte(maxParticipants);
            writer.WriteUShort((ushort)members.Count());
            foreach (var entry in members)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)guests.Count());
            foreach (var entry in guests)
            {
                 entry.Serialize(writer);
            }
            writer.WriteBoolean(restricted);
            writer.WriteUTF(partyName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            partyType = reader.ReadSByte();
            if (partyType < 0)
                throw new Exception("Forbidden value on partyType = " + partyType + ", it doesn't respect the following condition : partyType < 0");
            partyLeaderId = reader.ReadVarUhInt();
            if (partyLeaderId < 0)
                throw new Exception("Forbidden value on partyLeaderId = " + partyLeaderId + ", it doesn't respect the following condition : partyLeaderId < 0");
            maxParticipants = reader.ReadSByte();
            if (maxParticipants < 0)
                throw new Exception("Forbidden value on maxParticipants = " + maxParticipants + ", it doesn't respect the following condition : maxParticipants < 0");
            var limit = reader.ReadUShort();
            members = new Types.PartyMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (members as Types.PartyMemberInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.PartyMemberInformations>(reader.ReadShort());
                 (members as Types.PartyMemberInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            guests = new Types.PartyGuestInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guests as Types.PartyGuestInformations[])[i] = new Types.PartyGuestInformations();
                 (guests as Types.PartyGuestInformations[])[i].Deserialize(reader);
            }
            restricted = reader.ReadBoolean();
            partyName = reader.ReadUTF();
            

}


}


}