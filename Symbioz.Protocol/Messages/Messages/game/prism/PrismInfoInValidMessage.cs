


















// Generated on 06/04/2015 18:45:11
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismInfoInValidMessage : Message
{

public const ushort Id = 5859;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte reason;
        

public PrismInfoInValidMessage()
{
}

public PrismInfoInValidMessage(sbyte reason)
        {
            this.reason = reason;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(reason);
            

}

public override void Deserialize(ICustomDataInput reader)
{

reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
            

}


}


}