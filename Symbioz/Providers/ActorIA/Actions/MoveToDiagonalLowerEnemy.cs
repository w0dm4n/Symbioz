using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.PathProvider;
using Symbioz.PathProvider;

namespace Symbioz.Providers.ActorIA.Actions
{
    [IAAction(IAActionsEnum.MoveToDiagonalEnemy)]
    class MoveToDiagonalLowerEnemy : AbstractIAAction
    {
        public override void Execute(MonsterFighter fighter)
        {
            try
            {
                if (fighter.FighterStats.Stats.MovementPoints <= 0)
                    return;
                // search fighter lower by pm
                Fighter lower = fighter.GetOposedTeam().LowerProchFighter(fighter, fighter.FighterStats.Stats.MovementPoints);

                //si c'est un summon que que c'est notre fin de tour ont regarde plus loin un fighter non summon
                if (fighter.FighterStats.Stats.ActionPoints <= 2 && lower is MonsterFighter && (lower as MonsterFighter).isSummon)
                    lower = fighter.GetOposedTeam().LowerProchFighter(fighter, 6);

                var path = new Pathfinder(fighter.Fight.Map, fighter.CellId);
                path.PutEntities(fighter.Fight.GetAllFighters());
                var cells = path.FindPathProche(lower.CellId, fighter.FighterStats.Stats.MovementPoints, fighter.CellId);
                if (cells == null || cells.Count() <= 1)
                    return;
                List<short> nearcells = PathHelper.Getalldirection(fighter.Fight, lower.CellId);
                List<short> neardiagcells = PathHelper.GetDiagonalcells(fighter.Fight, lower.CellId);

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
