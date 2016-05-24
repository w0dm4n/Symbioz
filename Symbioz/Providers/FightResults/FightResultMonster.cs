using Symbioz.DofusProtocol.Types;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.FightResults
{
    public class FightResultMonster : FightResult
    {
        public FightResultMonster(MonsterFighter fighter, TeamColorEnum winner)
            : base(fighter, winner)
        {

        }
        public override FightResultListEntry GetEntry()
        {
            return new FightResultFighterListEntry((ushort)OutCome, 0, new FightLoot(new List<ushort>(), 0), Fighter.ContextualId, !Fighter.Dead);
        }
    }
}
