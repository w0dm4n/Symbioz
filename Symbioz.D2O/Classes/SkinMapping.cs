// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SkinMapping : ID2OClass
    {
        [Cache]
        public static List<SkinMapping> SkinMappings = new List<SkinMapping>();
        public Int32 id;
        public Int32 lowDefId;
        public SkinMapping(Int32 id, Int32 lowDefId)
        {
            this.id = id;
            this.lowDefId = lowDefId;
        }
    }
}
