


















// Generated on 06/04/2015 18:45:14
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SubscriptionZoneMessage : Message
{

public const ushort Id = 5573;
public override ushort MessageId
{
    get { return Id; }
}

public bool active;
        

public SubscriptionZoneMessage()
{
}

public SubscriptionZoneMessage(bool active)
        {
            this.active = active;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(active);
            

}

public override void Deserialize(ICustomDataInput reader)
{

active = reader.ReadBoolean();
            

}


}


}