


















// Generated on 06/04/2015 18:44:49
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class StatedElementUpdatedMessage : Message
{

public const ushort Id = 5709;
public override ushort MessageId
{
    get { return Id; }
}

public Types.StatedElement statedElement;
        

public StatedElementUpdatedMessage()
{
}

public StatedElementUpdatedMessage(Types.StatedElement statedElement)
        {
            this.statedElement = statedElement;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

statedElement.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

statedElement = new Types.StatedElement();
            statedElement.Deserialize(reader);
            

}


}


}