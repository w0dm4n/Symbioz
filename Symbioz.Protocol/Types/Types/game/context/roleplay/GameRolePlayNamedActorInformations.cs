


















// Generated on 06/04/2015 18:45:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameRolePlayNamedActorInformations : GameRolePlayActorInformations
{

public const short Id = 154;
public override short TypeId
{
    get { return Id; }
}

public string name;
        

public GameRolePlayNamedActorInformations()
{
}

public GameRolePlayNamedActorInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name)
         : base(contextualId, look, disposition)
        {
            this.name = name;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(name);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            name = reader.ReadUTF();
            

}


}


}