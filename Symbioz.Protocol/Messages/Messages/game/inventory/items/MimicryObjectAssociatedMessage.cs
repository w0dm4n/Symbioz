


















// Generated on 06/04/2015 18:45:05
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MimicryObjectAssociatedMessage : SymbioticObjectAssociatedMessage
{

public const ushort Id = 6462;
public override ushort MessageId
{
    get { return Id; }
}



public MimicryObjectAssociatedMessage()
{
}

public MimicryObjectAssociatedMessage(uint hostUID)
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