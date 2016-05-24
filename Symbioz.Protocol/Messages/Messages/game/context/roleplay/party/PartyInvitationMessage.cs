


















// Generated on 06/04/2015 18:44:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyInvitationMessage : AbstractPartyMessage
{

public const ushort Id = 5586;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte partyType;
        public string partyName;
        public sbyte maxParticipants;
        public uint fromId;
        public string fromName;
        public uint toId;
        

public PartyInvitationMessage()
{
}

public PartyInvitationMessage(uint partyId, sbyte partyType, string partyName, sbyte maxParticipants, uint fromId, string fromName, uint toId)
         : base(partyId)
        {
            this.partyType = partyType;
            this.partyName = partyName;
            this.maxParticipants = maxParticipants;
            this.fromId = fromId;
            this.fromName = fromName;
            this.toId = toId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(partyType);
            writer.WriteUTF(partyName);
            writer.WriteSByte(maxParticipants);
            writer.WriteVarUhInt(fromId);
            writer.WriteUTF(fromName);
            writer.WriteVarUhInt(toId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            partyType = reader.ReadSByte();
            if (partyType < 0)
                throw new Exception("Forbidden value on partyType = " + partyType + ", it doesn't respect the following condition : partyType < 0");
            partyName = reader.ReadUTF();
            maxParticipants = reader.ReadSByte();
            if (maxParticipants < 0)
                throw new Exception("Forbidden value on maxParticipants = " + maxParticipants + ", it doesn't respect the following condition : maxParticipants < 0");
            fromId = reader.ReadVarUhInt();
            if (fromId < 0)
                throw new Exception("Forbidden value on fromId = " + fromId + ", it doesn't respect the following condition : fromId < 0");
            fromName = reader.ReadUTF();
            toId = reader.ReadVarUhInt();
            if (toId < 0)
                throw new Exception("Forbidden value on toId = " + toId + ", it doesn't respect the following condition : toId < 0");
            

}


}


}