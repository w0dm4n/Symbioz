


















// Generated on 06/04/2015 18:44:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightLifePointsLostMessage : AbstractGameActionMessage
{

public const ushort Id = 6312;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public ushort loss;
        public ushort permanentDamages;
        

public GameActionFightLifePointsLostMessage()
{
}

public GameActionFightLifePointsLostMessage(ushort actionId, int sourceId, int targetId, ushort loss, ushort permanentDamages)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.loss = loss;
            this.permanentDamages = permanentDamages;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteVarUhShort(loss);
            writer.WriteVarUhShort(permanentDamages);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            loss = reader.ReadVarUhShort();
            if (loss < 0)
                throw new Exception("Forbidden value on loss = " + loss + ", it doesn't respect the following condition : loss < 0");
            permanentDamages = reader.ReadVarUhShort();
            if (permanentDamages < 0)
                throw new Exception("Forbidden value on permanentDamages = " + permanentDamages + ", it doesn't respect the following condition : permanentDamages < 0");
            

}


}


}