


















// Generated on 06/04/2015 18:44:55
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeShopStockMovementUpdatedMessage : Message
{

public const ushort Id = 5909;
public override ushort MessageId
{
    get { return Id; }
}

public Types.ObjectItemToSell objectInfo;
        

public ExchangeShopStockMovementUpdatedMessage()
{
}

public ExchangeShopStockMovementUpdatedMessage(Types.ObjectItemToSell objectInfo)
        {
            this.objectInfo = objectInfo;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

objectInfo.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

objectInfo = new Types.ObjectItemToSell();
            objectInfo.Deserialize(reader);
            

}


}


}