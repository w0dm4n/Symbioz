// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class MapCoordinates : ID2OClass
    {
        [Cache]
        public static List<MapCoordinates> MapCoordinatess = new List<MapCoordinates>();
        public Int32 compressedCoords;
        public Int32[] mapIds;
        public MapCoordinates(Int32 compressedCoords, Int32[] mapIds)
        {
            this.compressedCoords = compressedCoords;
            this.mapIds = mapIds;
        }
    }
}
