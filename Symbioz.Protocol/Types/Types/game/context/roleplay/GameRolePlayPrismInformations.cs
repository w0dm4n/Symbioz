


















// Generated on 06/04/2015 18:45:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameRolePlayPrismInformations : GameRolePlayActorInformations
{

public const short Id = 161;
public override short TypeId
{
    get { return Id; }
}

public Types.PrismInformation prism;
        

public GameRolePlayPrismInformations()
{
}

public GameRolePlayPrismInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, Types.PrismInformation prism)
         : base(contextualId, look, disposition)
        {
            this.prism = prism;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteShort(prism.TypeId);
            prism.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            prism = Types.ProtocolTypeManager.GetInstance<Types.PrismInformation>(reader.ReadShort());
            prism.Deserialize(reader);
            

}


}


}