


















// Generated on 06/04/2015 18:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class BasicAckMessage : Message
{

public const ushort Id = 6362;
public override ushort MessageId
{
    get { return Id; }
}

public uint seq;
        public ushort lastPacketId;
        

public BasicAckMessage()
{
}

public BasicAckMessage(uint seq, ushort lastPacketId)
        {
            this.seq = seq;
            this.lastPacketId = lastPacketId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(seq);
            writer.WriteVarUhShort(lastPacketId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

seq = reader.ReadVarUhInt();
            if (seq < 0)
                throw new Exception("Forbidden value on seq = " + seq + ", it doesn't respect the following condition : seq < 0");
            lastPacketId = reader.ReadVarUhShort();
            if (lastPacketId < 0)
                throw new Exception("Forbidden value on lastPacketId = " + lastPacketId + ", it doesn't respect the following condition : lastPacketId < 0");
            

}


}


}