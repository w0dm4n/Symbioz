// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class StealthBones : ID2OClass
    {
        [Cache]
        public static List<StealthBones> StealthBoness = new List<StealthBones>();
        public Int32 id;
        public StealthBones(Int32 id)
        {
            this.id = id;
        }
    }
}
