// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class RideFood : ID2OClass
    {
        [Cache]
        public static List<RideFood> RideFoods = new List<RideFood>();
        public Int32 gid;
        public Int32 typeId;
        public RideFood(Int32 gid, Int32 typeId)
        {
            this.gid = gid;
            this.typeId = typeId;
        }
    }
}
