using Symbioz.Enums;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Effects
{
    public class StatsEffects
    {
        [EffectHandler(EffectsEnum.Eff_AddWeaponDamagePercent)] // Maitrise d'arme
        public static void AddWeapnDamageBonusPercen(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affected, short castcellid)
        {
            foreach (var target in affected)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("WeaponDamagesBonusPercent"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddAgility)]
        public static void AddAgility(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("ContextAgility"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }

        }
        [EffectHandler(EffectsEnum.Eff_AddIntelligence)]
        public static void AddIntelligence(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("ContextIntelligence"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddChance)]
        public static void AddChance(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("ContextChance"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddStrength)]
        public static void AddStrength(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("ContextStrength"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddCriticalHit)]
        public static void AddCriticalHit(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("CriticalHit"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }

        }
        /// <summary>
        /// Epée du destin de iop
        /// </summary>
        [EffectHandler(EffectsEnum.Eff_SubCriticalHit)]
        public static void SubCriticalHit(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castspellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("CriticalHit"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId,(short)(-effect.BaseEffect.DiceNum), effect.BaseEffect.Delay));
            }
        }
        /// <summary>
        /// Fleche Magique de Cra
        /// </summary>
        /// <param name="fighter"></param>
        [EffectHandler(EffectsEnum.Eff_StealRange)]
        public static void StealRange(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castspellid)
        {
           
           
        }
        [EffectHandler(EffectsEnum.Eff_SubRange)]
        public static void SubRange(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castspellid)
        {
          
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("_Range"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, (short)(-effect.BaseEffect.DiceNum), effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_SubDamageBonus)]
        public static void SubDamageBonus(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castspellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("AllDamagesBonus"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, (short)(-effect.BaseEffect.DiceNum), effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_SubDodge)]
        public static void SubDodge(Fighter fighter,SpellLevelRecord level,ExtendedSpellEffect effect,List<Fighter> affecteds, short castspellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("DodgePM"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, (short)(-effect.BaseEffect.DiceNum), effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddErosion)]
        public static void AddErosion(Fighter fighter,SpellLevelRecord level,ExtendedSpellEffect effect,List<Fighter> affecteds,short castSpellId)
        {
            foreach (var target in affecteds)
            {
                ErosionBuff buff = new ErosionBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_StealIntelligence)]
        public static void StealIntelligence(Fighter fighter,SpellLevelRecord level,ExtendedSpellEffect effect,List<Fighter> affecteds,short castSpellId)
        {
            foreach (var target in affecteds)
            {
                
            }
        }
    }
}
