


















// Generated on 06/04/2015 18:44:09
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SequenceStartMessage : Message
{

public const ushort Id = 955;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte sequenceType;
        public int authorId;
        

public SequenceStartMessage()
{
}

public SequenceStartMessage(sbyte sequenceType, int authorId)
        {
            this.sequenceType = sequenceType;
            this.authorId = authorId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(sequenceType);
            writer.WriteInt(authorId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

sequenceType = reader.ReadSByte();
            authorId = reader.ReadInt();
            

}


}


}