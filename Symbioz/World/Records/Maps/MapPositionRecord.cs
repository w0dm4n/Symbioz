using Symbioz.ORM;
using Symbioz.World.PathProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Maps
{
    [Table("MapsPositions")]
    public class MapPositionRecord : ITable
    {
        public static List<MapPositionRecord> MapsPositions = new List<MapPositionRecord>();

        public int MapId;
        public int X;
        public int Y;
        public bool Outdoor;
        public int Capabilities;
        public string Name;

        public MapPositionRecord(int mapId,int x,int y,bool outdoor,int capabilities,string name)
        {
            this.MapId = mapId;
            this.X = x;
            this.Y = y;
            this.Outdoor = outdoor;
            this.Capabilities = capabilities;
            this.Name = name;
        }

        public static Point GetMapPosition(int mapId)
        {
           var position= MapsPositions.Find(x => x.MapId == mapId);
           return new Point(position.X, position.Y);
        }
        public static string GetMapName(int mapId)
        {
            return MapsPositions.Find(x => x.MapId == mapId).Name;
        }
    }
}
