using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.FightResults.Exp
{
    public class MonsterData
    {

        public MonsterData(int level, int xp)
        {
            this.xp = xp;
            this.level = level;
        }

        public int xp;

        public int level;
    }
}
