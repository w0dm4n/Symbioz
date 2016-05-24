using Symbioz.ORM;
using Symbioz.Providers.ActorIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Spells",true)]
    public class SpellRecord : ITable
    {
        public static List<SpellRecord> Spells = new List<SpellRecord>();

        public ushort Id;
        public string Name;
        public string Description;
        public sbyte TypeId;
        public short IconId;
        public List<int> SpellLevels;
        [Ignore]
        public SpellCategoryEnum Category;

        public SpellRecord(ushort id,string name,string description,sbyte typeid,short iconid,List<int> spelllevels)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.TypeId = typeid;
            this.IconId = iconid;
            this.SpellLevels = spelllevels;
        }
        public static ushort GetSpellId(int levelid)
        {
            return Spells.Find(x => x.SpellLevels.Contains(levelid)).Id;
        }
        public static SpellRecord GetSpell(ushort id)
        {
            return Spells.Find(x => x.Id == id);
        }
    }
}
