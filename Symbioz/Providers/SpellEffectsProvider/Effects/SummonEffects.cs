using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Effects
{
    class SummonEffects
    {
        [EffectHandler(EffectsEnum.Eff_405)] // Invocation Sadida
        public static void SadidaSummon(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castCellId)
        {
            if (affecteds.Count > 0)
            {
                MonsterFighter affected = affecteds.First() as MonsterFighter;

                if (affected != null && affected.FighterStats.SummonerId == fighter.ContextualId)
                {
                    MonsterFighter summoned = fighter.Fight.AddSummon(fighter, effect.BaseEffect.DiceNum, fighter.GetSpellLevel(level.SpellId).Grade, castCellId, fighter.Team);
                    summoned.AddBuff(new OnSadidaSummonDieEffect((uint)fighter.BuffIdProvider.Pop(), 0, -1, 0, 0, level.Grade, 0));
                    affected.Die();
                }
            }
        }
        [EffectHandler(EffectsEnum.Eff_Summon)]
        public static void Summon(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castCellid)
        {
            fighter.Fight.AddSummon(fighter, record.BaseEffect.DiceNum, fighter.GetSpellLevel(level.SpellId).Grade, castCellid, fighter.Team);
        }
        [EffectHandler(EffectsEnum.Eff_SpawnBomb)]
        public static void Bomb(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect record, List<Fighter> affecteds, short castCellid)
        {
            var target = fighter.Fight.GetFighter(castCellid);
            if (target == null)
            {
                fighter.Fight.SpawnBomb(fighter, record.BaseEffect.DiceNum, fighter.GetSpellLevel(level.SpellId).Grade, castCellid, fighter.Team);
            }
            else
               
            {
                MarksHelper.Instance.DirectExplosion(fighter, target, record.BaseEffect.DiceNum, level.Grade);
            
            }

        }
    }
}
