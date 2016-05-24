


















// Generated on 06/04/2015 18:45:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class HumanOptionObjectUse : HumanOption
{

public const short Id = 449;
public override short TypeId
{
    get { return Id; }
}

public sbyte delayTypeId;
        public double delayEndTime;
        public ushort objectGID;
        

public HumanOptionObjectUse()
{
}

public HumanOptionObjectUse(sbyte delayTypeId, double delayEndTime, ushort objectGID)
        {
            this.delayTypeId = delayTypeId;
            this.delayEndTime = delayEndTime;
            this.objectGID = objectGID;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(delayTypeId);
            writer.WriteDouble(delayEndTime);
            writer.WriteVarUhShort(objectGID);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            delayTypeId = reader.ReadSByte();
            if (delayTypeId < 0)
                throw new Exception("Forbidden value on delayTypeId = " + delayTypeId + ", it doesn't respect the following condition : delayTypeId < 0");
            delayEndTime = reader.ReadDouble();
            if ((delayEndTime < 0) || (delayEndTime > 9.007199254740992E15))
                throw new Exception("Forbidden value on delayEndTime = " + delayEndTime + ", it doesn't respect the following condition : (delayEndTime < 0) || (delayEndTime > 9.007199254740992E15)");
            objectGID = reader.ReadVarUhShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            

}


}


}