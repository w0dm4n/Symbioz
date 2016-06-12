using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Alliances.Prisms
{
    public enum CanAddPrismOnSubAreaResult
    {
        SUBAREA_ERROR = 0,
        SUBAREA_ALREADY_TAKEN = 1,
        SUBAREA_NOT_CONQUERABLE = 2,
        SUBAREA_AVAILABLE = 3,
    }
}
