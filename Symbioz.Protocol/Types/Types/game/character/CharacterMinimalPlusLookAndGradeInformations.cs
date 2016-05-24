


















// Generated on 06/04/2015 18:45:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class CharacterMinimalPlusLookAndGradeInformations : CharacterMinimalPlusLookInformations
{

public const short Id = 193;
public override short TypeId
{
    get { return Id; }
}

public uint grade;
        

public CharacterMinimalPlusLookAndGradeInformations()
{
}

public CharacterMinimalPlusLookAndGradeInformations(uint id, byte level, string name, Types.EntityLook entityLook, uint grade)
         : base(id, level, name, entityLook)
        {
            this.grade = grade;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(grade);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            grade = reader.ReadVarUhInt();
            if (grade < 0)
                throw new Exception("Forbidden value on grade = " + grade + ", it doesn't respect the following condition : grade < 0");
            

}


}


}