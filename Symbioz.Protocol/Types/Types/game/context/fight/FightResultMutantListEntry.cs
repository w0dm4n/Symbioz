


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightResultMutantListEntry : FightResultFighterListEntry
{

public const short Id = 216;
public override short TypeId
{
    get { return Id; }
}

public ushort level;
        

public FightResultMutantListEntry()
{
}

public FightResultMutantListEntry(ushort outcome, sbyte wave, Types.FightLoot rewards, int id, bool alive, ushort level)
         : base(outcome, wave, rewards, id, alive)
        {
            this.level = level;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(level);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            level = reader.ReadVarUhShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            

}


}


}