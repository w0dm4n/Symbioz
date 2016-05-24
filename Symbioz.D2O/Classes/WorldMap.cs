// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class WorldMap : ID2OClass
    {
        [Cache]
        public static List<WorldMap> WorldMaps = new List<WorldMap>();
        public Int32 id;
        public Int32 nameId;
        public Int32 origineX;
        public Int32 origineY;
        public Double mapWidth;
        public Double mapHeight;
        public Int32 horizontalChunck;
        public Int32 verticalChunck;
        public Boolean viewableEverywhere;
        public Double minScale;
        public Double maxScale;
        public Double startScale;
        public Int32 centerX;
        public Int32 centerY;
        public Int32 totalWidth;
        public Int32 totalHeight;
        public String[] zoom;
        public WorldMap(Int32 id, Int32 nameId, Int32 origineX, Int32 origineY, Double mapWidth, Double mapHeight, Int32 horizontalChunck, Int32 verticalChunck, Boolean viewableEverywhere, Double minScale, Double maxScale, Double startScale, Int32 centerX, Int32 centerY, Int32 totalWidth, Int32 totalHeight, String[] zoom)
        {
            this.id = id;
            this.nameId = nameId;
            this.origineX = origineX;
            this.origineY = origineY;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.horizontalChunck = horizontalChunck;
            this.verticalChunck = verticalChunck;
            this.viewableEverywhere = viewableEverywhere;
            this.minScale = minScale;
            this.maxScale = maxScale;
            this.startScale = startScale;
            this.centerX = centerX;
            this.centerY = centerY;
            this.totalWidth = totalWidth;
            this.totalHeight = totalHeight;
            this.zoom = zoom;
        }
    }
}
