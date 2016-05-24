


















// Generated on 06/04/2015 18:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class HouseBuyResultMessage : Message
{

public const ushort Id = 5735;
public override ushort MessageId
{
    get { return Id; }
}

public uint houseId;
        public bool bought;
        public uint realPrice;
        

public HouseBuyResultMessage()
{
}

public HouseBuyResultMessage(uint houseId, bool bought, uint realPrice)
        {
            this.houseId = houseId;
            this.bought = bought;
            this.realPrice = realPrice;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(houseId);
            writer.WriteBoolean(bought);
            writer.WriteVarUhInt(realPrice);
            

}

public override void Deserialize(ICustomDataInput reader)
{

houseId = reader.ReadVarUhInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            bought = reader.ReadBoolean();
            realPrice = reader.ReadVarUhInt();
            if (realPrice < 0)
                throw new Exception("Forbidden value on realPrice = " + realPrice + ", it doesn't respect the following condition : realPrice < 0");
            

}


}


}