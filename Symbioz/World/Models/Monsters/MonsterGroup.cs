using Symbioz.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Records;
using Symbioz.Helper;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights;

namespace Symbioz.World.Models
{
    public class MonsterGroup
    {
        public static int STARS_BONUS_INTERVAL = 300;
        public static short STARS_BONUS_INCREMENTATION = 15;
        public static short STARS_BONUS_LIMIT = 100;
        public const int START_ID = 100000000;

        public MonsterGroup(int monstergroupid,List<MonsterSpawnMapRecord> monsters,ushort cellid)
        {
            this.MonsterGroupId = monstergroupid;
            this.Monsters = monsters;
            this.CellId = cellid;
            this.CreationDate = DateTime.Now;
        }
        public ushort CellId { get; set; }
        public int MonsterGroupId { get; set; }
        public DateTime CreationDate { get; set; }
        public short AgeBonus
        {
            get
            {
                double num = (System.DateTime.Now - this.CreationDate).TotalSeconds / ((double)MonsterGroup.STARS_BONUS_INTERVAL / (double)MonsterGroup.STARS_BONUS_INCREMENTATION);
                if (num > (double)MonsterGroup.STARS_BONUS_LIMIT)
                {
                    num = (double)MonsterGroup.STARS_BONUS_LIMIT;
                }
                return (short)num;
              
              
            }
        }
        public List<MonsterSpawnMapRecord> Monsters = new List<MonsterSpawnMapRecord>();


     
        public List<MonsterFighter> CreateFighters(FightTeam team)
        {
            List<MonsterFighter> result = new List<MonsterFighter>();
            foreach (var monster in Monsters)
            {
                result.Add(new MonsterFighter(monster,team));
            }
            return result;
        }
        public static GameRolePlayGroupMonsterInformations GetActorInformations(MapRecord map, MonsterGroup group)
        {
            var random = new AsyncRandom();
            var firstMonster = group.Monsters[0];
            var firstMonsterTemplate = MonsterRecord.GetMonster(firstMonster.MonsterId);
            var light = new MonsterInGroupLightInformations(firstMonsterTemplate.Id, (sbyte)random.Next(1, 6));
            List<MonsterInGroupInformations> monstersinGroup = new List<MonsterInGroupInformations>();
            for (int i = 1; i < group.Monsters.Count; i++)
            {
                var mob = group.Monsters[i];
                var template = MonsterRecord.GetMonster(mob.MonsterId);
                monstersinGroup.Add(new MonsterInGroupInformations(mob.MonsterId, mob.ActualGrade, template.RealLook.ToEntityLook()));
            }
            var staticInfos = new GroupMonsterStaticInformations(light, monstersinGroup);

            return new GameRolePlayGroupMonsterInformations(group.MonsterGroupId, firstMonsterTemplate.RealLook.ToEntityLook(),
                new EntityDispositionInformations((short)group.CellId, 3), false, false, false, staticInfos,group.AgeBonus, 0, 0);
        }
        public static List<GameRolePlayGroupMonsterInformations> GetActorsInformations(MapRecord map,List<MonsterGroup> groups)
        {
           
            List<GameRolePlayGroupMonsterInformations> result = new List<GameRolePlayGroupMonsterInformations>();
            foreach (var group in groups)
            {
                result.Add(GetActorInformations(map, group));
            }
            return result;
        }
    }
}
