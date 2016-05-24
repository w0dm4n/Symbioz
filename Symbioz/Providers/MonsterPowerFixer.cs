using Symbioz.Core.Startup;
using Symbioz.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers
{
    /// <summary>
    /// To see
    /// </summary>
    class MonsterPowerFixer
    {
        public const int MAX_POWER = 400;

        [StartupInvoke("Monsters Power Fix",StartupInvokeType.Internal)]
        public static void FixMonsters()
        {
            foreach (var monster in MonsterGradeRecord.MonstersGrades)
            {
                monster.Power = (short)(monster.Level * monster.GradeId);

                if (monster.Power > MAX_POWER)
                    monster.Power = MAX_POWER;
            }
        }
    }
}
