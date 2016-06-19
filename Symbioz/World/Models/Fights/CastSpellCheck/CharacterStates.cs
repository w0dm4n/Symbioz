using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.CastSpellCheck
{
    class CharacterStates
    {
        public static bool CharacterSpellStatesChecker(Fighter Caster, List<Fighter> affecteds, SpellLevelRecord Record, ExtendedSpellEffect Effect)
        {
            List<CharacterFighter> p = new List<CharacterFighter>();
            
            foreach (var affected in affecteds)
            {
                if (affected is CharacterFighter)
                {
                    p.Add(affected as CharacterFighter);
                }
            }
            if (p.Count <= 0)
            {
                if (Record != null)
                    processCharacterCheck(Caster, null, Record, Effect);
                else
                    processWeaponCheck(Caster, null, Effect);
                return (true);
            }
            foreach (CharacterFighter player in p)
            {
                if (Record != null)
                    processCharacterCheck(Caster, player, Record, Effect);
                else
                    processWeaponCheck(Caster, player, Effect);
            }
            return (true);
        }

        public static void castSpellFighternotVisible(Fighter Caster, CharacterFighter affected, ExtendedSpellEffect Effect, bool iscac)
        {
            if (iscac)
                Caster.setvisible(true, Effect.BaseEffect.EffectId);
            else
            {
                switch (Effect.BaseEffect.EffectType)//Dommage direct
                {
                    case EffectsEnum.Eff_DamagePercentWater:
                    case EffectsEnum.Eff_DamagePercentEarth:
		            case EffectsEnum.Eff_DamagePercentAir:
		            case EffectsEnum.Eff_DamagePercentFire:
		            case EffectsEnum.Eff_DamagePercentNeutral:
                    case EffectsEnum.Eff_DamageWater:
                    case EffectsEnum.Eff_DamageEarth:
                    case EffectsEnum.Eff_DamageAir:
                    case EffectsEnum.Eff_DamageFire:
                    case EffectsEnum.Eff_DamageNeutral:
                        Caster.setvisible(true, Effect.BaseEffect.EffectId);
                        break;
                }
            }
            Caster.Fight.Send(new ShowCellMessage(Caster.ContextualId, (ushort)Caster.CellId));
        }

        public static void processWeaponCheck(Fighter Caster, CharacterFighter affected, ExtendedSpellEffect Effect)
        {
            if (Caster.visible == false)
                castSpellFighternotVisible(Caster, affected, Effect, true);
        }

        public static void processCharacterCheck(Fighter Caster, CharacterFighter affected, SpellLevelRecord Record, ExtendedSpellEffect Effect)
        {
            if (Caster.visible == false)
                castSpellFighternotVisible(Caster, affected, Effect, false);
        }
    }
}
