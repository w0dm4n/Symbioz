


















// Generated on 06/04/2015 18:44:55
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeShopStockStartedMessage : Message
{

public const ushort Id = 5910;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ObjectItemToSell> objectsInfos;
        

public ExchangeShopStockStartedMessage()
{
}

public ExchangeShopStockStartedMessage(IEnumerable<Types.ObjectItemToSell> objectsInfos)
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
            objectsInfos = new Types.ObjectItemToSell[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectsInfos as Types.ObjectItemToSell[])[i] = new Types.ObjectItemToSell();
                 (objectsInfos as Types.ObjectItemToSell[])[i].Deserialize(reader);
            }
            

}


}


}