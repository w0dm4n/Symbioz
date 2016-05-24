// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SpellType : ID2OClass
    {
        [Cache]
        public static List<SpellType> SpellTypes = new List<SpellType>();
        public Int32 id;
        public Int32 longNameId;
        public Int32 shortNameId;
        public SpellType(Int32 id, Int32 longNameId, Int32 shortNameId)
        {
            this.id = id;
            this.longNameId = longNameId;
            this.shortNameId = shortNameId;
        }
    }
}
