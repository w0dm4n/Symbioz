using Symbioz.Core;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Titles")]
    public class TitleRecord : ITable
    {
        public static List<TitleRecord> Titles = new List<TitleRecord>();

        public int Id;
        public int NameMaleId;
        public int NameFemaleId;
        [Ignore]
        public string NameMale;
        [Ignore]
        public string NameFemale;

        public TitleRecord(int id,int namemaleid,int namefemaleid)
        {
            this.Id = id;
            this.NameMaleId = namemaleid;
            this.NameFemaleId = namefemaleid;
            this.NameMale = LangManager.GetText(namemaleid);
            this.NameFemale = LangManager.GetText(namefemaleid);
        }

        public static TitleRecord GetTitle(int id)
        {
            return Titles.Find(x => x.Id == id);
        }
    }
}
