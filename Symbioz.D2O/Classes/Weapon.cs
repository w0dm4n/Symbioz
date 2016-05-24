// automatic generation Symbioz.Sync 2015

using Symbioz.D2O.InternalClasses;
using Symbioz.DofusProtocol.D2O;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Weapon : ID2OClass
    {
        [Cache]
        public static List<Weapon> Weapons = new List<Weapon>();
        public Int32 favoriteSubAreasBonus;
        public Int32 craftXpRatio;
        public Int32 range;
        public Boolean bonusIsSecret;
        public Int32 criticalHitBonus;
        public String criteriaTarget;
        public Int32 minRange;
        public Int32 maxCastPerTurn;
        public Int32 descriptionId;
        public ArrayList recipeIds;
        public Boolean secretRecipe;
        public Boolean etheral;
        public Int32 appearanceId;
        public Int32 id;
        public ArrayList dropMonsterIds;
        public Boolean cursed;
        public Boolean exchangeable;
        public Int32 level;
        public Int32 realWeight;
        public Boolean castTestLos;
        public ArrayList favoriteSubAreas;
        public Int32 criticalFailureProbability;
        public Boolean hideEffects;
        public String criteria;
        public Boolean targetable;
        public Int32 criticalHitProbability;
        public Boolean twoHanded;
        public Boolean nonUsableOnAnother;
        public Int32 itemSetId;
        public Int32 nameId;
        public Boolean castInDiagonal;
        public Double price;
        public Boolean enhanceable;
        public Boolean needUseConfirm;
        public Int32 apCost;
        public Boolean usable;
        public Boolean castInLine;
        public EffectInstanceDice[] possibleEffects;
        public Int32 useAnimationId;
        public Int32 iconId;
        public Int32 typeId;
        public Int32 recipeSlots;
        public Weapon(Int32 favoriteSubAreasBonus, Int32 craftXpRatio, Int32 range, Boolean bonusIsSecret, Int32 criticalHitBonus, String criteriaTarget, Int32 minRange, Int32 maxCastPerTurn, Int32 descriptionId, ArrayList recipeIds, Boolean secretRecipe, Boolean etheral, Int32 appearanceId, Int32 id, ArrayList dropMonsterIds, Boolean cursed, Boolean exchangeable, Int32 level, Int32 realWeight, Boolean castTestLos, ArrayList favoriteSubAreas, Int32 criticalFailureProbability, Boolean hideEffects, String criteria, Boolean targetable, Int32 criticalHitProbability, Boolean twoHanded, Boolean nonUsableOnAnother, Int32 itemSetId, Int32 nameId, Boolean castInDiagonal, Double price, Boolean enhanceable, Boolean needUseConfirm, Int32 apCost, Boolean usable, Boolean castInLine, object[] possibleEffects, Int32 useAnimationId, Int32 iconId, Int32 typeId, Int32 recipeSlots)
        {
            this.favoriteSubAreasBonus = favoriteSubAreasBonus;
            this.craftXpRatio = craftXpRatio;
            this.range = range;
            this.bonusIsSecret = bonusIsSecret;
            this.criticalHitBonus = criticalHitBonus;
            this.criteriaTarget = criteriaTarget;
            this.minRange = minRange;
            this.maxCastPerTurn = maxCastPerTurn;
            this.descriptionId = descriptionId;
            this.recipeIds = recipeIds;
            this.secretRecipe = secretRecipe;
            this.etheral = etheral;
            this.appearanceId = appearanceId;
            this.id = id;
            this.dropMonsterIds = dropMonsterIds;
            this.cursed = cursed;
            this.exchangeable = exchangeable;
            this.level = level;
            this.realWeight = realWeight;
            this.castTestLos = castTestLos;
            this.favoriteSubAreas = favoriteSubAreas;
            this.criticalFailureProbability = criticalFailureProbability;
            this.hideEffects = hideEffects;
            this.criteria = criteria;
            this.targetable = targetable;
            this.criticalHitProbability = criticalHitProbability;
            this.twoHanded = twoHanded;
            this.nonUsableOnAnother = nonUsableOnAnother;
            this.itemSetId = itemSetId;
            this.nameId = nameId;
            this.castInDiagonal = castInDiagonal;
            this.price = price;
            this.enhanceable = enhanceable;
            this.needUseConfirm = needUseConfirm;
            this.apCost = apCost;
            this.usable = usable;
            this.castInLine = castInLine;
            this.possibleEffects = possibleEffects.Cast<EffectInstanceDice>().ToArray();
            this.useAnimationId = useAnimationId;
            this.iconId = iconId;
            this.typeId = typeId;
            this.recipeSlots = recipeSlots;
        }
    }
}
