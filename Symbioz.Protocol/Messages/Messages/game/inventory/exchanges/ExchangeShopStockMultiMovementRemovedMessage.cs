


















// Generated on 06/04/2015 18:44:55
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeShopStockMultiMovementRemovedMessage : Message
{

public const ushort Id = 6037;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<uint> objectIdList;
        

public ExchangeShopStockMultiMovementRemovedMessage()
{
}

public ExchangeShopStockMultiMovementRemovedMessage(IEnumerable<uint> objectIdList)
        {
            this.objectIdList = objectIdList;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)objectIdList.Count());
            foreach (var entry in objectIdList)
            {
                 writer.WriteVarUhInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            objectIdList = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectIdList as uint[])[i] = reader.ReadVarUhInt();
            }
            

}


}


}