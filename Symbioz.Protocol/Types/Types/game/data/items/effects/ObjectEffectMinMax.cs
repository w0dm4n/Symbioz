


















// Generated on 06/04/2015 18:45:33
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ObjectEffectMinMax : ObjectEffect
{

public const short Id = 82;
public override short TypeId
{
    get { return Id; }
}

public uint min { get; set; }
public uint max { get; set; }
        

public ObjectEffectMinMax()
{
}

public ObjectEffectMinMax(ushort actionId, uint min, uint max)
         : base(actionId)
        {
            this.min = min;
            this.max = max;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(min);
            writer.WriteVarUhInt(max);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            min = reader.ReadVarUhInt();
            if (min < 0)
                throw new Exception("Forbidden value on min = " + min + ", it doesn't respect the following condition : min < 0");
            max = reader.ReadVarUhInt();
            if (max < 0)
                throw new Exception("Forbidden value on max = " + max + ", it doesn't respect the following condition : max < 0");
            

}


}


}