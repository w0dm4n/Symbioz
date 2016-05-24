// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AlignmentRank : ID2OClass
    {
        [Cache]
        public static List<AlignmentRank> AlignmentRanks = new List<AlignmentRank>();
        public Int32 id;
        public Int32 orderId;
        public Int32 nameId;
        public Int32 descriptionId;
        public Int32 minimumAlignment;
        public Int32 objectsStolen;
        public ArrayList gifts;
        public AlignmentRank(Int32 id, Int32 orderId, Int32 nameId, Int32 descriptionId, Int32 minimumAlignment, Int32 objectsStolen, ArrayList gifts)
        {
            this.id = id;
            this.orderId = orderId;
            this.nameId = nameId;
            this.descriptionId = descriptionId;
            this.minimumAlignment = minimumAlignment;
            this.objectsStolen = objectsStolen;
            this.gifts = gifts;
        }
    }
}
