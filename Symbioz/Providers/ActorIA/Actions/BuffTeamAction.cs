using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA.Actions
{
    [IAAction(IAActionsEnum.BuffTeam)]
    class BuffTeamAction : AbstractIAAction
    {
        public override void Execute(World.Models.Fights.Fighters.MonsterFighter fighter)
        {
            var spells = fighter.Template.Spells.ConvertAll<SpellRecord>(x => SpellRecord.GetSpell(x));
            var target = fighter.Team.LowerFighter();

            if (fighter.GetOposedTeam().LowerFighter().FighterStats.LifePercentage <= 20 && spells.FindAll(x=>x.Category == SpellCategoryEnum.Damages).Count > 0)
                return;

           
         
            foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Heal))
            {
                CastAction.TryCast(fighter, spell.Id, target);
            }
            foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Buff))
            {
                CastAction.TryCast(fighter, spell.Id,target);
            }
        }
    }
}
