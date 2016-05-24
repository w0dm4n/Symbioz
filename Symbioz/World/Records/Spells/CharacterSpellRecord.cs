using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("CharactersSpells", true)]
    public class CharacterSpellRecord : ITable
    {
        public static List<CharacterSpellRecord> CharactersSpells = new List<CharacterSpellRecord>();
        [Primary]
        public int Id;
        public int CharacterId;
        public int SpellId;
        [Update]
        public sbyte SpellLevel;

        public CharacterSpellRecord(int id, int characterid, int spellid, sbyte spelllevel)
        {
            this.Id = id;
            this.CharacterId = characterid;
            this.SpellId = spellid;
            this.SpellLevel = spelllevel;
        }
        public SpellItem GetSpellItem()
        {
            var item = new SpellItem(63, SpellId, SpellLevel);
            item.characterId = CharacterId;
            return item;
        }
        public static void UpdateSpellLevel(SpellItem spell)
        {
            var spellRecord = CharactersSpells.FindAll(x => x.CharacterId == spell.characterId).Find(x => x.SpellId == spell.spellId);
            spellRecord.SpellLevel = spell.spellLevel;
            spellRecord.UpdateElement();
        }
        public static List<SpellItem> GetCharacterSpells(int characterid)
        {
            return CharactersSpells.FindAll(x => x.CharacterId == characterid).ConvertAll<SpellItem>(x => x.GetSpellItem());
        }
        public static void SetSpellLevel(int characterid, short spellid, sbyte spelllevel)
        {
            CharacterSpellRecord spell = CharactersSpells.Find(x => x.CharacterId == characterid && x.SpellId == spellid);
            spell.SpellLevel = spelllevel;
            spell.UpdateElement();
        }
        public static void RemoveAll(int characterid)
        {
            CharactersSpells.FindAll(x => x.CharacterId == characterid).ForEach(x => x.RemoveElement());
        }
    }
}
