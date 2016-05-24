


















// Generated on 06/04/2015 18:45:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameFightMutantInformations : GameFightFighterNamedInformations
{

public const short Id = 50;
public override short TypeId
{
    get { return Id; }
}

public sbyte powerLevel;
        

public GameFightMutantInformations()
{
}

public GameFightMutantInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, sbyte teamId, sbyte wave, bool alive, Types.GameFightMinimalStats stats, IEnumerable<ushort> previousPositions, string name, Types.PlayerStatus status, sbyte powerLevel)
         : base(contextualId, look, disposition, teamId, wave, alive, stats, previousPositions, name, status)
        {
            this.powerLevel = powerLevel;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(powerLevel);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            powerLevel = reader.ReadSByte();
            if (powerLevel < 0)
                throw new Exception("Forbidden value on powerLevel = " + powerLevel + ", it doesn't respect the following condition : powerLevel < 0");
            

}


}


}