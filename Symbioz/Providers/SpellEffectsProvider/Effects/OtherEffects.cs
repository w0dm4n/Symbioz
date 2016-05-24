using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.World.Models;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Effects
{
    class OtherEffects
    {
        [EffectHandler(EffectsEnum.Eff_1009)] // exploser une bombe
        public static void Detonate(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            var bomb = fighter.Fight.GetFighter(castcellid) as BombFighter;
            if (bomb != null)
            {
                bomb.Detonate();
            }

        }
        [EffectHandler(EffectsEnum.Eff_SpawnGlyphOnDied)]
        public static void SpawnGlyphOnDied(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {

        }
        [EffectHandler(EffectsEnum.Eff_MultiplyTakenDamages)] // dommages subis *X%
        public static void MultiplyTakenDamages(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                MultiplyTakenDamageBuff buff = new MultiplyTakenDamageBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_SwapDamages)] // Les dommages reçut soignent
        public static void SwapDamages(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                DamageSwapBuff buff = new DamageSwapBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }
        [EffectHandler(EffectsEnum.Eff_406)]
        public static void Debuff(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            affecteds.ForEach(x => x.DispellSpell((ushort)effect.BaseEffect.Value));
        }
        [EffectHandler(EffectsEnum.Eff_CarryEntity)]
        public static void CarryEntity(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {

        }
        [EffectHandler(EffectsEnum.Eff_ThrowEntity)]
        public static void ThrowEntity(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {

        }

        [EffectHandler(EffectsEnum.Eff_1045)]
        public static void AddSpellCooldown(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                if (target is CharacterFighter)
                {
                    var ctarget = target as CharacterFighter;
                    ctarget.Client.Send(new GameActionFightSpellCooldownVariationMessage((ushort)ActionsEnum.ACTION_CHARACTER_ADD_SPELL_COOLDOWN,
                        fighter.ContextualId, ctarget.ContextualId, (ushort)effect.BaseEffect.DiceNum, (short)effect.BaseEffect.Value));
                }
            }

        }
        [EffectHandler(EffectsEnum.Eff_RevealsInvisible)]
        public static void RevealsInvisible(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {

        }
        [EffectHandler(EffectsEnum.Eff_Invisibility)]
        public static void Invisibility(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                InvisibilityBuff buff = new InvisibilityBuff((uint)target.BuffIdProvider.Pop(), effect.BaseEffect.DiceNum, effect.BaseEffect.Duration, fighter.ContextualId, (short)level.SpellId, effect.BaseEffect.Delay);
                target.AddBuff(buff);
            }
        }

        [EffectHandler(EffectsEnum.Eff_ReduceEffsDuration)]
        public static void ReduceEffsDuration(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach(Fighter target in affecteds)
            {
                foreach(Buff buff in target.Buffs)
                {
                    short LastDuration = buff.Duration;
                    buff.setDuration(effect.BaseEffect.Value, fighter);
                    short ral = (short)((LastDuration - buff.Duration) * -1);
                    target.Fight.Send(new GameActionFightModifyEffectsDurationMessage((ushort)buff.UID, buff.SourceId, fighter.ContextualId, ral));
                }
            }
        }

    }
}