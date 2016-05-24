using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("BreedsSpells",true)]
    public class BreedSpellRecord : ITable
    {
        public static List<BreedSpellRecord> BreedsSpells = new List<BreedSpellRecord>();

        [Primary]
        public int Id;
        public ushort SpellId;
        public ushort Level;
        public sbyte Breed;
        
        public BreedSpellRecord(int id,ushort spellid,ushort level,sbyte breed)
        {
            this.Id = id;
            this.SpellId = spellid;
            this.Level = level;
            this.Breed = breed;
        }
        public static List<BreedSpellRecord> GetBreedSpells(short breedid)
        {
            return BreedsSpells.FindAll(x => x.Breed == breedid);
        }
        public static List<SpellItem> GetBreedSpellsForLevel(short level, short breedid, List<short> actualspells)
        {
            var spells = GetBreedSpells(breedid).FindAll(x => x.Level <= level);
            List<SpellItem> results = new List<SpellItem>();
            foreach (var spell in spells)
            {
                if (!actualspells.Contains((short)spell.SpellId))
                    results.Add(new SpellItem(63, spell.SpellId, 1));
            }
            return results;
        }
        
    }
}
