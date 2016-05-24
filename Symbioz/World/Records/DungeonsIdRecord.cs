using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("DungeonsIds", true)]
    public class DungeonsIdRecord : ITable
    {
        public static List<DungeonsIdRecord> DungeonsId = new List<DungeonsIdRecord>();
        public int Id;
        public string Name;
        public DungeonsIdRecord(int id, string name)
        {
            this.Id = id;
            this.Name = name;   
        }
    }
}