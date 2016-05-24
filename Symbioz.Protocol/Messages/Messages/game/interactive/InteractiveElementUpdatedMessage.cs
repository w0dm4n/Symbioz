


















// Generated on 06/04/2015 18:44:48
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class InteractiveElementUpdatedMessage : Message
{

public const ushort Id = 5708;
public override ushort MessageId
{
    get { return Id; }
}

public Types.InteractiveElement interactiveElement;
        

public InteractiveElementUpdatedMessage()
{
}

public InteractiveElementUpdatedMessage(Types.InteractiveElement interactiveElement)
        {
            this.interactiveElement = interactiveElement;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

interactiveElement.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

interactiveElement = new Types.InteractiveElement();
            interactiveElement.Deserialize(reader);
            

}


}


}