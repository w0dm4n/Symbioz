


















// Generated on 06/04/2015 18:44:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PaddockBuyResultMessage : Message
{

public const ushort Id = 6516;
public override ushort MessageId
{
    get { return Id; }
}

public int paddockId;
        public bool bought;
        public uint realPrice;
        

public PaddockBuyResultMessage()
{
}

public PaddockBuyResultMessage(int paddockId, bool bought, uint realPrice)
        {
            this.paddockId = paddockId;
            this.bought = bought;
            this.realPrice = realPrice;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(paddockId);
            writer.WriteBoolean(bought);
            writer.WriteVarUhInt(realPrice);
            

}

public override void Deserialize(ICustomDataInput reader)
{

paddockId = reader.ReadInt();
            bought = reader.ReadBoolean();
            realPrice = reader.ReadVarUhInt();
            if (realPrice < 0)
                throw new Exception("Forbidden value on realPrice = " + realPrice + ", it doesn't respect the following condition : realPrice < 0");
            

}


}


}