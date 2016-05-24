


















// Generated on 06/04/2015 18:44:50
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeBidHouseInListRemovedMessage : Message
{

public const ushort Id = 5950;
public override ushort MessageId
{
    get { return Id; }
}

public int itemUID;
        

public ExchangeBidHouseInListRemovedMessage()
{
}

public ExchangeBidHouseInListRemovedMessage(int itemUID)
        {
            this.itemUID = itemUID;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(itemUID);
            

}

public override void Deserialize(ICustomDataInput reader)
{

itemUID = reader.ReadInt();
            

}


}


}