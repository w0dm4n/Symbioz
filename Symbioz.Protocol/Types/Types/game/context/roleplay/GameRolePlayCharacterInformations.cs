


















// Generated on 06/04/2015 18:45:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameRolePlayCharacterInformations : GameRolePlayHumanoidInformations
{

public const short Id = 36;
public override short TypeId
{
    get { return Id; }
}

public Types.ActorAlignmentInformations alignmentInfos;
        

public GameRolePlayCharacterInformations()
{
}

public GameRolePlayCharacterInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, Types.HumanInformations humanoidInfo, int accountId, Types.ActorAlignmentInformations alignmentInfos)
         : base(contextualId, look, disposition, name, humanoidInfo, accountId)
        {
            this.alignmentInfos = alignmentInfos;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            alignmentInfos.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            alignmentInfos = new Types.ActorAlignmentInformations();
            alignmentInfos.Deserialize(reader);
            

}


}


}