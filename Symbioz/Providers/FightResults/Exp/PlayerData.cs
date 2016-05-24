using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.FightResults.Exp 
{

    public class PlayerData
    {

        public PlayerData(int level, int wisdom)
        {
            this.level = level;
            this.wisdom = wisdom;
            this.xpBonusPercent = 0;
            this.xpRatioMount = 0;
            this.xpGuildGivenPercent = 0;
            this.xpAlliancePrismBonusPercent = 0;
        }

        public int level;

        public int wisdom;

        public int xpBonusPercent;

        public int xpRatioMount;

        public int xpGuildGivenPercent;

        public int xpAlliancePrismBonusPercent;
    }
}
