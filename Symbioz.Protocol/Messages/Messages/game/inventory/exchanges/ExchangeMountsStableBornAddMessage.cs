


















// Generated on 06/04/2015 18:44:53
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeMountsStableBornAddMessage : ExchangeMountsStableAddMessage
{

public const ushort Id = 6557;
public override ushort MessageId
{
    get { return Id; }
}



public ExchangeMountsStableBornAddMessage()
{
}

public ExchangeMountsStableBornAddMessage(IEnumerable<Types.MountClientData> mountDescription)
         : base(mountDescription)
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