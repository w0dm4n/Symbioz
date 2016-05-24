


















// Generated on 06/04/2015 18:45:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class MonsterInGroupInformations : MonsterInGroupLightInformations
{

public const short Id = 144;
public override short TypeId
{
    get { return Id; }
}

public Types.EntityLook look;
        

public MonsterInGroupInformations()
{
}

public MonsterInGroupInformations(int creatureGenericId, sbyte grade, Types.EntityLook look)
         : base(creatureGenericId, grade)
        {
            this.look = look;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            look.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            look = new Types.EntityLook();
            look.Deserialize(reader);
            

}


}


}