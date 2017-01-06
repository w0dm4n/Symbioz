using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.PathProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA.Actions
{
    [IAAction(IAActionsEnum.MoveToalignedEnemy)]
    class MoveToLowerAllignedEnemy : AbstractIAAction
    {
        public override void Execute(MonsterFighter fighter)
        {
            try
            {
                if (fighter.FighterStats.Stats.MovementPoints <= 0)
                    return;

                Fighter lower = fighter.Team.LowerProchAlignFighter(fighter, fighter.FighterStats.Stats.MovementPoints);
                var path = new Pathfinder(fighter.Fight.Map, fighter.CellId);
                path.PutEntities(fighter.Fight.GetAllFighters());
                var cells = path.FindPathProche(lower.CellId, fighter.FighterStats.Stats.MovementPoints, fighter.CellId);
                if (cells == null || cells.Count() <= 1)
                    return;
                cells.Remove(cells.Last());
                cells.Insert(0, fighter.CellId);
                cells = cells.Take(fighter.FighterStats.Stats.MovementPoints + 1).ToList();
                sbyte direction = PathParser.GetDirection(cells.Last());
                fighter.Move(cells, cells.Last(), direction);
            }
            catch (Exception error)
            {
                Logger.Error(error);
            }
        }
    }
}
