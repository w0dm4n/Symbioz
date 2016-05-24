


















// Generated on 06/04/2015 18:44:56
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartOkHumanVendorMessage : Message
{

public const ushort Id = 5767;
public override ushort MessageId
{
    get { return Id; }
}

public uint sellerId;
        public IEnumerable<Types.ObjectItemToSellInHumanVendorShop> objectsInfos;
        

public ExchangeStartOkHumanVendorMessage()
{
}

public ExchangeStartOkHumanVendorMessage(uint sellerId, IEnumerable<Types.ObjectItemToSellInHumanVendorShop> objectsInfos)
        {
            this.sellerId = sellerId;
            this.objectsInfos = objectsInfos;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(sellerId);
            writer.WriteUShort((ushort)objectsInfos.Count());
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

sellerId = reader.ReadVarUhInt();
            if (sellerId < 0)
                throw new Exception("Forbidden value on sellerId = " + sellerId + ", it doesn't respect the following condition : sellerId < 0");
            var limit = reader.ReadUShort();
            objectsInfos = new Types.ObjectItemToSellInHumanVendorShop[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectsInfos as Types.ObjectItemToSellInHumanVendorShop[])[i] = new Types.ObjectItemToSellInHumanVendorShop();
                 (objectsInfos as Types.ObjectItemToSellInHumanVendorShop[])[i].Deserialize(reader);
            }
            

}


}


}