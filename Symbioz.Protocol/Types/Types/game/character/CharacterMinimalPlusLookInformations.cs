


















// Generated on 06/04/2015 18:45:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class CharacterMinimalPlusLookInformations : CharacterMinimalInformations
{

public const short Id = 163;
public override short TypeId
{
    get { return Id; }
}

public Types.EntityLook entityLook;
        

public CharacterMinimalPlusLookInformations()
{
}

public CharacterMinimalPlusLookInformations(uint id, byte level, string name, Types.EntityLook entityLook)
         : base(id, level, name)
        {
            this.entityLook = entityLook;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            entityLook.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            entityLook = new Types.EntityLook();
            entityLook.Deserialize(reader);
            

}


}


}