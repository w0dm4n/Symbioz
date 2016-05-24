


















// Generated on 06/04/2015 18:44:38
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PurchasableDialogMessage : Message
{

public const ushort Id = 5739;
public override ushort MessageId
{
    get { return Id; }
}

public bool buyOrSell;
        public uint purchasableId;
        public uint price;
        

public PurchasableDialogMessage()
{
}

public PurchasableDialogMessage(bool buyOrSell, uint purchasableId, uint price)
        {
            this.buyOrSell = buyOrSell;
            this.purchasableId = purchasableId;
            this.price = price;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(buyOrSell);
            writer.WriteVarUhInt(purchasableId);
            writer.WriteVarUhInt(price);
            

}

public override void Deserialize(ICustomDataInput reader)
{

buyOrSell = reader.ReadBoolean();
            purchasableId = reader.ReadVarUhInt();
            if (purchasableId < 0)
                throw new Exception("Forbidden value on purchasableId = " + purchasableId + ", it doesn't respect the following condition : purchasableId < 0");
            price = reader.ReadVarUhInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
            

}


}


}