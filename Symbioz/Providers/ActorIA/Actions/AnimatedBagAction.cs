using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA.Actions
{
    [IAAction(IAActionsEnum.ANIMATED_BAG)]
    class AnimatedBagAction : AbstractIAAction
    {
        public override void Execute(MonsterFighter fighter)
        {
            var martyrSpell = fighter.Template.Spells[0];
            Fighter lower = fighter.Team.LowerFighter();
            fighter.CastSpellOnTarget(martyrSpell, lower.ContextualId);
        }
    }
}
