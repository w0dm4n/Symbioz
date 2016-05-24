using Symbioz.Core;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Ornaments")]
    public class OrnamentRecord : ITable
    {
        public static List<OrnamentRecord> Ornaments = new List<OrnamentRecord>();

        public ushort Id;
        public int NameId;
        [Ignore]
        public string Name;
        public OrnamentRecord(ushort id,int nameid)
        {
            this.Id = id;
            this.NameId = nameid;
            this.Name = LangManager.GetText(nameid);
        }
    }
}
