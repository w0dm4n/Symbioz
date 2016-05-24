// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AlignmentEffect : ID2OClass
    {
        [Cache]
        public static List<AlignmentEffect> AlignmentEffects = new List<AlignmentEffect>();
        public Int32 id;
        public Int32 characteristicId;
        public Int32 descriptionId;
        public AlignmentEffect(Int32 id, Int32 characteristicId, Int32 descriptionId)
        {
            this.id = id;
            this.characteristicId = characteristicId;
            this.descriptionId = descriptionId;
        }
    }
}
