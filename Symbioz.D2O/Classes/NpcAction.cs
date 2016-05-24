// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class NpcAction : ID2OClass
    {
        [Cache]
        public static List<NpcAction> NpcActions = new List<NpcAction>();
        public Int32 id;
        public Int32 realId;
        public Int32 nameId;
        public NpcAction(Int32 id, Int32 realId, Int32 nameId)
        {
            this.id = id;
            this.realId = realId;
            this.nameId = nameId;
        }
    }
}
