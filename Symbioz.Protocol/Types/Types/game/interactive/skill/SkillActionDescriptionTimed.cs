


















// Generated on 06/04/2015 18:45:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class SkillActionDescriptionTimed : SkillActionDescription
{

public const short Id = 103;
public override short TypeId
{
    get { return Id; }
}

public byte time;
        

public SkillActionDescriptionTimed()
{
}

public SkillActionDescriptionTimed(ushort skillId, byte time)
         : base(skillId)
        {
            this.time = time;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteByte(time);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            time = reader.ReadByte();
            if ((time < 0) || (time > 255))
                throw new Exception("Forbidden value on time = " + time + ", it doesn't respect the following condition : (time < 0) || (time > 255)");
            

}


}


}