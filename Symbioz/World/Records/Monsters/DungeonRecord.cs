using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Monsters
{
    [Table("Dungeons")]
    public class DungeonRecord : ITable
    {
        public static List<DungeonRecord> Dungeons = new List<DungeonRecord>();

        [Primary]
        public int MapId;
        public string DungeonName;
        public int TeleportMapId;
        public short TeleportCellId;
        public List<ushort> MonsterGroup;

        public DungeonRecord(int mapid,string dungeonname,int teleportmapid,short teleportcellid,List<ushort> monstergroup)
        {
            this.MapId = mapid;
            this.DungeonName = dungeonname;
            this.TeleportMapId = teleportmapid;
            this.TeleportCellId = teleportcellid;
            this.MonsterGroup = monstergroup;
        }
        public static List<ushort> GetDungeonMapMonsters(int mapid)
        {
            return Dungeons.Find(x => x.MapId == mapid).MonsterGroup;
        }
        public static DungeonRecord GetDungeonData(int mapid)
        {
            return Dungeons.Find(x => x.MapId == mapid);
        }
        public static bool IsDungeonMap(int mapid)
        {
            return Dungeons.Find(x => x.MapId == mapid) != null;
        }
    }
}
