using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    public enum FighterEventType
    {
        ON_TURN_STARTED,
        ON_TURN_ENDED,
        ON_CASTED,
        BEFORE_ATTACKED,
        AFTER_ATTACKED,
        AFTER_USED_AP,
        AFTER_MOVE,
        ON_TELEPORTED,
        BEFORE_MOVE,
        BEFORE_DIED,
        AFTER_DIED,
    }
}
