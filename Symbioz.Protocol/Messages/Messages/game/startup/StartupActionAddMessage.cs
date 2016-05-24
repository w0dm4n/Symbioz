


















// Generated on 06/04/2015 18:45:14
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class StartupActionAddMessage : Message
{

public const ushort Id = 6538;
public override ushort MessageId
{
    get { return Id; }
}

public Types.StartupActionAddObject newAction;
        

public StartupActionAddMessage()
{
}

public StartupActionAddMessage(Types.StartupActionAddObject newAction)
        {
            this.newAction = newAction;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

newAction.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

newAction = new Types.StartupActionAddObject();
            newAction.Deserialize(reader);
            

}


}


}