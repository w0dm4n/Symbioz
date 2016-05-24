


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class HouseSellRequestMessage : Message
{

public const ushort Id = 5697;
public override ushort MessageId
{
    get { return Id; }
}

public uint amount;
        

public HouseSellRequestMessage()
{
}

public HouseSellRequestMessage(uint amount)
        {
            this.amount = amount;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(amount);
            

}

public override void Deserialize(ICustomDataInput reader)
{

amount = reader.ReadVarUhInt();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
            

}


}


}