


















// Generated on 06/04/2015 18:44:57
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartOkNpcShopMessage : Message
{

public const ushort Id = 5761;
public override ushort MessageId
{
    get { return Id; }
}

public int npcSellerId;
        public ushort tokenId;
        public IEnumerable<Types.ObjectItemToSellInNpcShop> objectsInfos;
        

public ExchangeStartOkNpcShopMessage()
{
}

public ExchangeStartOkNpcShopMessage(int npcSellerId, ushort tokenId, IEnumerable<Types.ObjectItemToSellInNpcShop> objectsInfos)
        {
            this.npcSellerId = npcSellerId;
            this.tokenId = tokenId;
            this.objectsInfos = objectsInfos;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(npcSellerId);
            writer.WriteVarUhShort(tokenId);
            writer.WriteUShort((ushort)objectsInfos.Count());
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

npcSellerId = reader.ReadInt();
            tokenId = reader.ReadVarUhShort();
            if (tokenId < 0)
                throw new Exception("Forbidden value on tokenId = " + tokenId + ", it doesn't respect the following condition : tokenId < 0");
            var limit = reader.ReadUShort();
            objectsInfos = new Types.ObjectItemToSellInNpcShop[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectsInfos as Types.ObjectItemToSellInNpcShop[])[i] = new Types.ObjectItemToSellInNpcShop();
                 (objectsInfos as Types.ObjectItemToSellInNpcShop[])[i].Deserialize(reader);
            }
            

}


}


}