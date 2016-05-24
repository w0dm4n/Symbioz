using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using Symbioz.Helper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("SpellsShortcuts",true)]
    public class SpellShortcutRecord : ITable
    {
        public static List<SpellShortcutRecord> SpellsShortcuts = new List<SpellShortcutRecord>();

        [Primary]
        public int Id;
        public int CharacterId;
        public ushort SpellId;
        [Update]
        public sbyte SlotId;

        public SpellShortcutRecord(int id,int characterid,ushort spellid,sbyte slotid)
        {
            this.Id = id;
            this.CharacterId = characterid;
            this.SpellId = spellid;
            this.SlotId = slotid;
        }
        public static void RemoveAll(int characterid)
        {
            SpellsShortcuts.FindAll(x => x.CharacterId == characterid).ForEach(x => SaveTask.RemoveElement(x));
        }
        public static List<ShortcutSpell> GetCharacterShortcuts(int characterid)
        {
            return SpellsShortcuts.FindAll(x => x.CharacterId == characterid).ConvertAll<ShortcutSpell>(x => new ShortcutSpell(x.SlotId, x.SpellId));
        }
        static SpellShortcutRecord GetShorcut(int characterid,sbyte slotid)
        {
            return SpellsShortcuts.Find(x => x.CharacterId == characterid && x.SlotId == slotid);
        }
        public static void RemoveShortcut(int characterid, sbyte slot)
        {
            SaveTask.RemoveElement(GetShorcut(characterid, slot));
        }
        public static void SwapSortcut(int characterid,sbyte firstslot,sbyte secondslot)
        {
            var shortcut1 = GetShorcut(characterid, firstslot);
            var shortcut2 = GetShorcut(characterid, secondslot);
            if (!shortcut1.IsNull() && !shortcut2.IsNull())
            {
                if (shortcut1.SlotId == firstslot)
                    shortcut1.SlotId = secondslot;
                else
                    shortcut1.SlotId = firstslot;

                if (shortcut2.SlotId == firstslot)
                    shortcut2.SlotId = secondslot;
                else
                    shortcut2.SlotId = firstslot;
                SaveTask.UpdateElement(shortcut2);
            }
            else if (shortcut1 != null)
                shortcut1.SlotId = secondslot;
            SaveTask.UpdateElement(shortcut1);
        }
        public static void AddShortcut(int characterid,sbyte slotid,ushort spellid)
        {
            var existing = GetShorcut(characterid, slotid);
            if (existing != null)
                RemoveShortcut(existing.CharacterId, existing.SlotId);
            SaveTask.AddElement(new SpellShortcutRecord(SpellsShortcuts.PopNextId<SpellShortcutRecord>(x=>x.Id), characterid, spellid, slotid));
        }
        public static sbyte GetFreeSlotId(int characterid)
        {
            var spells = SpellsShortcuts.FindAll(x => x.CharacterId == characterid);
            var ids = spells.ConvertAll<sbyte>(x => x.SlotId);
            if (ids.Count() == 0)
                return 0;
            ids.Sort();
            return (sbyte)(ids.Last() + 1);
        }
    }
}
