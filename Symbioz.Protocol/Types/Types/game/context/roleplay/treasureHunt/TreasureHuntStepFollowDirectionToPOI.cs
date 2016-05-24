


















// Generated on 06/04/2015 18:45:32
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class TreasureHuntStepFollowDirectionToPOI : TreasureHuntStep
{

public const short Id = 461;
public override short TypeId
{
    get { return Id; }
}

public sbyte direction;
        public ushort poiLabelId;
        

public TreasureHuntStepFollowDirectionToPOI()
{
}

public TreasureHuntStepFollowDirectionToPOI(sbyte direction, ushort poiLabelId)
        {
            this.direction = direction;
            this.poiLabelId = poiLabelId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(direction);
            writer.WriteVarUhShort(poiLabelId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
            poiLabelId = reader.ReadVarUhShort();
            if (poiLabelId < 0)
                throw new Exception("Forbidden value on poiLabelId = " + poiLabelId + ", it doesn't respect the following condition : poiLabelId < 0");
            

}


}


}