using Symbioz.ORM;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Monsters
{
    [Table("MonstersSpawnsMaps")]
    public class MonsterSpawnMapRecord : ITable
    {
        public static List<MonsterSpawnMapRecord> MonstersSpawnsMaps = new List<MonsterSpawnMapRecord>();
        public int Id;
        public ushort MonsterId;
        public int MapId;
        public sbyte Probability;
        [Ignore]
        public sbyte ActualGrade;
        public MonsterSpawnMapRecord(int id, ushort monsterid,int mapid, sbyte probability)
        {
            this.Id = id;
            this.MonsterId = monsterid;
            this.MapId = mapid;
            this.Probability = probability;
        }
        public MonsterFighter CreateFighter(FightTeam team)
        {
            return new MonsterFighter(this,team);
        }
        public static List<MonsterSpawnMapRecord> GetSpawn(int mapid)
        {
            return MonstersSpawnsMaps.FindAll(x => x.MapId == mapid);
        }
    }
}
