using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Monsters
{
    [Table("MapsNoSpawns")]
    public class MapNoSpawnRecord : ITable
    {
        public static List<MapNoSpawnRecord> MapsNoSpawns = new List<MapNoSpawnRecord>();

        public int MapId;
        public MapNoSpawnRecord(int mapid)
        {
            this.MapId = mapid;
        }
        public static bool CanSpawn(int mapid)
        {
            if (MapsNoSpawns.Find(x=>x.MapId == mapid) != null)
                return false;
            else
                return true;
        }
    }
}
