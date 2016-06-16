using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.CastSpellCheck
{
    class MonsterStates
    {
        public static bool MonsterSpellStatesChecker(Fighter Caster, List<Fighter> affecteds, SpellLevelRecord Record, ExtendedSpellEffect Effect)
        {
            List<MonsterFighter> mobs = new List<MonsterFighter>();

            Logger.Log("LUNCH SPELL CHECK");
            foreach (var affected in affecteds)
            {
                if (affected is MonsterFighter)
                {
                    mobs.Add(affected as MonsterFighter);
                }
            }
            if (mobs.Count <= 0)
                return (true);
            foreach (MonsterFighter monster in mobs)
            {
                processMonsterCheck(Caster, monster, Record, Effect);
            }
            return (true);
        }

        public static void processMonsterCheck(Fighter Caster, MonsterFighter affected, SpellLevelRecord Record, ExtendedSpellEffect Effect)
        {

        }
    }
}
