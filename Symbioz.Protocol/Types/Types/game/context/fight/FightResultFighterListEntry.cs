


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightResultFighterListEntry : FightResultListEntry
{

public const short Id = 189;
public override short TypeId
{
    get { return Id; }
}

public int id;
        public bool alive;
        

public FightResultFighterListEntry()
{
}

public FightResultFighterListEntry(ushort outcome, sbyte wave, Types.FightLoot rewards, int id, bool alive)
         : base(outcome, wave, rewards)
        {
            this.id = id;
            this.alive = alive;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(id);
            writer.WriteBoolean(alive);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            id = reader.ReadInt();
            alive = reader.ReadBoolean();
            

}


}


}