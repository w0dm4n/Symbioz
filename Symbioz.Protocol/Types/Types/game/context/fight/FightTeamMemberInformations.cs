


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightTeamMemberInformations
{

public const short Id = 44;
public virtual short TypeId
{
    get { return Id; }
}

public int id;
        

public FightTeamMemberInformations()
{
}

public FightTeamMemberInformations(int id)
        {
            this.id = id;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(id);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

id = reader.ReadInt();
            

}


}


}