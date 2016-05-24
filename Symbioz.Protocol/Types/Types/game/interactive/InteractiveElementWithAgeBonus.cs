


















// Generated on 06/04/2015 18:45:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class InteractiveElementWithAgeBonus : InteractiveElement
{

public const short Id = 398;
public override short TypeId
{
    get { return Id; }
}

public short ageBonus;
        

public InteractiveElementWithAgeBonus()
{
}

public InteractiveElementWithAgeBonus(int elementId, int elementTypeId, IEnumerable<Types.InteractiveElementSkill> enabledSkills, IEnumerable<Types.InteractiveElementSkill> disabledSkills, short ageBonus)
         : base(elementId, elementTypeId, enabledSkills, disabledSkills)
        {
            this.ageBonus = ageBonus;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteShort(ageBonus);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            ageBonus = reader.ReadShort();
            if ((ageBonus < -1) || (ageBonus > 1000))
                throw new Exception("Forbidden value on ageBonus = " + ageBonus + ", it doesn't respect the following condition : (ageBonus < -1) || (ageBonus > 1000)");
            

}


}


}