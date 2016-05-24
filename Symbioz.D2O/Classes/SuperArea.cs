// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SuperArea : ID2OClass
    {
        [Cache]
        public static List<SuperArea> SuperAreas = new List<SuperArea>();
        public Int32 id;
        public Int32 nameId;
        public Int32 worldmapId;
        public Boolean hasWorldMap;
        public SuperArea(Int32 id, Int32 nameId, Int32 worldmapId, Boolean hasWorldMap)
        {
            this.id = id;
            this.nameId = nameId;
            this.worldmapId = worldmapId;
            this.hasWorldMap = hasWorldMap;
        }
    }
}
