


















// Generated on 06/04/2015 18:45:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class InteractiveElementNamedSkill : InteractiveElementSkill
{

public const short Id = 220;
public override short TypeId
{
    get { return Id; }
}

public uint nameId;
        

public InteractiveElementNamedSkill()
{
}

public InteractiveElementNamedSkill(uint skillId, int skillInstanceUid, uint nameId)
         : base(skillId, skillInstanceUid)
        {
            this.nameId = nameId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(nameId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            nameId = reader.ReadVarUhInt();
            if (nameId < 0)
                throw new Exception("Forbidden value on nameId = " + nameId + ", it doesn't respect the following condition : nameId < 0");
            

}


}


}