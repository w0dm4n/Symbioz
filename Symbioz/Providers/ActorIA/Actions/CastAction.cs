using Symbioz.DofusProtocol.Messages;
using Symbioz.Helper;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.World.Records;

namespace Symbioz.Providers.ActorIA.Actions
{
    [IAAction(IAActionsEnum.AgressivSpellCast)]
    public class CastAction : AbstractIAAction
    {
        public static void TryCast(MonsterFighter fighter, ushort spellid,Fighter target)
        {
            var level = fighter.GetSpellLevel(spellid);
          
            if (target != null && fighter.FighterStats.Stats.ActionPoints - level.ApCost >= 0)
            {
                var refreshedTarget =  fighter.Fight.GetFighter(target.CellId);
                if (refreshedTarget != null && !fighter.HaveCooldown((short)spellid) && fighter.CanCast(target.CellId, level,refreshedTarget))
                    fighter.CastSpellOnCell(spellid,target.CellId);
            }
        }
        public override void Execute(MonsterFighter fighter)
        {
            Fighter lower = fighter.GetOposedTeam().LowerFighter();
            if (lower == null)
                return;
            var spells = fighter.Template.Spells.ConvertAll<SpellRecord>(x => SpellRecord.GetSpell(x));

            
            foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Damages))
            {
                TryCast(fighter, spell.Id,lower);
            }
            foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Agress))
            {
                TryCast(fighter, spell.Id,lower);
            }
            foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Undefined))
            {
                TryCast(fighter, spell.Id,lower);
            }

        }
    }
}
