// automatic generation Symbioz.Sync 2015

using Symbioz.D2O.InternalClasses;
using Symbioz.DofusProtocol.D2O;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Area : ID2OClass
    {
        [Cache]
        public static List<Area> Areas = new List<Area>();
        public Int32 id;
        public Int32 nameId;
        public Int32 superAreaId;
        public Boolean containHouses;
        public Boolean containPaddocks;
        public Rectangle bounds;
        public Int32 worldmapId;
        public Boolean hasWorldMap;
        public Area(Int32 id, Int32 nameId, Int32 superAreaId, Boolean containHouses, Boolean containPaddocks,Rectangle bounds, Int32 worldmapId, Boolean hasWorldMap)
        {
            this.id = id;
            this.nameId = nameId;
            this.superAreaId = superAreaId;
            this.containHouses = containHouses;
            this.containPaddocks = containPaddocks;
            this.bounds = bounds;
            this.worldmapId = worldmapId;
            this.hasWorldMap = hasWorldMap;
        }
    }
}
