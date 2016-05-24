// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class QuestStep : ID2OClass
    {
        [Cache]
        public static List<QuestStep> QuestSteps = new List<QuestStep>();
        public Int32 id;
        public Int32 questId;
        public Int32 nameId;
        public Int32 descriptionId;
        public Int32 dialogId;
        public Int32 optimalLevel;
        public Double duration;
        public Boolean kamasScaleWithPlayerLevel;
        public Double kamasRatio;
        public Double xpRatio;
        public UInt32[] objectiveIds;
        public ArrayList rewardsIds;
        public QuestStep(Int32 id, Int32 questId, Int32 nameId, Int32 descriptionId, Int32 dialogId, Int32 optimalLevel, Double duration, Boolean kamasScaleWithPlayerLevel, Double kamasRatio, Double xpRatio, UInt32[] objectiveIds, ArrayList rewardsIds)
        {
            this.id = id;
            this.questId = questId;
            this.nameId = nameId;
            this.descriptionId = descriptionId;
            this.dialogId = dialogId;
            this.optimalLevel = optimalLevel;
            this.duration = duration;
            this.kamasScaleWithPlayerLevel = kamasScaleWithPlayerLevel;
            this.kamasRatio = kamasRatio;
            this.xpRatio = xpRatio;
            this.objectiveIds = objectiveIds;
            this.rewardsIds = rewardsIds;
        }
    }
}
