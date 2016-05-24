


















// Generated on 06/04/2015 18:45:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameRolePlayNpcWithQuestInformations : GameRolePlayNpcInformations
{

public const short Id = 383;
public override short TypeId
{
    get { return Id; }
}

public Types.GameRolePlayNpcQuestFlag questFlag;
        

public GameRolePlayNpcWithQuestInformations()
{
}

public GameRolePlayNpcWithQuestInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, ushort npcId, bool sex, ushort specialArtworkId, Types.GameRolePlayNpcQuestFlag questFlag)
         : base(contextualId, look, disposition, npcId, sex, specialArtworkId)
        {
            this.questFlag = questFlag;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            questFlag.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            questFlag = new Types.GameRolePlayNpcQuestFlag();
            questFlag.Deserialize(reader);
            

}


}


}