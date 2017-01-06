using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records;
using Symbioz.World.Records.Spells;

namespace Symbioz.Providers.ActorIA.Actions
{
    /// <summary>
    /// Author GyGy19
    /// </summary>
    [IAAction(IAActionsEnum.CastOnLineAction)]
    class CastOnLineAction : AbstractIAAction
    {
        public static void TryCast(MonsterFighter fighter, ushort spellid, Fighter target)
        {
            var level = fighter.GetSpellLevel(spellid);

            if (target != null && fighter.FighterStats.Stats.ActionPoints - level.ApCost >= 0)
            {
                var refreshedTarget = fighter.Fight.GetFighter(target.CellId);
                if (refreshedTarget != null && !fighter.HaveCooldown((short)spellid) && fighter.CanCast(target.CellId, level, refreshedTarget))
                    fighter.CastSpellOnCell(spellid, target.CellId);
            }
        }

        public override void Execute(MonsterFighter fighter)
        {
            var spells = fighter.Template.Spells.ConvertAll<SpellRecord>(x => SpellRecord.GetSpell(x));
            int po = 2;
            // Select Max vision Spell
            foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Damages))
            {
                foreach (int s in spell.SpellLevels)
                {
                    SpellLevelRecord record = SpellLevelRecord.GetLevel(s);
                    if (record == null)
                        continue;
                    if (po < record.MaxRange)
                        po = record.MaxRange;
                }
            }

            Fighter lower = fighter.GetOposedTeam().LowerProchAlignFighter(fighter, po);
            if (lower == null)
            {
                lower = fighter.GetOposedTeam().LowerProchFighter(fighter, po);
                if (lower == null)
                return;
            }


            foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Damages))
            {
                TryCast(fighter, spell.Id, lower);
            }
            foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Agress))
            {
                TryCast(fighter, spell.Id, lower);
            }
            foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Undefined))
            {
                TryCast(fighter, spell.Id, lower);
            }
        }
    }
}
