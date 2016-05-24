// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class QuestStepRewards : ID2OClass
    {
        [Cache]
        public static List<QuestStepRewards> QuestStepRewardss = new List<QuestStepRewards>();
        public Int32 id;
        public Int32 stepId;
        public Int32 levelMin;
        public Int32 levelMax;
        public ArrayList[] itemsReward;
        public ArrayList emotesReward;
        public ArrayList jobsReward;
        public ArrayList spellsReward;
        public QuestStepRewards(Int32 id, Int32 stepId, Int32 levelMin, Int32 levelMax, ArrayList[] itemsReward, ArrayList emotesReward, ArrayList jobsReward, ArrayList spellsReward)
        {
            this.id = id;
            this.stepId = stepId;
            this.levelMin = levelMin;
            this.levelMax = levelMax;
            this.itemsReward = itemsReward;
            this.emotesReward = emotesReward;
            this.jobsReward = jobsReward;
            this.spellsReward = spellsReward;
        }
    }
}
