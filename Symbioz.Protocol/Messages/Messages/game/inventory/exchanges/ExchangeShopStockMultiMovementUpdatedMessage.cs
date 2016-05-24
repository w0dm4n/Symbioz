


















// Generated on 06/04/2015 18:44:55
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeShopStockMultiMovementUpdatedMessage : Message
{

public const ushort Id = 6038;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ObjectItemToSell> objectInfoList;
        

public ExchangeShopStockMultiMovementUpdatedMessage()
{
}

public ExchangeShopStockMultiMovementUpdatedMessage(IEnumerable<Types.ObjectItemToSell> objectInfoList)
        {
            this.objectInfoList = objectInfoList;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)objectInfoList.Count());
            foreach (var entry in objectInfoList)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            objectInfoList = new Types.ObjectItemToSell[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectInfoList as Types.ObjectItemToSell[])[i] = new Types.ObjectItemToSell();
                 (objectInfoList as Types.ObjectItemToSell[])[i].Deserialize(reader);
            }
            

}


}


}