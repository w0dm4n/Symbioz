


















// Generated on 06/04/2015 18:44:56
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartedMountStockMessage : Message
{

public const ushort Id = 5984;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ObjectItem> objectsInfos;
        

public ExchangeStartedMountStockMessage()
{
}

public ExchangeStartedMountStockMessage(IEnumerable<Types.ObjectItem> objectsInfos)
        {
            this.objectsInfos = objectsInfos;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)objectsInfos.Count());
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            objectsInfos = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectsInfos as Types.ObjectItem[])[i] = new Types.ObjectItem();
                 (objectsInfos as Types.ObjectItem[])[i].Deserialize(reader);
            }
            

}


}


}