using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.FightResults.Exp
{
    public class GroupMemberData
    {
        public GroupMemberData(int level, bool isCompanion)
        {
            this.level = level;
            this.companion = isCompanion;
        }

        public int level;

        public bool companion;
    }
}
