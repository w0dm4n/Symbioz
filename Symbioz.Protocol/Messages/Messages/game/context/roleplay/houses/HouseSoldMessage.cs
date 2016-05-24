


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class HouseSoldMessage : Message
{

public const ushort Id = 5737;
public override ushort MessageId
{
    get { return Id; }
}

public uint houseId;
        public uint realPrice;
        public string buyerName;
        

public HouseSoldMessage()
{
}

public HouseSoldMessage(uint houseId, uint realPrice, string buyerName)
        {
            this.houseId = houseId;
            this.realPrice = realPrice;
            this.buyerName = buyerName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(houseId);
            writer.WriteVarUhInt(realPrice);
            writer.WriteUTF(buyerName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

houseId = reader.ReadVarUhInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            realPrice = reader.ReadVarUhInt();
            if (realPrice < 0)
                throw new Exception("Forbidden value on realPrice = " + realPrice + ", it doesn't respect the following condition : realPrice < 0");
            buyerName = reader.ReadUTF();
            

}


}


}