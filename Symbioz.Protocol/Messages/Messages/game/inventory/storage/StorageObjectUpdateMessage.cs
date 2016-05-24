


















// Generated on 06/04/2015 18:45:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class StorageObjectUpdateMessage : Message
{

public const ushort Id = 5647;
public override ushort MessageId
{
    get { return Id; }
}

public Types.ObjectItem @object;
        

public StorageObjectUpdateMessage()
{
}

public StorageObjectUpdateMessage(Types.ObjectItem @object)
        {
            this.@object = @object;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

@object.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

@object = new Types.ObjectItem();
            @object.Deserialize(reader);
            

}


}


}