// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Achievement : ID2OClass
    {
        [Cache]
        public static List<Achievement> Achievements = new List<Achievement>();
        public Int32 id;
        public Int32 nameId;
        public Int32 categoryId;
        public Int32 descriptionId;
        public Int32 iconId;
        public Int32 points;
        public Int32 level;
        public Int32 order;
        public Double kamasRatio;
        public Double experienceRatio;
        public Boolean kamasScaleWithPlayerLevel;
        public Int32[] objectiveIds;
        public ArrayList rewardIds;
        public Achievement(Int32 id, Int32 nameId, Int32 categoryId, Int32 descriptionId, Int32 iconId, Int32 points, Int32 level, Int32 order, Double kamasRatio, Double experienceRatio, Boolean kamasScaleWithPlayerLevel, Int32[] objectiveIds, ArrayList rewardIds)
        {
            this.id = id;
            this.nameId = nameId;
            this.categoryId = categoryId;
            this.descriptionId = descriptionId;
            this.iconId = iconId;
            this.points = points;
            this.level = level;
            this.order = order;
            this.kamasRatio = kamasRatio;
            this.experienceRatio = experienceRatio;
            this.kamasScaleWithPlayerLevel = kamasScaleWithPlayerLevel;
            this.objectiveIds = objectiveIds;
            this.rewardIds = rewardIds;
        }
    }
}
