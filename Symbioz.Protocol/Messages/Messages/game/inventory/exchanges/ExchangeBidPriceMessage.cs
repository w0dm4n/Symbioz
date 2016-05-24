


















// Generated on 06/04/2015 18:44:51
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeBidPriceMessage : Message
{

public const ushort Id = 5755;
public override ushort MessageId
{
    get { return Id; }
}

public ushort genericId;
        public int averagePrice;
        

public ExchangeBidPriceMessage()
{
}

public ExchangeBidPriceMessage(ushort genericId, int averagePrice)
        {
            this.genericId = genericId;
            this.averagePrice = averagePrice;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(genericId);
            writer.WriteVarInt(averagePrice);
            

}

public override void Deserialize(ICustomDataInput reader)
{

genericId = reader.ReadVarUhShort();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            averagePrice = reader.ReadVarInt();
            

}


}


}