


















// Generated on 06/04/2015 18:45:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameRolePlayMutantInformations : GameRolePlayHumanoidInformations
{

public const short Id = 3;
public override short TypeId
{
    get { return Id; }
}

public ushort monsterId;
        public sbyte powerLevel;
        

public GameRolePlayMutantInformations()
{
}

public GameRolePlayMutantInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, Types.HumanInformations humanoidInfo, int accountId, ushort monsterId, sbyte powerLevel)
         : base(contextualId, look, disposition, name, humanoidInfo, accountId)
        {
            this.monsterId = monsterId;
            this.powerLevel = powerLevel;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(monsterId);
            writer.WriteSByte(powerLevel);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            monsterId = reader.ReadVarUhShort();
            if (monsterId < 0)
                throw new Exception("Forbidden value on monsterId = " + monsterId + ", it doesn't respect the following condition : monsterId < 0");
            powerLevel = reader.ReadSByte();
            

}


}


}