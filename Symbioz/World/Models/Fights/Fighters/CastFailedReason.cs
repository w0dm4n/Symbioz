using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    public enum CastFailedReason
    {
        RANGE,
        FIGHT_ENDED,
        FORBIDDEN_STATE,
        AP_COST,
        CAST_LIMIT,
        NOT_PLAYING,
    }
}
