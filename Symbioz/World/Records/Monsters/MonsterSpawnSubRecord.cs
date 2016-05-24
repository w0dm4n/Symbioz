using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Monsters
{
    [Table("MonstersSpawnsSubs")]
    public class MonsterSpawnSubRecord : ITable
    {
        public static List<MonsterSpawnSubRecord> MonstersSpawnsSub = new List<MonsterSpawnSubRecord>();

        public int Id;
        public ushort MonsterId;
        public ushort SubAreaId;
        public sbyte Probability;

        public MonsterSpawnSubRecord(int id, ushort monsterid, ushort subareaid, sbyte probability)
        {
            this.Id = id;
            this.MonsterId = monsterid;
            this.SubAreaId = subareaid;
            this.Probability = probability;
        }

        public static List<MonsterSpawnMapRecord> GetSpawns(int mapid)
        {
            var spawn = MonsterSpawnMapRecord.GetSpawn(mapid);
            if (spawn.Count() > 0)
            {
                return spawn;
            }
            else
            {
                var subId = MapRecord.GetSubAreaId(mapid);
                var sub = MonstersSpawnsSub.FindAll(x => x.SubAreaId ==subId);
                return sub.ConvertAll<MonsterSpawnMapRecord>(x => new MonsterSpawnMapRecord(0, x.MonsterId,mapid, x.Probability));
            }
        }
    }
}
