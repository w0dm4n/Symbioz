using Symbioz.Core.Startup;
using Symbioz.Helper;
using Symbioz.ORM;
using Symbioz.World.Models;
using Symbioz.World.Models.Maps;
using Symbioz.World.PathProvider;
using Symbioz.World.Records.Maps;
using Symbioz.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Maps", true)]
    public class MapRecord : ITable
    {
        public static List<MapRecord> Maps = new List<MapRecord>();
        [Ignore]
        public MapInstance Instance;
        [Primary]
        public int Id;
        public int SubAreaId;
        public int TopMap;
        public int DownMap;
        public int LeftMap;
        public int RightMap;
        public List<short> WalkableCells;
        public List<short> BlueCells;
        public List<short> RedCells;
        public bool DugeonMap { get { return DungeonRecord.IsDungeonMap(this.Id); } }

        public bool HaveZaap { get { return Zaap != null; } }
        public InteractiveRecord Zaap { get { return InteractiveRecord.GetInteractivesOnMap(Id).Find(x => x.ElementTypeId == 16);
        } }
        public Point Position { get { return MapPositionRecord.GetMapPosition(Id); } }

        public string Name { get { return MapPositionRecord.GetMapName(Id); } }

        public MapRecord(int id, int subareaid, int topmap, int downmap, int leftmap, int rightmap, List<short> walkable, List<short> bluecells, List<short> redcells)
        {
            this.Id = id;
            this.SubAreaId = subareaid;
            this.TopMap = topmap;
            this.DownMap = downmap;
            this.LeftMap = leftmap;
            this.RightMap = rightmap;
            this.WalkableCells = walkable;
            this.BlueCells = bluecells;
            this.RedCells = redcells;
        }
        public bool Walkable(short cellid)
        {
            return WalkableCells.Contains(cellid);
        }
        public short RandomWalkableCell()
        {
            return WalkableCells.Random<short>();
        }
        public short CloseCell(short cellid)
        {
            var cells = ShapesProvider.GetSquare(cellid, false);
            foreach (var cell in cells)
            {
                if (WalkableCells.Contains(cell))
                    return (short)cell;
            }
            return 0;
        }
        public static ushort GetSubAreaId(int mapid)
        {
            var map = GetMap(mapid);
            if (map != null)
                return (ushort)map.SubAreaId;
            else
                return 0;
        }
        public bool IsValid()
        {
            if (WalkableCells.Count() == 0)
                return false;
            if (RedCells.Count() == 0)
                return false;
            if (BlueCells.Count() == 0)
                return false;
            return true;
        }
        public static List<int> GetSubAreaMaps(int subareaid)
        {
            return Maps.FindAll(x => x.SubAreaId == subareaid).ConvertAll<int>(x => x.Id);
        }
        public static MapRecord GetMap(int id)
        {
            return Maps.Find(x => x.Id == id);
        }
        public static List<MapRecord> GetMapWithoutPlacementCells()
        {
            return Maps.FindAll(x => x.BlueCells.Count == 0 || x.RedCells.Count == 0);
        }
        [StartupInvoke("Map Instances", StartupInvokeType.Others)]
        public static void CreateInstances()
        {
            foreach (var record in Maps)
            {
                record.Instance = new MapInstance(record);
            }
        }
    }
}
