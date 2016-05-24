


















// Generated on 06/04/2015 18:45:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class CharacterMinimalGuildInformations : CharacterMinimalPlusLookInformations
{

public const short Id = 445;
public override short TypeId
{
    get { return Id; }
}

public Types.BasicGuildInformations guild;
        

public CharacterMinimalGuildInformations()
{
}

public CharacterMinimalGuildInformations(uint id, byte level, string name, Types.EntityLook entityLook, Types.BasicGuildInformations guild)
         : base(id, level, name, entityLook)
        {
            this.guild = guild;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            guild.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            guild = new Types.BasicGuildInformations();
            guild.Deserialize(reader);
            

}


}


}