using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Companions
{
    [Table("CompanionsSpells")]
    public class CompanionSpellRecord : ITable
    {
        public static List<CompanionSpellRecord> CompanionSpells = new List<CompanionSpellRecord>();

        [Primary]
        public int Id;
        public ushort SpellId;
        public short CompanionId;
        public List<sbyte> GradeByLevel;

        public CompanionSpellRecord(int id,ushort spellid,short companionid,List<sbyte> gradebylevel)
        {
            this.Id = id;
            this.SpellId = spellid;
            this.CompanionId = companionid;
            this.GradeByLevel = gradebylevel;
        }

        public static CompanionSpellRecord GetSpell(int id)
        {
            return CompanionSpells.Find(x => x.Id == id);
        }
    }
}
