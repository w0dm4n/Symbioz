using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbioz;
using System.Threading.Tasks;
using Symbioz.Network.Servers;

namespace Symbioz.World.Records
{
    [Table("GeneralShortcuts")]
    public class GeneralShortcutRecord : ITable
    {
        public static List<GeneralShortcutRecord> GeneralShortcuts = new List<GeneralShortcutRecord>();

        [Primary]
        public int Id;
        public int CharacterId;
        public int ShortcutType;
        public int Value1;
        public int Value2;
        [Update]
        public sbyte SlotId;

        public GeneralShortcutRecord(int id, int characterid, int shortcuttype, int value1, int value2, sbyte slotid)
        {
            this.Id = id;
            this.CharacterId = characterid;
            this.ShortcutType = shortcuttype;
            this.Value1 = value1;
            this.Value2 = value2;
            this.SlotId = slotid;
        }
        public static void RemoveAll(int characterId)
        {
            GeneralShortcuts.FindAll(x => x.CharacterId == characterId).ForEach(x => SaveTask.RemoveElementWithoutDelay(x));
        }
        public static void RemoveShortcut(int characterId, sbyte slot)
        {
            SaveTask.RemoveElement(GetShorcut(characterId, slot), characterId);
        }
        public static void AddShortcut(int characterId, sbyte slotid, int shortcuttype, int value1, int value2)
        {
            var existing = GetShorcut(characterId, slotid);
            if (existing != null)
                RemoveShortcut(existing.CharacterId, existing.SlotId);
            var shortcut = new GeneralShortcutRecord(GeneralShortcuts.PopNextId<GeneralShortcutRecord>(x => x.Id), characterId, shortcuttype, value1, value2, slotid);
            SaveTask.AddElement(shortcut, characterId);
        }
        static GeneralShortcutRecord GetShorcut(int characterid, sbyte slotid)
        {
            return GeneralShortcuts.Find(x => x.CharacterId == characterid && x.SlotId == slotid);
        }
        public static void SwapSortcut(int characterId, sbyte firstslot, sbyte secondslot)
        {
            var shortcut1 = GetShorcut(characterId, firstslot);
            var shortcut2 = GetShorcut(characterId, secondslot);
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
                SaveTask.UpdateElement(shortcut2, characterId);
            }
            else if (shortcut1 != null)
                shortcut1.SlotId = secondslot;
        }
        public static List<Shortcut> GetCharacterShortcuts(int characterid)
        {
            List<Shortcut> results = new List<Shortcut>();
            List<GeneralShortcutRecord> shortcuts = GeneralShortcuts.FindAll(x => x.CharacterId == characterid);
            foreach (var shortcut in shortcuts)
            {
                switch (shortcut.ShortcutType)
                {
                    case ShortcutObjectItem.Id:
                        results.Add(new ShortcutObjectItem(shortcut.SlotId, shortcut.Value1, shortcut.Value2));
                        break;
                    case ShortcutSmiley.Id:
                        results.Add(new ShortcutSmiley(shortcut.SlotId, (sbyte)shortcut.Value1));
                        break;
                    case ShortcutEmote.Id:
                        results.Add(new ShortcutEmote(shortcut.SlotId, (byte)shortcut.Value1));
                        break;
                }
            }
            return results;
        }
    }
}
