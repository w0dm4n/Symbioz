


















// Generated on 06/04/2015 18:44:09
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SequenceEndMessage : Message
{

public const ushort Id = 956;
public override ushort MessageId
{
    get { return Id; }
}

public ushort actionId;
        public int authorId;
        public sbyte sequenceType;
        

public SequenceEndMessage()
{
}

public SequenceEndMessage(ushort actionId, int authorId, sbyte sequenceType)
        {
            this.actionId = actionId;
            this.authorId = authorId;
            this.sequenceType = sequenceType;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(actionId);
            writer.WriteInt(authorId);
            writer.WriteSByte(sequenceType);
            

}

public override void Deserialize(ICustomDataInput reader)
{

actionId = reader.ReadVarUhShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            authorId = reader.ReadInt();
            sequenceType = reader.ReadSByte();
            

}


}


}