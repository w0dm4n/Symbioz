using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Enums;
using Symbioz.World.Models.Fights;
using Symbioz.DofusProtocol.Types;

namespace Symbioz.Providers.FightResults
{
    public abstract class FightResult
    {
        public Fighter Fighter { get; set; }
        public FightOutcomeEnum OutCome { get; set; }
        public FightResult(Fighter fighter,TeamColorEnum winner)
        {
            this.Fighter = fighter;
            if (fighter.Team.TeamColor == winner)
                OutCome = FightOutcomeEnum.RESULT_VICTORY;
            else
                OutCome = FightOutcomeEnum.RESULT_LOST;
      
        }
        public abstract FightResultListEntry GetEntry();
       
    }
}
