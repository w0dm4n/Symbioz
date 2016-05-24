


















// Generated on 06/04/2015 18:44:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PaddockBuyRequestMessage : Message
{

public const ushort Id = 5951;
public override ushort MessageId
{
    get { return Id; }
}

public uint proposedPrice;
        

public PaddockBuyRequestMessage()
{
}

public PaddockBuyRequestMessage(uint proposedPrice)
        {
            this.proposedPrice = proposedPrice;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(proposedPrice);
            

}

public override void Deserialize(ICustomDataInput reader)
{

proposedPrice = reader.ReadVarUhInt();
            if (proposedPrice < 0)
                throw new Exception("Forbidden value on proposedPrice = " + proposedPrice + ", it doesn't respect the following condition : proposedPrice < 0");
            

}


}


}