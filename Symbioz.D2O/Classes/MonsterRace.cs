// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class MonsterRace : ID2OClass
    {
        [Cache]
        public static List<MonsterRace> MonsterRaces = new List<MonsterRace>();
        public Int32 id;
        public Int32 superRaceId;
        public Int32 nameId;
        public UInt32[] monsters;
        public MonsterRace(Int32 id, Int32 superRaceId, Int32 nameId, UInt32[] monsters)
        {
            this.id = id;
            this.superRaceId = superRaceId;
            this.nameId = nameId;
            this.monsters = monsters;
        }
    }
}
