// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Quest : ID2OClass
    {
        [Cache]
        public static List<Quest> Quests = new List<Quest>();
        public Int32 id;
        public Int32 nameId;
        public Int32 categoryId;
        public Boolean isRepeatable;
        public Int32 repeatType;
        public Int32 repeatLimit;
        public Boolean isDungeonQuest;
        public Int32 levelMin;
        public Int32 levelMax;
        public UInt32[] stepIds;
        public Boolean isPartyQuest;
        public Quest(Int32 id, Int32 nameId, Int32 categoryId, Boolean isRepeatable, Int32 repeatType, Int32 repeatLimit, Boolean isDungeonQuest, Int32 levelMin, Int32 levelMax, UInt32[] stepIds, Boolean isPartyQuest)
        {
            this.id = id;
            this.nameId = nameId;
            this.categoryId = categoryId;
            this.isRepeatable = isRepeatable;
            this.repeatType = repeatType;
            this.repeatLimit = repeatLimit;
            this.isDungeonQuest = isDungeonQuest;
            this.levelMin = levelMin;
            this.levelMax = levelMax;
            this.stepIds = stepIds;
            this.isPartyQuest = isPartyQuest;
        }
    }
}
