


















// Generated on 06/04/2015 18:45:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class WrapperObjectAssociatedMessage : SymbioticObjectAssociatedMessage
{

public const ushort Id = 6523;
public override ushort MessageId
{
    get { return Id; }
}



public WrapperObjectAssociatedMessage()
{
}

public WrapperObjectAssociatedMessage(uint hostUID)
         : base(hostUID)
        {
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            

}


}


}