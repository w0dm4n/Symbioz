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
    }
}
