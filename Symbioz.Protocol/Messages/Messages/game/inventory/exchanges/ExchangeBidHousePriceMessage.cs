


















// Generated on 06/04/2015 18:44:51
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeBidHousePriceMessage : Message
{

public const ushort Id = 5805;
public override ushort MessageId
{
    get { return Id; }
}

public ushort genId;
        

public ExchangeBidHousePriceMessage()
{
}

public ExchangeBidHousePriceMessage(ushort genId)
        {
            this.genId = genId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(genId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

genId = reader.ReadVarUhShort();
            if (genId < 0)
                throw new Exception("Forbidden value on genId = " + genId + ", it doesn't respect the following condition : genId < 0");
            

}


}


}