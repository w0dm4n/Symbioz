


















// Generated on 06/04/2015 18:45:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameRolePlayActorInformations : GameContextActorInformations
{

public const short Id = 141;
public override short TypeId
{
    get { return Id; }
}



public GameRolePlayActorInformations()
{
}

public GameRolePlayActorInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition)
         : base(contextualId, look, disposition)
        {
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            

}


}


}