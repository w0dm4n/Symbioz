// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class CreatureBoneType : ID2OClass
    {
        [Cache]
        public static List<CreatureBoneType> CreatureBoneTypes = new List<CreatureBoneType>();
        public Int32 id;
        public Int32 creatureBoneId;
        public CreatureBoneType(Int32 id, Int32 creatureBoneId)
        {
            this.id = id;
            this.creatureBoneId = creatureBoneId;
        }
    }
}
