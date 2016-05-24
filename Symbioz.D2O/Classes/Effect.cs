// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Effect : ID2OClass
    {
        [Cache]
        public static List<Effect> Effects = new List<Effect>();
        public Int32 id;
        public Int32 descriptionId;
        public Int32 iconId;
        public Int32 characteristic;
        public Int32 category;
        public Boolean showInTooltip;
        public Boolean useDice;
        public Boolean forceMinMax;
        public Boolean boost;
        public Boolean active;
        public Int32 oppositeId;
        public Int32 theoreticalDescriptionId;
        public Int32 theoreticalPattern;
        public Boolean showInSet;
        public Int32 bonusType;
        public Boolean useInFight;
        public Int32 effectPriority;
        public Int32 elementId;
        public Effect(Int32 id, Int32 descriptionId, Int32 iconId, Int32 characteristic, Int32 category, Boolean showInTooltip, Boolean useDice, Boolean forceMinMax, Boolean boost, Boolean active, Int32 oppositeId, Int32 theoreticalDescriptionId, Int32 theoreticalPattern, Boolean showInSet, Int32 bonusType, Boolean useInFight, Int32 effectPriority, Int32 elementId)
        {
            this.id = id;
            this.descriptionId = descriptionId;
            this.iconId = iconId;
            this.characteristic = characteristic;
            this.category = category;
            this.showInTooltip = showInTooltip;
            this.useDice = useDice;
            this.forceMinMax = forceMinMax;
            this.boost = boost;
            this.active = active;
            this.oppositeId = oppositeId;
            this.theoreticalDescriptionId = theoreticalDescriptionId;
            this.theoreticalPattern = theoreticalPattern;
            this.showInSet = showInSet;
            this.bonusType = bonusType;
            this.useInFight = useInFight;
            this.effectPriority = effectPriority;
            this.elementId = elementId;
        }
    }
}
