// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AlignmentOrder : ID2OClass
    {
        [Cache]
        public static List<AlignmentOrder> AlignmentOrders = new List<AlignmentOrder>();
        public Int32 id;
        public Int32 nameId;
        public Int32 sideId;
        public AlignmentOrder(Int32 id, Int32 nameId, Int32 sideId)
        {
            this.id = id;
            this.nameId = nameId;
            this.sideId = sideId;
        }
    }
}
