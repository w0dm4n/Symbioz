// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Mount : ID2OClass
    {
        [Cache]
        public static List<Mount> Mounts = new List<Mount>();
        public Int32 id;
        public Int32 nameId;
        public String look;
        public Mount(Int32 id, Int32 nameId, String look)
        {
            this.id = id;
            this.nameId = nameId;
            this.look = look;
        }
    }
}
