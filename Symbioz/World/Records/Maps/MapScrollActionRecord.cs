using Symbioz.Helper;
using Symbioz.ORM;
using Symbioz.World.Models.Maps;
using Symbioz.World.PathProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Maps
{
    [Table("MapsScrollActions")]
    public class MapScrollActionRecord : ITable
    {
        public static List<MapScrollActionRecord> ScrollActions = new List<MapScrollActionRecord>();

        public int MapId;
        public int RightMapId;
        public int BottomMapId;
        public int LeftMapId;
        public int TopMapId;

        public MapScrollActionRecord(int mapid, int rightmapid, int bottommapid, int leftmapid, int topmapid)
        {
            this.MapId = mapid;
            this.RightMapId = rightmapid;
            this.BottomMapId = bottommapid;
            this.LeftMapId = leftmapid;
            this.TopMapId = topmapid;
        }
        public static sbyte GetScrollDirection(MapScrollType type)
        {
            switch (type)
            {
                case MapScrollType.TOP:
                    return 6;

                case MapScrollType.LEFT:
                    return 4;

                case MapScrollType.BOTTOM:
                    return 2;

                case MapScrollType.RIGHT:
                    return 0;

                default:
                    return 0;
            }
        }
        public static short SearchScrollCellId(short cellid, MapScrollType type, MapRecord map)
        {
            var defaultCell = GetScrollDefaultCellId(cellid, type);
            var cells = ShapesProvider.GetMapBorder(GetOposedTransition(type));
            var walkables = cells.FindAll(x => map.Walkable(x));
            return walkables.Count == 0 ? map.RandomWalkableCell() : walkables[new AsyncRandom().Next(0, walkables.Count - 1)];
        }
        public static MapScrollType GetOposedTransition(MapScrollType type)
        {
            switch (type)
            {
                case MapScrollType.TOP:
                    return MapScrollType.BOTTOM;
                case MapScrollType.LEFT:
                    return MapScrollType.RIGHT;
                case MapScrollType.BOTTOM:
                    return MapScrollType.TOP;
                case MapScrollType.RIGHT:
                    return MapScrollType.LEFT;
            }
            throw new Exception("What is that MapScrollType dude?");
        }
        public static short GetScrollDefaultCellId(short cellid, MapScrollType type)
        {
            switch (type)
            {
                case MapScrollType.TOP:
                    return (short)(cellid + 532);
                case MapScrollType.LEFT:
                    return (short)(cellid + 27);
                case MapScrollType.BOTTOM:
                    return (short)(cellid - 532); ;
                case MapScrollType.RIGHT:
                    return (short)(cellid - 27);
                default:
                    return 0;
            }
        }
        public static MapScrollType GetScrollTypeFromCell(short cellid)
        {
            if (ShapesProvider.GetMapBorder(MapScrollType.TOP).Contains(cellid))
                return MapScrollType.TOP;
            if (ShapesProvider.GetMapBorder(MapScrollType.BOTTOM).Contains(cellid))
                return MapScrollType.BOTTOM;
            if (ShapesProvider.GetMapBorder(MapScrollType.LEFT).Contains(cellid))
                return MapScrollType.LEFT;
            if (ShapesProvider.GetMapBorder(MapScrollType.RIGHT).Contains(cellid))
                return MapScrollType.RIGHT;
            return MapScrollType.UNDEFINED;
        }
        public static int GetOverrideScrollMapId(int mapid, MapScrollType type)
        {
            var scroll = ScrollActions.Find(x => x.MapId == mapid);
            if (scroll != null)
            {
                switch (type)
                {
                    case MapScrollType.TOP:
                        return scroll.TopMapId;
                    case MapScrollType.LEFT:
                        return scroll.LeftMapId;
                    case MapScrollType.BOTTOM:
                        return scroll.BottomMapId;
                    case MapScrollType.RIGHT:
                        return scroll.RightMapId;
                }
            }
            return -1;
        }

    }
}
