// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AlignmentTitle : ID2OClass
    {
        [Cache]
        public static List<AlignmentTitle> AlignmentTitles = new List<AlignmentTitle>();
        public Int32 sideId;
        public Int32[] namesId;
        public Int32[] shortsId;
        public AlignmentTitle(Int32 sideId, Int32[] namesId, Int32[] shortsId)
        {
            this.sideId = sideId;
            this.namesId = namesId;
            this.shortsId = shortsId;
        }
    }
}
