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
    class ResistancesEffects
    {
        [EffectHandler(EffectsEnum.Eff_AddArmorDamageReduction)]
        public static void AddArmorDamagesReduction(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            AddGlobalDamageReduction(fighter, level, effect, affecteds, castcellid);
        }
        [EffectHandler(EffectsEnum.Eff_AddGlobalDamageReduction_105)]
        public static void AddGlobalDamageReduction(Fighter fighter,SpellLevelRecord level,ExtendedSpellEffect effect,List<Fighter> affecteds,short castcellid)
        {
            foreach (var target in affecteds)
            {
                var statdefinition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("GlobalDamageReduction"),target.FighterStats.Stats);
                StatBuff buff = new StatBuff((uint)target.BuffIdProvider.Pop(), statdefinition, (uint)effect.BaseEffect.EffectId, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddAirResistPercent)]
        public static void AirResistPercent(Fighter fighter,SpellLevelRecord level,ExtendedSpellEffect effect,List<Fighter> affecteds,short castcellid)
        {
            foreach (var target in affecteds)
            {
                var statdefinition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("AirResistPercent"), fighter.FighterStats.Stats);
                StatBuff buff = new StatBuff((uint)target.BuffIdProvider.Pop(), statdefinition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddFireResistPercent)]
        public static void FireResistPercent(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var statdefinition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("FireResistPercent"), fighter.FighterStats.Stats);
                StatBuff buff = new StatBuff((uint)target.BuffIdProvider.Pop(), statdefinition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddWaterResistPercent)]
        public static void WaterResistPercent(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var statdefinition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("WaterResistPercent"), fighter.FighterStats.Stats);
                StatBuff buff = new StatBuff((uint)target.BuffIdProvider.Pop(), statdefinition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddEarthResistPercent)]
        public static void EarthResistPercent(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var statdefinition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("EarthResistPercent"), fighter.FighterStats.Stats);
                StatBuff buff = new StatBuff((uint)target.BuffIdProvider.Pop(), statdefinition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
    }
}
