using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.PathProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA.Actions
{
  //  [IAAction(IAActionsEnum.MOVE_TO_ALLY)]
    class MoveToLowerAllyAction : AbstractIAAction
    {
        public override void Execute(MonsterFighter fighter)
        {
            if (fighter.FighterStats.Stats.MovementPoints <= 0)
                return;

            Fighter lower = fighter.Team.LowerFighter();
            var path = new Pathfinder(fighter.Fight.Map, fighter.CellId);
            path.PutEntities(fighter.Fight.GetAllFighters());
            var cells = path.FindPath(lower.CellId);
            if (cells == null || cells.Count() <= 1)
                return;
            cells.Remove(cells.Last());
            cells.Insert(0, fighter.CellId);
            cells = cells.Take(fighter.FighterStats.Stats.MovementPoints + 1).ToList();
            sbyte direction = PathParser.GetDirection(cells.Last());
            fighter.Move(cells, cells.Last(), direction);
        }
    }
}
