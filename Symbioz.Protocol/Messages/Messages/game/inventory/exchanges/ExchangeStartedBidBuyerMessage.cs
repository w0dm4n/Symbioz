


















// Generated on 06/04/2015 18:44:56
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartedBidBuyerMessage : Message
{

public const ushort Id = 5904;
public override ushort MessageId
{
    get { return Id; }
}

public Types.SellerBuyerDescriptor buyerDescriptor;
        

public ExchangeStartedBidBuyerMessage()
{
}

public ExchangeStartedBidBuyerMessage(Types.SellerBuyerDescriptor buyerDescriptor)
        {
            this.buyerDescriptor = buyerDescriptor;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

buyerDescriptor.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

buyerDescriptor = new Types.SellerBuyerDescriptor();
            buyerDescriptor.Deserialize(reader);
            

}


}


}