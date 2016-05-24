using Symbioz.Enums;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Effects
{
    class HealEffects
    {
        [EffectHandler(EffectsEnum.Eff_GiveHPPercent)]
        public static void GiveHPPercent(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castspellid)
        {
            short num = (short)((double)fighter.FighterStats.RealStats.LifePoints * ((double)record.BaseEffect.DiceNum / 100.0));
            if (fighter.FighterStats.Stats.LifePoints == 1)
                return;
            if (num > fighter.FighterStats.Stats.LifePoints)
            {
                num = (short)(fighter.FighterStats.Stats.LifePoints - 1);
            }
            else
            {
                fighter.TakeDamages(new TakenDamages(num,ElementType.Neutral), fighter.ContextualId);
                foreach (var target in affecteds)
                {
                    target.Heal(num, fighter.ContextualId);
                }
            }
        }
        [EffectHandler(EffectsEnum.Eff_RestoreHPPercent)]
        public static void RestoreHpPercent(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castspellid)
        {
        
            if (effect.BaseEffect.Duration == 0)
            {
                foreach (var target in affecteds)
                {
                    short num = (short)((double)target.FighterStats.RealStats.LifePoints * ((double)effect.BaseEffect.DiceNum / 100.0));
                    target.Heal(num, fighter.ContextualId);
                }
            }
            else
            {
                foreach (var target in affecteds)
                {
                    HealPercentBuff buff = new HealPercentBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                    target.AddBuff(buff);
                }
            }
        }
        [EffectHandler(EffectsEnum.Eff_HealHP_108)]
        public static void HealHp(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castspellid)
        {
            foreach (var target in affecteds)
            {
                var jet = fighter.CalculateJet(record, fighter.FighterStats.Stats.Intelligence);
                target.Heal(jet, fighter.ContextualId);

            }
        }
    }
}
