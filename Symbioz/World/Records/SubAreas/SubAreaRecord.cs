using Shader.Helper;
using Symbioz.Core;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.SubAreas
{
    [Table("SubAreas")]
    public class SubAreaRecord : ITable
    {
        public static List<SubAreaRecord> SubAreas = new List<SubAreaRecord>();

        [Primary]
        public int Id;
        public string Name;
        public int Level;
        public bool IsConquestVillage;
        public bool Capturable;
        [Ignore]
        public int ArchMonsterTime = 0;
        [Ignore]
        public int ArchMonsterCount = 0;

        public SubAreaRecord(int id, string name, int level, bool isConquestVillage, bool capturable)
        {
            this.Id = id;
            this.Name = name;
            this.Level = level;
            this.IsConquestVillage = isConquestVillage;
            this.Capturable = capturable;
        }

        public static SubAreaRecord GetSubArea(int id)
        {
            return SubAreas.FirstOrDefault(x => x.Id == id);
        }

        public static string GetSubAreaName(int id)
        {
            string res = "NaN";
            var subArea = GetSubArea(id);
            if(subArea != null)
            {
                res = subArea.Name;
            }
            return res;
        }

        public static void UpdateLastArchMonsterOnSubArea(int idSubArea)
        {
            foreach (var subarea in SubAreaRecord.SubAreas)
            {
                if (subarea.Id == idSubArea)
                {
                    subarea.ArchMonsterTime = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                    break;
                }
            }
        }

        public static bool RefreshArchMonsterTime(int idSubArea)
        {
            foreach (var subarea in SubAreaRecord.SubAreas)
            {
                if (subarea.Id == idSubArea)
                {
                    if (subarea.ArchMonsterTime == 0)
                        return true;
                    var StartTime = subarea.ArchMonsterTime;
                    var CurrentTime = DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                    var seconds = 0;
                    while (StartTime >= CurrentTime)
                    {
                        seconds = 0;
                        StartTime++;
                    }
                    if (seconds >= ConfigurationManager.Instance.TimeBetweenSpawnArchMonster)
                    {
                        subarea.ArchMonsterCount = 0;
                        return true;
                    }
                    break;
                }
            }
            return false;
        }
    }
}
