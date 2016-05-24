


















// Generated on 06/04/2015 18:44:55
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeShopStockMovementRemovedMessage : Message
{

public const ushort Id = 5907;
public override ushort MessageId
{
    get { return Id; }
}

public uint objectId;
        

public ExchangeShopStockMovementRemovedMessage()
{
}

public ExchangeShopStockMovementRemovedMessage(uint objectId)
        {
            this.objectId = objectId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(objectId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

objectId = reader.ReadVarUhInt();
            if (objectId < 0)
                throw new Exception("Forbidden value on objectId = " + objectId + ", it doesn't respect the following condition : objectId < 0");
            

}


}


}