// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class EffectInstanceDice : ID2OInternalClass
    {
        
        public Boolean visibleInTooltip;
        public Int32 random;
        public String rawZone;
        public Int32 targetId;
        public String targetMask;
        public Int32 effectId;
        public Int32 diceNum;
        public Int32 duration;
        public Boolean visibleInFightLog;
        public Int32 effectUid;
        public Int32 diceSide;
        public Int32 value;
        public Boolean visibleInBuffUi;
        public Int32 delay;
        public String triggers;
        public Int32 group;

        public EffectInstanceDice(Boolean visibleInTooltip, Int32 random, String rawZone, Int32 targetId, String targetMask, Int32 effectId, Int32 diceNum, Int32 duration, Boolean visibleInFightLog, Int32 effectUid, Int32 diceSide, Int32 value, Boolean visibleInBuffUi, Int32 delay, String triggers, Int32 group)
        {
            this.visibleInTooltip = visibleInTooltip;
            this.random = random;
            this.rawZone = rawZone;
            this.targetId = targetId;
            this.targetMask = targetMask;
            this.effectId = effectId;
            this.diceNum = diceNum;
            this.duration = duration;
            this.visibleInFightLog = visibleInFightLog;
            this.effectUid = effectUid;
            this.diceSide = diceSide;
            this.value = value;
            this.visibleInBuffUi = visibleInBuffUi;
            this.delay = delay;
            this.triggers = triggers;
            this.group = group;
        }

       
    }
}
