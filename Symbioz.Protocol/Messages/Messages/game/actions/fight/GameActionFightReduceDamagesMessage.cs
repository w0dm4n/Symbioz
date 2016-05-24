


















// Generated on 06/04/2015 18:44:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightReduceDamagesMessage : AbstractGameActionMessage
{

public const ushort Id = 5526;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public uint amount;
        

public GameActionFightReduceDamagesMessage()
{
}

public GameActionFightReduceDamagesMessage(ushort actionId, int sourceId, int targetId, uint amount)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.amount = amount;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteVarUhInt(amount);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            amount = reader.ReadVarUhInt();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
            

}


}


}