


















// Generated on 06/04/2015 18:45:06
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ObjectsQuantityMessage : Message
{

public const ushort Id = 6206;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ObjectItemQuantity> objectsUIDAndQty;
        

public ObjectsQuantityMessage()
{
}

public ObjectsQuantityMessage(IEnumerable<Types.ObjectItemQuantity> objectsUIDAndQty)
        {
            this.objectsUIDAndQty = objectsUIDAndQty;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)objectsUIDAndQty.Count());
            foreach (var entry in objectsUIDAndQty)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            objectsUIDAndQty = new Types.ObjectItemQuantity[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectsUIDAndQty as Types.ObjectItemQuantity[])[i] = new Types.ObjectItemQuantity();
                 (objectsUIDAndQty as Types.ObjectItemQuantity[])[i].Deserialize(reader);
            }
            

}


}


}