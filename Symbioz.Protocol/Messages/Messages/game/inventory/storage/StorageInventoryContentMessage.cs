


















// Generated on 06/04/2015 18:45:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class StorageInventoryContentMessage : InventoryContentMessage
{

public const ushort Id = 5646;
public override ushort MessageId
{
    get { return Id; }
}



public StorageInventoryContentMessage()
{
}

public StorageInventoryContentMessage(IEnumerable<Types.ObjectItem> objects, uint kamas)
         : base(objects, kamas)
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