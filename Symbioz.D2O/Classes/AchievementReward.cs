// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AchievementReward : ID2OClass
    {
        [Cache]
        public static List<AchievementReward> AchievementRewards = new List<AchievementReward>();
        public Int32 id;
        public Int32 achievementId;
        public Int32 levelMin;
        public Int32 levelMax;
        public UInt32[] itemsReward;
        public UInt32[] itemsQuantityReward;
        public ArrayList emotesReward;
        public ArrayList spellsReward;
        public ArrayList titlesReward;
        public ArrayList ornamentsReward;
        public AchievementReward(Int32 id, Int32 achievementId, Int32 levelMin, Int32 levelMax, UInt32[] itemsReward, UInt32[] itemsQuantityReward, ArrayList emotesReward, ArrayList spellsReward, ArrayList titlesReward, ArrayList ornamentsReward)
        {
            this.id = id;
            this.achievementId = achievementId;
            this.levelMin = levelMin;
            this.levelMax = levelMax;
            this.itemsReward = itemsReward;
            this.itemsQuantityReward = itemsQuantityReward;
            this.emotesReward = emotesReward;
            this.spellsReward = spellsReward;
            this.titlesReward = titlesReward;
            this.ornamentsReward = ornamentsReward;
        }
    }
}
