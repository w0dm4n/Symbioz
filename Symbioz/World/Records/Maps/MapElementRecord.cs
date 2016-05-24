using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("MapsElements",true)]
    public class MapElementRecord : ITable
    {
        public static List<MapElementRecord> MapsElements = new List<MapElementRecord>();

        [Primary]
        public int Id;
        public int ElementId;
        public int MapId;
        public ushort CellId;

        public MapElementRecord(int id,int elementid,int mapid,ushort cellid)
        {
            this.Id = id;
            this.ElementId = elementid;
            this.MapId = mapid;
            this.CellId = cellid;
        }

        public static MapElementRecord GetMapElement(int elementid)
        {
            return MapsElements.Find(x => x.ElementId == elementid);
        }
        public static List<MapElementRecord> GetMapElementByMap(int mapid)
        {
            return MapsElements.FindAll(x => x.MapId == mapid);
        }
    }
}
