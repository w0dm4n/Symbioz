using Symbioz.Enums;
using Symbioz.Providers;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Messages;
using Symbioz.World.PathProvider;

namespace Symbioz.World.Models.Fights.Marks
{
    public class Glyph : MarkTrigger
    {

        public short Duration { get; set; }
        public void DecrementDuration()
        {
            this.Duration--;
            if (Duration <= 0)
                Caster.Fight.RemoveMarkTrigger(Caster, this);
        }
        public Glyph(Fighter caster, short centercell, char shapetype, short radius, short associatedspellid, sbyte spellgrade, int markcolor, short duration)
            : base(caster, centercell,centercell, shapetype, radius, associatedspellid, spellgrade, markcolor)
        {
            this.Duration = duration;
        }
        public override GameActionMarkTypeEnum MarkType
        {
            get { return GameActionMarkTypeEnum.GLYPH; }
        }

        [Interaction(FighterEventType.ON_TURN_STARTED)]
        public void OnTurnStarted(Fighter fighter, object arg1, object arg2, object arg3)
        {
            ActivateOnTurnStart(fighter);
        }

        public void ActivateOnTurnStart(Fighter fighter)
        {
           
            var spellLevel = SpellLevelRecord.GetLevel((ushort)AssociatedSpellId, AssociatedSpellGrade);
            DisplayTriggered(fighter);
            foreach (var effect in spellLevel.Effects)
            {
                Caster.Fight.TryStartSequence(Caster.ContextualId, 1);
                short[] cells = ShapesProvider.Handle(effect.ZoneShape, CenterCell, fighter.CellId, effect.ZoneSize).ToArray();
                SpellEffectsHandler.Handle(Caster, spellLevel, effect, new List<Fighter>() { fighter }, CenterCell);
                Caster.Fight.TryEndSequence(1, 0);
            }
            Caster.Fight.CheckFightEnd();
        }
        public void DisplayTriggered(Fighter source)
        {
            Caster.Fight.TryStartSequence(Caster.ContextualId, 1);
            Caster.Fight.Send(new GameActionFightTriggerGlyphTrapMessage((ushort)ActionsEnum.ACTION_FIGHT_TRIGGER_GLYPH, Caster.ContextualId, Id, source.ContextualId, (ushort)AssociatedSpellId));
            Caster.Fight.TryEndSequence(1, 0);
        }
        public void ActivateZone(Fighter fighter)
        {
          //  fighter.Fight.TryEndSequence(1, 0);
      //    fighter.Fight.TryStartSequence(1, 0);
            var spellLevel = SpellLevelRecord.GetLevel((ushort)AssociatedSpellId, AssociatedSpellGrade);
            DisplayTriggered(fighter);
            Caster.HandleSpellEffects(spellLevel, CenterCell, FightSpellCastCriticalEnum.NORMAL);
            Caster.Fight.CheckFightEnd();
        }
    }
}
