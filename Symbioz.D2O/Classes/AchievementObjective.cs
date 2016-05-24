// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AchievementObjective : ID2OClass
    {
        [Cache]
        public static List<AchievementObjective> AchievementObjectives = new List<AchievementObjective>();
        public Int32 id;
        public Int32 achievementId;
        public Int32 order;
        public Int32 nameId;
        public String criterion;
        public AchievementObjective(Int32 id, Int32 achievementId, Int32 order, Int32 nameId, String criterion)
        {
            this.id = id;
            this.achievementId = achievementId;
            this.order = order;
            this.nameId = nameId;
            this.criterion = criterion;
        }
    }
}
