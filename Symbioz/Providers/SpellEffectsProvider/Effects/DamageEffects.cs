using Symbioz.Enums;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider
{
    class DamageEffects
    {
        static void Steal(Fighter fighter, Fighter target, short jet, ElementType element, SpellLevelRecord level)
        {
            var takenDamages = new TakenDamages(jet, element);
            takenDamages.EvaluateWithResistances(fighter, target, fighter.Fight.PvP);
            short healJet = (short)(takenDamages.Delta / 2);
            short targetLife = target.FighterStats.Stats.LifePoints;
            target.TakeDamages(new TakenDamages(jet, element), fighter.ContextualId);
            if (targetLife - jet < 0)
            {
                fighter.Heal((short)(targetLife / 2), fighter.ContextualId);
            }
            else
                fighter.Heal(healJet, fighter.ContextualId);

        }
        [EffectHandler(EffectsEnum.Eff_StealHPFix)]
        public static void StealHPFix(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castercellid)
        {
            foreach (var target in affecteds)
            {
                Steal(fighter, target, effect.BaseEffect.DiceNum, ElementType.Neutral, level);
            }
        }
        [EffectHandler(EffectsEnum.Eff_Punishment_Damage)]
        public static void PunishementDamage(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castercellid)
        {
            double num = 0.0;
            double num2 = (double)fighter.FighterStats.Stats.LifePoints / (double)fighter.FighterStats.RealStats.LifePoints;
            if (num2 <= 0.5)
            {
                num = 2.0 * num2;
            }
            else
            {
                if (num2 > 0.5)
                {
                    num = 1.0 + (num2 - 0.5) * -2.0;
                }
            }
            short jet = (short)((double)fighter.FighterStats.RealStats.LifePoints * num * (double)effect.BaseEffect.DiceNum / 100.0);

            foreach (var target in affecteds)
            {
                target.TakeDamages(new TakenDamages(jet, ElementType.Neutral), fighter.ContextualId);
            }
        }
        [EffectHandler(EffectsEnum.Eff_StealHPEarth)]
        public static void StealEarth(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castercellid)
        {
            var jet = fighter.CalculateJet(effect, fighter.FighterStats.Stats.Strength);
            affecteds.ForEach(x => Steal(fighter, x, jet, ElementType.Earth, level));
        }
        [EffectHandler(EffectsEnum.Eff_StealHPWater)]
        public static void StealWater(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castercellid)
        {
            var jet = fighter.CalculateJet(effect, fighter.FighterStats.Stats.Chance);
            affecteds.ForEach(x => Steal(fighter, x, jet, ElementType.Water, level));
        }
        [EffectHandler(EffectsEnum.Eff_StealHPFire)]
        public static void StealFire(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castercellid)
        {
            var jet = fighter.CalculateJet(effect, fighter.FighterStats.Stats.Intelligence);
            affecteds.ForEach(x => Steal(fighter, x, jet, ElementType.Fire, level));

        }
        [EffectHandler(EffectsEnum.Eff_StealHPAir)]
        public static void StealAir(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castercellid)
        {
            var jet = fighter.CalculateJet(effect, fighter.FighterStats.Stats.Agility);
            affecteds.ForEach(x => Steal(fighter, x, jet, ElementType.Air, level));

        }
        [EffectHandler(EffectsEnum.Eff_DamageEarth)]
        public static void DamageEarth(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castcellid)
        {
            var jet = fighter.CalculateJet(record, fighter.FighterStats.Stats.Strength);
            affecteds.ForEach(x => x.TakeDamages(new TakenDamages(jet, ElementType.Earth), fighter.ContextualId));

        }
        [EffectHandler(EffectsEnum.Eff_DamageFire)]
        public static void DamageFire(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castcellid)
        {
            var jet = fighter.CalculateJet(record, fighter.FighterStats.Stats.Intelligence);
            affecteds.ForEach(x => x.TakeDamages(new TakenDamages(jet, ElementType.Fire), fighter.ContextualId));

        }
        [EffectHandler(EffectsEnum.Eff_DamageWater)]
        public static void DamageWater(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castcellid)
        {
            var jet = fighter.CalculateJet(record, fighter.FighterStats.Stats.Chance);
            affecteds.ForEach(x => x.TakeDamages(new TakenDamages(jet, ElementType.Water), fighter.ContextualId));

        }
        [EffectHandler(EffectsEnum.Eff_DamageAir)]
        public static void DamageAir(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castcellid)
        {
            var jet = fighter.CalculateJet(record, fighter.FighterStats.Stats.Agility);
            affecteds.ForEach(x => x.TakeDamages(new TakenDamages(jet, ElementType.Air), fighter.ContextualId));

        }
        [EffectHandler(EffectsEnum.Eff_DamageNeutral)]
        public static void DamageNeutral(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castcellid)
        {
            var jet = fighter.CalculateJet(record, fighter.FighterStats.Stats.NeutralDamageBonus);
            affecteds.ForEach(x => x.TakeDamages(new TakenDamages(jet, ElementType.Neutral), fighter.ContextualId));

        }
        [EffectHandler(EffectsEnum.Eff_Kill)]
        public static void Kill(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castcellid)
        {
            affecteds.ForEach(x => x.Die());
        }
        /// <summary>
        /// Fury sacrieur
        /// </summary>
        [EffectHandler(EffectsEnum.Eff_DamagePercentNeutral)]
        public static void DamagePercentNeutral(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            //foreach (var target in affecteds)
            //{
            //    short num = (short)((double)fighter.FighterStats.RealStats.LifePoints * ((double)effect.BaseEffect.DiceNum / 100.0));
            //    target.TakeDamages(new TakenDamages(num, ElementType.Neutral), fighter.ContextualId);
            //}

        }

        /// <summary>
        /// % de pv érodé (feu) => épée du destin, iop
        /// </summary>
        [EffectHandler(EffectsEnum.Eff_1094)]
        public static void DamageErodedFire(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castCellId)
        {

            foreach (var target in affecteds)
            {
                int num = target.FighterStats.ErodedLife.Percentage(effect.BaseEffect.DiceNum);
                target.TakeDamages(new TakenDamages((short)num, ElementType.Fire), fighter.ContextualId);
            }
        }
        /// <summary>
        /// Duel Iop
        /// </summary>
        [EffectHandler(EffectsEnum.Eff_1093)]
        public static void DamageErodedAir(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castCellId)
        {
            foreach (var target in affecteds)
            {
                int num = target.FighterStats.ErodedLife.Percentage(effect.BaseEffect.DiceNum);
                target.TakeDamages(new TakenDamages((short)num, ElementType.Air), fighter.ContextualId);
            }
        }
        /// <summary>
        /// Pulsar Roublard
        /// </summary>
        [EffectHandler(EffectsEnum.Eff_1015)]
        public static void FireDamageFunctionMP(Fighter fighter,SpellLevelRecord level,ExtendedSpellEffect effect,List<Fighter> affecteds,short castCellId)
        {
            var jet = fighter.CalculateJet(effect, fighter.FighterStats.Stats.Intelligence);
            jet = (short)((double)jet * (double)fighter.FighterStats.MPPercentage / (double)100);
            affecteds.ForEach(x => x.TakeDamages(new TakenDamages(jet, ElementType.Fire), fighter.ContextualId));
        }
    }
}
