


















// Generated on 06/04/2015 18:44:58
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeKamaModifiedMessage : ExchangeObjectMessage
{

public const ushort Id = 5521;
public override ushort MessageId
{
    get { return Id; }
}

public uint quantity;
        

public ExchangeKamaModifiedMessage()
{
}

public ExchangeKamaModifiedMessage(bool remote, uint quantity)
         : base(remote)
        {
            this.quantity = quantity;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(quantity);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            quantity = reader.ReadVarUhInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
            

}


}


}