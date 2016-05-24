


















// Generated on 06/04/2015 18:45:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameRolePlayTreasureHintInformations : GameRolePlayActorInformations
{

public const short Id = 471;
public override short TypeId
{
    get { return Id; }
}

public ushort npcId;
        

public GameRolePlayTreasureHintInformations()
{
}

public GameRolePlayTreasureHintInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, ushort npcId)
         : base(contextualId, look, disposition)
        {
            this.npcId = npcId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(npcId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            npcId = reader.ReadVarUhShort();
            if (npcId < 0)
                throw new Exception("Forbidden value on npcId = " + npcId + ", it doesn't respect the following condition : npcId < 0");
            

}


}


}