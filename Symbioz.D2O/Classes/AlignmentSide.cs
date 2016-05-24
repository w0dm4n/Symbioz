// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AlignmentSide : ID2OClass
    {
        [Cache]
        public static List<AlignmentSide> AlignmentSides = new List<AlignmentSide>();
        public Int32 id;
        public Int32 nameId;
        public Boolean canConquest;
        public AlignmentSide(Int32 id, Int32 nameId, Boolean canConquest)
        {
            this.id = id;
            this.nameId = nameId;
            this.canConquest = canConquest;
        }
    }
}
