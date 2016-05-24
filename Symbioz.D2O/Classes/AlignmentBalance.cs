// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AlignmentBalance : ID2OClass
    {
        [Cache]
        public static List<AlignmentBalance> AlignmentBalances = new List<AlignmentBalance>();
        public Int32 id;
        public Int32 startValue;
        public Int32 endValue;
        public Int32 nameId;
        public Int32 descriptionId;
        public AlignmentBalance(Int32 id, Int32 startValue, Int32 endValue, Int32 nameId, Int32 descriptionId)
        {
            this.id = id;
            this.startValue = startValue;
            this.endValue = endValue;
            this.nameId = nameId;
            this.descriptionId = descriptionId;
        }
    }
}
