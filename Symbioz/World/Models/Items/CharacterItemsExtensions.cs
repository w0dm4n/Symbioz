using Symbioz.DofusProtocol.Types;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models
{
    public static class CharacterItemsExtensions
    {
        public static CharacterItemRecord ExistingItem(this List<CharacterItemRecord> itemlist, CharacterItemRecord item)
        {
            var existingItem = itemlist.Find(x => x.GID == item.GID && x.EffectsLinkedToList == item.EffectsLinkedToList && x.Position == 63);
            if (existingItem == null)
                return null;
            else return existingItem;
        }
        public static CharacterItemRecord ExistingItem(this List<CharacterItemRecord> itemlist,BankItemRecord item)
        {
            var existingItem = itemlist.Find(x => x.GID == item.GID && x.EffectsLinkedToList == item.EffectsLinkedToList);
            if (existingItem == null)
                return null;
            else return existingItem;
        }
        public static BankItemRecord ExistingItem(this List<BankItemRecord> itemlist,BankItemRecord item)
        {
            var existingItem = itemlist.Find(x => x.GID == item.GID && x.EffectsLinkedToList == item.EffectsLinkedToList);
            if (existingItem == null)
                return null;
            else return existingItem;
        }
        public static BankItemRecord ExistingItem(this List<BankItemRecord> itemlist, CharacterItemRecord item)
        {
            var existingItem = itemlist.Find(x => x.GID == item.GID && x.EffectsLinkedToList == item.EffectsLinkedToList);
            if (existingItem == null)
                return null;
            else return existingItem;
        }
        public static List<ObjectItem> GetObjects(this List<BankItemRecord> items)
        {
            return items.ConvertAll<ObjectItem>(x => new ObjectItem(63, x.GID, x.GetEffects(), x.UID, x.Quantity));
        }
    }
}
