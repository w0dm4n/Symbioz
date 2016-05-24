


















// Generated on 06/04/2015 18:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SequenceNumberMessage : Message
{

public const ushort Id = 6317;
public override ushort MessageId
{
    get { return Id; }
}

public ushort number;
        

public SequenceNumberMessage()
{
}

public SequenceNumberMessage(ushort number)
        {
            this.number = number;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort(number);
            

}

public override void Deserialize(ICustomDataInput reader)
{

number = reader.ReadUShort();
            if ((number < 0) || (number > 65535))
                throw new Exception("Forbidden value on number = " + number + ", it doesn't respect the following condition : (number < 0) || (number > 65535)");
            

}


}


}