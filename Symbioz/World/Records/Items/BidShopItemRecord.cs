using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.ORM;
using Symbioz.World.Models;
using Symbioz.World.Models.Exchanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("BidShopsItems",true)]
    public class BidShopItemRecord : ITable
    {
        public static List<BidShopItemRecord> BidShopsItems = new List<BidShopItemRecord>();

        public int BidShopId;
        public int OwnerId;
        [Primary]
        public uint UID;
        public ushort GID;
        public string Effects;
        [Ignore]
        List<ObjectEffect> m_realEffect = new List<ObjectEffect>();
        public uint Price;
        public uint Quantity;
        [Ignore]
        public string EffectsLinkedToList { get { return CharacterItemRecord.EffectsToString(m_realEffect); } } 
        public BidShopItemRecord(int bidshopid,int ownerid,uint uid,ushort gid,string effects,uint price,uint quantity)
        {
            this.BidShopId = bidshopid;
            this.OwnerId = ownerid;
            this.UID = uid;
            this.GID = gid;
            this.Effects = effects;
            this.Price = price;
            this.Quantity = quantity;
            this.m_realEffect = CharacterItemRecord.StringToObjectEffects(Effects);
        }
        public BidShopItemRecord(int bidshopid,uint price,int quantity,CharacterItemRecord item)
        {
            this.BidShopId = bidshopid;
            this.OwnerId = item.CharacterId;
            this.UID = item.UID;
            this.GID = item.GID;
            this.Effects = item.Effects;
            this.Price = price;
            this.Quantity = (uint)quantity;
            this.m_realEffect = item.GetEffects();
        }
        public ObjectItemToSellInBid GetObjectItemToSellInBid()
        {
            return new ObjectItemToSellInBid(GID, m_realEffect, UID, Quantity, Price, 100);
        }
        public List<ObjectEffect> GetEffects()
        {
            return m_realEffect;
        }
        public static BidShopItemRecord GetBidShopItem(uint uid)
        {
            return BidShopsItems.Find(x => x.UID == uid);
        }
        public static List<ObjectItemToSellInBid> GetCharactersBidItems(int characterid,int bidshopid)
        {
            return BidShopsItems.FindAll(x => x.OwnerId == characterid && x.BidShopId == bidshopid).ConvertAll<ObjectItemToSellInBid>(x=>x.GetObjectItemToSellInBid());
        }
        public static List<uint> GetAllItemsUIDs()
        {
            return BidShopsItems.ConvertAll<uint>(x => x.UID);
        }
        public static List<BidShopItemRecord> GetAllBidShopItems(int bishopid)
        {
            return BidShopsItems.FindAll(x => x.BidShopId == bishopid);
        }
        public static void RemoveAll(int characterid)
        {
            var items = BidShopsItems.FindAll(x => x.OwnerId == characterid);
            foreach (var item in items)
            {
                Exchange.GetOnlineClientsExchanging(ExchangeTypeEnum.BIDHOUSE_BUY).ForEach(x => x.Character.BidShopInstance.RemoveItem(item));
            }
        }
        
        

    }
}
