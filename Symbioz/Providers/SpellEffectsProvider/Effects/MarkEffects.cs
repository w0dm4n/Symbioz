using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Effects
{
    class MarkEffects
    {
        [EffectHandler(EffectsEnum.Eff_ActivateGlyph)]
        public static void ActivateGlyph(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            var glyphs = fighter.Fight.GetMarks<Glyph>(x => x.Cells.Contains(castcellid) && x.Caster == fighter);
            glyphs.ForEach(x => x.ActivateZone(fighter));
        }
        [EffectHandler(EffectsEnum.Eff_Glyph_402)]
        public static void SpawnGlyph402(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            SpawnGlyph(fighter, level, effect, affecteds, castcellid);
        }
        [EffectHandler(EffectsEnum.Eff_Glyph)]
        public static void SpawnGlyph(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            Glyph glyph = new Glyph(fighter, castcellid, effect.ZoneShape, effect.ZoneSize, effect.BaseEffect.DiceNum, level.Grade, effect.BaseEffect.Value,effect.BaseEffect.Duration);
            fighter.Fight.AddMarkTrigger(fighter, glyph);
        }
        [EffectHandler(EffectsEnum.Eff_Trap)]
        public static void SpawnTrap(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            Trap trap = new Trap(fighter, castcellid, effect.ZoneShape, effect.ZoneSize, effect.BaseEffect.DiceNum, level.Grade, effect.BaseEffect.Value);
            fighter.Fight.AddMarkTrigger(fighter, trap,fighter.Team);
        }
        [EffectHandler(EffectsEnum.Eff_Portal)]
        public static void SpawnPortal(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            Portal portal = new Portal(fighter, castcellid, effect.ZoneShape, effect.ZoneSize,
                effect.BaseEffect.DiceNum, level.Grade,Color.Red.ToArgb());
            fighter.Fight.AddMarkTrigger(fighter, portal);
        }
    }
}
