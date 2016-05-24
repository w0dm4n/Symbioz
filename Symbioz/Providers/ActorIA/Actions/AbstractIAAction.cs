using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA
{
    public abstract class AbstractIAAction
    {
        public abstract void Execute(MonsterFighter fighter);
    }
}
