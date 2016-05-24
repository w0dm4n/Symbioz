using Symbioz.Core;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Jobs",true)]
    class JobRecord : ITable
    {
        public static List<JobRecord> Jobs = new List<JobRecord>();
        [Primary]
        public sbyte Id;
        public int NameId;
        public string Name { get; set; }
        public JobRecord(sbyte id,int nameid)
        {
            this.Id = id;
            this.NameId = nameid;
            this.Name = LangManager.GetText(nameid);
        }
        public static string GetJobName(sbyte id)
        {
            var job = Jobs.Find(x => x.Id == id);
            if (job == null)
                return "?";
            else
                return job.Name;
        }
    }
}
