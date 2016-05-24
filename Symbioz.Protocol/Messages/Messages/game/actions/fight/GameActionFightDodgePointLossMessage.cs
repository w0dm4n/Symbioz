


















// Generated on 06/04/2015 18:44:06
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightDodgePointLossMessage : AbstractGameActionMessage
{

public const ushort Id = 5828;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public ushort amount;
        

public GameActionFightDodgePointLossMessage()
{
}

public GameActionFightDodgePointLossMessage(ushort actionId, int sourceId, int targetId, ushort amount)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.amount = amount;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteVarUhShort(amount);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            amount = reader.ReadVarUhShort();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
            

}


}


}