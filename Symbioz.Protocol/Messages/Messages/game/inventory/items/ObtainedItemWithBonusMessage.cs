


















// Generated on 06/04/2015 18:45:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ObtainedItemWithBonusMessage : ObtainedItemMessage
{

public const ushort Id = 6520;
public override ushort MessageId
{
    get { return Id; }
}

public uint bonusQuantity;
        

public ObtainedItemWithBonusMessage()
{
}

public ObtainedItemWithBonusMessage(ushort genericId, uint baseQuantity, uint bonusQuantity)
         : base(genericId, baseQuantity)
        {
            this.bonusQuantity = bonusQuantity;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(bonusQuantity);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            bonusQuantity = reader.ReadVarUhInt();
            if (bonusQuantity < 0)
                throw new Exception("Forbidden value on bonusQuantity = " + bonusQuantity + ", it doesn't respect the following condition : bonusQuantity < 0");
            

}


}


}