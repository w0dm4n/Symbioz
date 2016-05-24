using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("BidShopsGains",true)]
    class BidShopGainRecord : ITable
    {
        public static List<BidShopGainRecord> BidShopsGains = new List<BidShopGainRecord>();

        [Primary]
        public int Id;
        public int CharacterId;
        public ushort ItemGID;
        public uint ItemQuantity;
        public uint ItemPrice;

        public BidShopGainRecord(int id,int characterid,ushort itemgid,uint itemquantity,uint itemprice)
        {
            this.Id = id;
            this.CharacterId = characterid;
            this.ItemGID = itemgid;
            this.ItemQuantity = itemquantity;
            this.ItemPrice = itemprice;
        }
        public static int PopNextUID()
        {
            List<int> uids = BidShopsGains.ConvertAll<int>(x => x.Id);
            uids.Sort();
            if (uids.Count == 0)
                return 1;
            return uids.Last() + 1;
        }
        public static List<BidShopGainRecord> GetAllCharacterBidShopGains(int characterid)
        {
            return BidShopsGains.FindAll(x => x.CharacterId == characterid);
        }

        public static void AddBidShopGain(BidShopItemRecord selledItem)
        {
            SaveTask.AddElement(new BidShopGainRecord(PopNextUID(), selledItem.OwnerId, selledItem.GID, selledItem.Quantity, selledItem.Price));
        }
        public static void RemoveAll(int characterid)
        {
            GetAllCharacterBidShopGains(characterid).ForEach(x => x.RemoveElement());
        }
    }
}
