using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Monsters
{
    public class MonsterSpawnManager
    {
        private static List<MonsterGroup> m_monsterGroupsSpawn = new List<MonsterGroup>();

        public static void AddMonsterSpawnGroup(MonsterGroup group)
        {
            if(!m_monsterGroupsSpawn.Contains(group))
            {
                m_monsterGroupsSpawn.Add(group);
            }
        }

        public static void RemoveMonsterSpawnGroup(MonsterGroup group)
        {
            if(m_monsterGroupsSpawn.Contains(group))
            {
                m_monsterGroupsSpawn.Remove(group);
            }
        }

        public static List<MonsterGroup> GetMonsterSpawnBySubArea(int subAreaId)
        {
            return m_monsterGroupsSpawn.Where(x => x.SubAreaId == subAreaId).ToList();
        }
    }
}
