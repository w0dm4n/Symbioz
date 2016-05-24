


















// Generated on 06/04/2015 18:45:32
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class TreasureHuntStepFollowDirection : TreasureHuntStep
{

public const short Id = 468;
public override short TypeId
{
    get { return Id; }
}

public sbyte direction;
        public ushort mapCount;
        

public TreasureHuntStepFollowDirection()
{
}

public TreasureHuntStepFollowDirection(sbyte direction, ushort mapCount)
        {
            this.direction = direction;
            this.mapCount = mapCount;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(direction);
            writer.WriteVarUhShort(mapCount);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
            mapCount = reader.ReadVarUhShort();
            if (mapCount < 0)
                throw new Exception("Forbidden value on mapCount = " + mapCount + ", it doesn't respect the following condition : mapCount < 0");
            

}


}


}