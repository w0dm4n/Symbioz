// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AlignmentGift : ID2OClass
    {
        [Cache]
        public static List<AlignmentGift> AlignmentGifts = new List<AlignmentGift>();
        public Int32 id;
        public Int32 nameId;
        public Int32 effectId;
        public Int32 gfxId;
        public AlignmentGift(Int32 id, Int32 nameId, Int32 effectId, Int32 gfxId)
        {
            this.id = id;
            this.nameId = nameId;
            this.effectId = effectId;
            this.gfxId = gfxId;
        }
    }
}
