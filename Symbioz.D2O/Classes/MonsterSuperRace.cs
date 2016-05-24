// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class MonsterSuperRace : ID2OClass
    {
        [Cache]
        public static List<MonsterSuperRace> MonsterSuperRaces = new List<MonsterSuperRace>();
        public Int32 id;
        public Int32 nameId;
        public MonsterSuperRace(Int32 id, Int32 nameId)
        {
            this.id = id;
            this.nameId = nameId;
        }
    }
}
