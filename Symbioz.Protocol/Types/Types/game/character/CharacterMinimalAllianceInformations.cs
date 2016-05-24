


















// Generated on 06/04/2015 18:45:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class CharacterMinimalAllianceInformations : CharacterMinimalGuildInformations
{

public const short Id = 444;
public override short TypeId
{
    get { return Id; }
}

public Types.BasicAllianceInformations alliance;
        

public CharacterMinimalAllianceInformations()
{
}

public CharacterMinimalAllianceInformations(uint id, byte level, string name, Types.EntityLook entityLook, Types.BasicGuildInformations guild, Types.BasicAllianceInformations alliance)
         : base(id, level, name, entityLook, guild)
        {
            this.alliance = alliance;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            alliance.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            alliance = new Types.BasicAllianceInformations();
            alliance.Deserialize(reader);
            

}


}


}