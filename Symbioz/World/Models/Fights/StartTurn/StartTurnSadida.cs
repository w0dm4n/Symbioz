using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.StartTurn
{
    class StartTurnSadida
    {
        public static void EffectAllTreeSadida(Fighter Sadida)
        {
            MonsterFighter mob = null;
            foreach(Fighter f in Sadida.Team.GetFighters())
            {
               if (f is MonsterFighter && Sadida.GetAliveFighterSummons().Contains(f))
               {
                    mob = f as MonsterFighter;
                    if (mob.tree != null)
                    {
                        if (mob.tree.Master == null)
                            mob.tree.Master = Sadida;
                        mob.tree.floor();
                    }
                    else if (mob.sacrifier != null)
                    {
                        if (mob.sacrifier.Master == null)
                            mob.sacrifier.Master = Sadida;
                        mob.sacrifier.Increment();
                    }
               }
            }
        }
    }
}
