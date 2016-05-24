


















// Generated on 06/04/2015 18:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class LockableStateUpdateAbstractMessage : Message
{

public const ushort Id = 5671;
public override ushort MessageId
{
    get { return Id; }
}

public bool locked;
        

public LockableStateUpdateAbstractMessage()
{
}

public LockableStateUpdateAbstractMessage(bool locked)
        {
            this.locked = locked;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(locked);
            

}

public override void Deserialize(ICustomDataInput reader)
{

locked = reader.ReadBoolean();
            

}


}


}