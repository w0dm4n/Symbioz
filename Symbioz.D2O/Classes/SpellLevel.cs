// automatic generation Symbioz.Sync 2015

using Symbioz.D2O.InternalClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SpellLevel : ID2OClass
    {
        [Cache]
        public static List<SpellLevel> SpellLevels = new List<SpellLevel>();
        public Int32 id;
        public Int32 spellId;
        public Int32 grade;
        public Int32 spellBreed;
        public Int32 apCost;
        public Int32 minRange;
        public Int32 range;
        public Boolean castInLine;
        public Boolean castInDiagonal;
        public Boolean castTestLos;
        public Int32 criticalHitProbability;
        public Int32 criticalFailureProbability;
        public Boolean needFreeCell;
        public Boolean needTakenCell;
        public Boolean needFreeTrapCell;
        public Boolean rangeCanBeBoosted;
        public Int32 maxStack;
        public Int32 maxCastPerTurn;
        public Int32 maxCastPerTarget;
        public Int32 minCastInterval;
        public Int32 initialCooldown;
        public Int32 globalCooldown;
        public Int32 minPlayerLevel;
        public Boolean criticalFailureEndsTurn;
        public Boolean hideEffects;
        public Boolean hidden;
        public ArrayList statesRequired;
        public ArrayList statesForbidden;
        public EffectInstanceDice[] effects;
        public EffectInstanceDice[] criticalEffect;
        public SpellLevel(Int32 id, Int32 spellId, Int32 grade, Int32 spellBreed, Int32 apCost, Int32 minRange, Int32 range, Boolean castInLine, Boolean castInDiagonal, Boolean castTestLos, Int32 criticalHitProbability, Int32 criticalFailureProbability, Boolean needFreeCell, Boolean needTakenCell, Boolean needFreeTrapCell, Boolean rangeCanBeBoosted, Int32 maxStack, Int32 maxCastPerTurn, Int32 maxCastPerTarget, Int32 minCastInterval, Int32 initialCooldown, Int32 globalCooldown, Int32 minPlayerLevel, Boolean criticalFailureEndsTurn, Boolean hideEffects, Boolean hidden, ArrayList statesRequired, ArrayList statesForbidden, object[] effects, object[] criticalEffect)
        {
            this.id = id;
            this.spellId = spellId;
            this.grade = grade;
            this.spellBreed = spellBreed;
            this.apCost = apCost;
            this.minRange = minRange;
            this.range = range;
            this.castInLine = castInLine;
            this.castInDiagonal = castInDiagonal;
            this.castTestLos = castTestLos;
            this.criticalHitProbability = criticalHitProbability;
            this.criticalFailureProbability = criticalFailureProbability;
            this.needFreeCell = needFreeCell;
            this.needTakenCell = needTakenCell;
            this.needFreeTrapCell = needFreeTrapCell;
            this.rangeCanBeBoosted = rangeCanBeBoosted;
            this.maxStack = maxStack;
            this.maxCastPerTurn = maxCastPerTurn;
            this.maxCastPerTarget = maxCastPerTarget;
            this.minCastInterval = minCastInterval;
            this.initialCooldown = initialCooldown;
            this.globalCooldown = globalCooldown;
            this.minPlayerLevel = minPlayerLevel;
            this.criticalFailureEndsTurn = criticalFailureEndsTurn;
            this.hideEffects = hideEffects;
            this.hidden = hidden;
            this.statesRequired = statesRequired;
            this.statesForbidden = statesForbidden;
            this.effects = effects.Cast<EffectInstanceDice>().ToArray();
            this.criticalEffect = criticalEffect.Cast<EffectInstanceDice>().ToArray();
        }
    }
}
