using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Experiences")]
    class ExperienceRecord : ITable
    {
        public static List<ExperienceRecord> Experiences = new List<ExperienceRecord>();


        public uint Level;
        public ulong Experience;
        public int Honor;
        public ulong Guild;

        public ExperienceRecord(uint level, ulong exp, int honor, ulong guild)
        {
            this.Level = level;
            this.Experience = exp;
            this.Honor = (int)honor;
            this.Guild = guild;
        }
        public static ulong GetExperienceForLevel(uint level)
        {
            if (level > 200)
                return 0;
            return Experiences.Find(x => x.Level == level).Experience;
        }
        public static ushort GetHonorForGrade(sbyte grade)
        {

            if (grade > 10 || grade == 0)
                return 0;
            return (ushort)Experiences.Find(x => x.Level == grade).Honor;
        }
        public static ulong GetExperienceForGuild(ushort level)
        {
            if (level > 200)
                return 0;
            return Experiences.Find(x => x.Level == level).Guild;
        }
    }
}
