


















// Generated on 06/04/2015 18:45:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class HumanOptionGuild : HumanOption
{

public const short Id = 409;
public override short TypeId
{
    get { return Id; }
}

public Types.GuildInformations guildInformations;
        

public HumanOptionGuild()
{
}

public HumanOptionGuild(Types.GuildInformations guildInformations)
        {
            this.guildInformations = guildInformations;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            guildInformations.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            guildInformations = new Types.GuildInformations();
            guildInformations.Deserialize(reader);
            

}


}


}