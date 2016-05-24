// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class MapReference : ID2OClass
    {
        [Cache]
        public static List<MapReference> MapReferences = new List<MapReference>();
        public Int32 id;
        public Int32 mapId;
        public Int32 cellId;
        public MapReference(Int32 id, Int32 mapId, Int32 cellId)
        {
            this.id = id;
            this.mapId = mapId;
            this.cellId = cellId;
        }
    }
}
