


















// Generated on 06/04/2015 18:45:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AccessoryPreviewMessage : Message
{

public const ushort Id = 6517;
public override ushort MessageId
{
    get { return Id; }
}

public Types.EntityLook look;
        

public AccessoryPreviewMessage()
{
}

public AccessoryPreviewMessage(Types.EntityLook look)
        {
            this.look = look;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

look.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

look = new Types.EntityLook();
            look.Deserialize(reader);
            

}


}


}