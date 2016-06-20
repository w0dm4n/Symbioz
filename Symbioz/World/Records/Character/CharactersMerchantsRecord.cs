using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("CharactersMerchants", true)]
    public class CharactersMerchantsRecord : ITable
    {
        private static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public static List<CharactersMerchantsRecord> CharactersMerchants = new List<CharactersMerchantsRecord>();

        [Primary]
        public int Id;
        public int CharacterId;
        public int ItemUID;
        public uint Price;
        public uint Quantity;

        public CharactersMerchantsRecord(int id, int characterId, int itemUID, uint price, uint quantity)
        {
            this.Id = id;
            this.CharacterId = characterId;
            this.ItemUID = itemUID;
            this.Price = price;
            this.Quantity = quantity;
        }

        public static List<CharactersMerchantsRecord> GetCharactersItems(int CharacterId)
        {
            List<CharactersMerchantsRecord> Items = new List<CharactersMerchantsRecord>();
            foreach (var m in CharactersMerchantsRecord.CharactersMerchants)
            {
                if (m.CharacterId == CharacterId)
                    Items.Add(m);
            }
            return (Items);
        }

        public static bool InMerchantList(int itemUID)
        {
            foreach (var m in CharactersMerchantsRecord.CharactersMerchants)
            {
                if (m.ItemUID == itemUID)
                    return true;
            }
            return false;
        }

        public static int PopNextId()
        {
            Locker.EnterReadLock();
            try
            {
                var ids = CharactersMerchants.ConvertAll<int>(x => x.Id);
                ids.Sort();
                return ids.Count == 0 ? 1 : ids.Last() + 1;
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        public static CharactersMerchantsRecord GetItemFromUID(int itemUID)
        {
            foreach (var item in CharactersMerchants)
            {
                if (item.ItemUID == itemUID)
                {
                    return item;
                }
            }
            return null;
        }

        public static uint GetQuantityFromUID(int itemUID)
        {
            foreach (var m in CharactersMerchantsRecord.CharactersMerchants)
            {
                if (m.ItemUID == itemUID)
                    return m.Quantity;
            }
            return 0;
        }

        public static List<CharactersMerchantsRecord> GetItemsFromCharacterId(uint characterId)
        {
            List<CharactersMerchantsRecord> items = new List<CharactersMerchantsRecord>();
            foreach (var all in CharactersMerchants)
                if (all.CharacterId == characterId)
                    items.Add(all);
            return (items.Count != 0) ? items : null;
        }

        public static void DeleteFromUID(int ObjectUID)
        {
            foreach (var item in CharactersMerchants)
            {
                if (item.ItemUID == ObjectUID)
                {
                    SaveTask.RemoveElement(item);
                    break;
                }
            }
        }

        public static void UpdateItemQuantityFromUID(int ObjectUID, int newQuantity)
        {
            foreach (var item in CharactersMerchants)
            {
                if (item.ItemUID == ObjectUID)
                {
                    item.Quantity = (uint)newQuantity;
                    SaveTask.UpdateElement(item);
                    break;
                }
            }
        }
    }
}
