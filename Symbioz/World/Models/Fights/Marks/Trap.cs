using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Marks
{
    public class Trap : MarkTrigger
    {
        public Trap(Fighter caster, short centercell,char shapetype, short radius, short associatedspellid, sbyte spellgrade, int markcolor)
            : base(caster, centercell,centercell, shapetype, radius, associatedspellid, spellgrade, markcolor)
        {

        }
        public override GameActionMarkTypeEnum MarkType
        {
            get { return GameActionMarkTypeEnum.TRAP; }
        }

        [Interaction(FighterEventType.AFTER_MOVE)]
        public void AfterMove(Fighter source, object arg1, object arg2, object arg3)
        {
            List<short> path = (List<short>)arg2;
            foreach (var cell in path)
            {
                if (Cells.Contains(cell))
                {
                    Explode(source);
                    break;
                }
            }
        }
        public void Explode(Fighter fighter)
        {
            Caster.Fight.TryEndSequence(1, 0);
            Caster.Fight.TryStartSequence(fighter.ContextualId, 1);
            var spellLevel = SpellLevelRecord.GetLevel((ushort)AssociatedSpellId, AssociatedSpellGrade);
            Caster.Fight.RemoveMarkTrigger(fighter, this);
            Caster.HandleSpellEffects(spellLevel, CenterCell, FightSpellCastCriticalEnum.NORMAL);
            Caster.Fight.CheckFightEnd();
          
        }
    }
}
