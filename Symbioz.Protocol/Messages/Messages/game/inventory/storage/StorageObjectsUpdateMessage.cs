


















// Generated on 06/04/2015 18:45:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class StorageObjectsUpdateMessage : Message
{

public const ushort Id = 6036;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ObjectItem> objectList;
        

public StorageObjectsUpdateMessage()
{
}

public StorageObjectsUpdateMessage(IEnumerable<Types.ObjectItem> objectList)
        {
            this.objectList = objectList;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)objectList.Count());
            foreach (var entry in objectList)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            objectList = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectList as Types.ObjectItem[])[i] = new Types.ObjectItem();
                 (objectList as Types.ObjectItem[])[i].Deserialize(reader);
            }
            

}


}


}