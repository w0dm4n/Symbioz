


















// Generated on 06/04/2015 18:44:53
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeObjectMoveKamaMessage : Message
{

public const ushort Id = 5520;
public override ushort MessageId
{
    get { return Id; }
}

public int quantity;
        

public ExchangeObjectMoveKamaMessage()
{
}

public ExchangeObjectMoveKamaMessage(int quantity)
        {
            this.quantity = quantity;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarInt(quantity);
            

}

public override void Deserialize(ICustomDataInput reader)
{

quantity = reader.ReadVarInt();
            

}


}


}