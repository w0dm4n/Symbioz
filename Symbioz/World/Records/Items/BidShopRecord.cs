using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("BidShops",true)]
    class BidShopRecord : ITable
    {
        public static List<BidShopRecord> BidShops = new List<BidShopRecord>();

        public int Id;
        public List<uint> Quantities;
        public List<uint> ItemTypes;
        public uint MaxItemPerAccount;

        public BidShopRecord(int id,List<uint> quantities,List<uint> itemtypes,uint maxitemperaccount)
        {
            this.Id = id;
            this.Quantities = quantities;
            this.ItemTypes = itemtypes;
            this.MaxItemPerAccount = maxitemperaccount;
        }

        public static BidShopRecord GetBidShop(int id)
        {
            return BidShops.Find(x => x.Id == id);
        }
        public SellerBuyerDescriptor GetDescriptor(int npcid)
        {
            return new SellerBuyerDescriptor(Quantities, ItemTypes, 0, 0, 200, MaxItemPerAccount, -npcid, 100);
        }
    }
}
