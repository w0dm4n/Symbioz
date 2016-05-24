


















// Generated on 06/04/2015 18:44:50
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeBidHouseInListUpdatedMessage : ExchangeBidHouseInListAddedMessage
{

public const ushort Id = 6337;
public override ushort MessageId
{
    get { return Id; }
}



public ExchangeBidHouseInListUpdatedMessage()
{
}

public ExchangeBidHouseInListUpdatedMessage(int itemUID, int objGenericId, IEnumerable<Types.ObjectEffect> effects, IEnumerable<uint> prices)
         : base(itemUID, objGenericId, effects, prices)
        {
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            

}


}


}