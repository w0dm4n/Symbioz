


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class HouseSellFromInsideRequestMessage : HouseSellRequestMessage
{

public const ushort Id = 5884;
public override ushort MessageId
{
    get { return Id; }
}



public HouseSellFromInsideRequestMessage()
{
}

public HouseSellFromInsideRequestMessage(uint amount)
         : base(amount)
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