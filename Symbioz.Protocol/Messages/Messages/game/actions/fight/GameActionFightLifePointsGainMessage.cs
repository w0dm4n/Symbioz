


















// Generated on 06/04/2015 18:44:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightLifePointsGainMessage : AbstractGameActionMessage
{

public const ushort Id = 6311;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public uint delta;
        

public GameActionFightLifePointsGainMessage()
{
}

public GameActionFightLifePointsGainMessage(ushort actionId, int sourceId, int targetId, uint delta)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.delta = delta;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteVarUhInt(delta);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            delta = reader.ReadVarUhInt();
            if (delta < 0)
                throw new Exception("Forbidden value on delta = " + delta + ", it doesn't respect the following condition : delta < 0");
            

}


}


}