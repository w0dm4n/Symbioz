// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AchievementCategory : ID2OClass
    {
        [Cache]
        public static List<AchievementCategory> AchievementCategorys = new List<AchievementCategory>();
        public Int32 id;
        public Int32 nameId;
        public Int32 parentId;
        public String icon;
        public Int32 order;
        public String color;
        public UInt32[] achievementIds;
        public AchievementCategory(Int32 id, Int32 nameId, Int32 parentId, String icon, Int32 order, String color, UInt32[] achievementIds)
        {
            this.id = id;
            this.nameId = nameId;
            this.parentId = parentId;
            this.icon = icon;
            this.order = order;
            this.color = color;
            this.achievementIds = achievementIds;
        }
    }
}
