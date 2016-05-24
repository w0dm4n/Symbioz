using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Helper;
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
    class BuffEffects
    {
        /// <summary>
        /// Sacrifice du sacrieur
        /// </summary>
        [EffectHandler(EffectsEnum.Eff_765)]
        public static void Martyr(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                MartyrBuff buff = new MartyrBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        /// <summary>
        /// Friction de Iop
        /// </summary>
        [EffectHandler(EffectsEnum.Eff_792)]
        public static void SetFrictionBuff(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                FrictionBuff buff = new FrictionBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddVitalityPercent)]
        public static void AddVitalityPercent(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affected, short castcellid)
        {
            short num = (short)((double)fighter.FighterStats.RealStats.LifePoints * ((double)effect.BaseEffect.DiceNum / 100.0));
            foreach (var target in affected)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("LifePoints"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)ActionsEnum.ACTION_CHARACTER_BOOST_VITALITY, num, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, num, effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddVitality)]
        public static void AddVitality(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affected, short castcellid)
        {
            foreach (var target in affected)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("LifePoints"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)ActionsEnum.ACTION_CHARACTER_BOOST_VITALITY, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_Dodge)]
        public static void DodgeBuffEffect(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affected, short castcellid)
        {
            foreach (var target in affected)
            {
                DodgeBuff buff = new DodgeBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceSide, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_IncreaseDamage_138)]
        public static void IncreaseDamage(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affected, short castcellid)
        {
            foreach (var target in affected)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("AllDamagesBonusPercent"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_IncreaseDamage_1054)]
        public static void IncreaseDamage1054(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affected, short castcellid)
        {
           
            IncreaseDamage(fighter, level, effect, affected, castcellid);
        }
        [EffectHandler(EffectsEnum.Eff_AddDamageBonus)]
        public static void AddDamageBonus(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("AllDamagesBonus"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_1048)]
        public static void RemoveLifePercent(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            // transe, zobal actions enum dans le displayable ^^

        }
        [EffectHandler(EffectsEnum.Eff_AddShieldPercent)]
        public static void AddShieldPercent(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            short num = (short)((double)fighter.FighterStats.RealStats.LifePoints * ((double)effect.BaseEffect.DiceNum / 100.0));
            foreach (var target in affecteds)
            {
                ShieldBuff buff = new ShieldBuff((uint)target.BuffIdProvider.Pop(), num, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddShield)]
        public static void AddShield(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                ShieldBuff buff = new ShieldBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddState)]
        public static void AddState(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castercellid)
        {
            foreach (var target in affecteds)
            {
                StateBuff buff = new StateBuff((uint)target.BuffIdProvider.Pop(), (short)effect.BaseEffect.Value, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_AddRange)]
        public static void AddRange(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var definition = new UInt16ReflectedStat(StatsRecord.GetFieldInfo("_Range"), target.FighterStats.Stats);
                target.AddBuff(new StatBuff((uint)target.BuffIdProvider.Pop(), definition, (uint)effect.BaseEffect.EffectType, effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, effect.BaseEffect.Delay));
            }
        }
        [EffectHandler(EffectsEnum.Eff_ChangeAppearance)]
        public static void ChangeAppearence(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castcellid)
        {
            ExtendedChangeAppearence(fighter, level, record, affecteds, castcellid);
        }
        [EffectHandler(EffectsEnum.Eff_ChangeAppearance_335)]
        public static void ExtendedChangeAppearence(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                LookBuff buff = new LookBuff((uint)target.BuffIdProvider.Pop(), 0, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_Punishment)]
        public static void PunishmentEffect(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castspellid)
        {
            foreach (var target in affecteds)
            {
                PunishementBuff buff = new PunishementBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceSide, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.DiceNum, (short)effect.BaseEffect.Value, 0);
                target.AddBuff(buff);
            }
        }
        // Poutch
        [EffectHandler(EffectsEnum.Eff_Rewind)]
        public static void RewindEffect(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castspellid)
        {
            foreach (var target in affecteds)
            {
                RewindBuff buff = new RewindBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_SpellBoost)]
        public static void AddSpellBoost(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                SpellBoostBuff buff = new SpellBoostBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay, (short)effect.BaseEffect.Value);
                target.AddBuff(buff);
            }
        }
      


    }
}
