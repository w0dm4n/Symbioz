


















// Generated on 06/04/2015 18:45:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class InventoryContentMessage : Message
{

public const ushort Id = 3016;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ObjectItem> objects;
        public uint kamas;
        

public InventoryContentMessage()
{
}

public InventoryContentMessage(IEnumerable<Types.ObjectItem> objects, uint kamas)
        {
            this.objects = objects;
            this.kamas = kamas;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)objects.Count());
            foreach (var entry in objects)
            {
                 entry.Serialize(writer);
            }
            writer.WriteVarUhInt(kamas);
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            objects = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objects as Types.ObjectItem[])[i] = new Types.ObjectItem();
                 (objects as Types.ObjectItem[])[i].Deserialize(reader);
            }
            kamas = reader.ReadVarUhInt();
            if (kamas < 0)
                throw new Exception("Forbidden value on kamas = " + kamas + ", it doesn't respect the following condition : kamas < 0");
            

}


}


}