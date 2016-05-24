// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Tips : ID2OClass
    {
        [Cache]
        public static List<Tips> Tipss = new List<Tips>();
        public Int32 id;
        public Int32 descId;
        public Tips(Int32 id, Int32 descId)
        {
            this.id = id;
            this.descId = descId;
        }
    }
}
