using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.ORM;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Helper;
using Symbioz.World.Handlers;
using Symbioz.Network.Servers;

namespace Symbioz.World.Models.Exchanges
{
    public class BidShopExchange
    {
        public List<WorldClient> BidShopInstance { get { return WorldServer.Instance.WorldClients.FindAll(x => x.Character.BidShopInstance != null && x.Character.BidShopInstance.BidShopId == BidShopId); } }
        public List<BidShopItemRecord> BidShopItems { get { return BidShopItemRecord.GetAllBidShopItems(BidShopId); } }
        public List<ObjectItemToSellInBid> CharacterBidItems { get { return BidShopItemRecord.GetCharactersBidItems(Client.Character.Id, BidShopId); } }
        public WorldClient Client { get; set; }
        public SellerBuyerDescriptor Descriptor { get; set; }
        public ushort WatchingItemGID { get; set; }
        public int BidShopId { get; set; }
        public BidShopExchange(WorldClient client, SellerBuyerDescriptor buyerdescriptor, int bidshopid)
        {
            this.Client = client;
            this.Descriptor = buyerdescriptor;
            this.BidShopId = bidshopid;
        }

        #region Sell
        public void OpenSellPanel()
        {
            Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Character.ExchangeType = ExchangeTypeEnum.BIDHOUSE_SELL;
            Client.Send(new ExchangeStartedBidSellerMessage(Descriptor, CharacterBidItems));
        }
        public void AddItem(uint uid, int quantity, uint price)
        {
            var item = Client.Character.Inventory.GetItem(uid);
            if (item == null)
            {
                Client.Character.ReplyError("Impossible, cet item n'éxiste pas");
                return;
            }
            if (item.Quantity < quantity)
                return;
            if (CharacterBidItems.Count == BidShopRecord.GetBidShop(BidShopId).MaxItemPerAccount)
                return;
            Client.Character.Inventory.RemoveItem(item.UID, (uint)quantity);
            var existing = CharacterBidItems.Find(x => x.objectUID == item.UID);
            if (existing == null)
                SaveTask.AddElement(new BidShopItemRecord(BidShopId, price, quantity, item));
            else
            {
                SaveTask.AddElement(new BidShopItemRecord(BidShopId, price, quantity, item.CloneAndGetNewUID()));
            }
            OpenSellPanel();
        }
        public void MoveItem(uint uid, int quantity)
        {
            var item = CharacterBidItems.Find(x => x.objectUID == uid);
            var realItem = BidShopItemRecord.GetBidShopItem(uid);
            SaveTask.RemoveElement(realItem);
            Client.Character.Inventory.Add(new CharacterItemRecord(realItem.UID, 63, realItem.GID, Client.Character.Id, realItem.Quantity, realItem.GetEffects()));
            OpenSellPanel();
        }
        #endregion
        #region Buy
        public void BuyItem(uint uid, uint quantity, uint price)
        {
            if (Client.Character.RemoveKamas((int)price, true))
            {
                var item = BidShopItems.Find(x => x.UID == uid);
                if (!BidShopsHandler.TrySendBishopGainAdded(item.OwnerId, item.GID, (int)quantity, (int)price))
                {
                    BidShopGainRecord.AddBidShopGain(item);
                }
                Client.Character.Inventory.Add(item);
                RemoveItem(item);
            }
           
         
        }
        public void RemoveItem(BidShopItemRecord item)
        {
            item.RemoveElement();
            ShowItemList(item.GID, true);
            if (!ItemGIDExistInBid(item.GID))
            {
                DeleteGIDFromBidShop(item.GID);
            }
        }
        public void OpenBuyPanel()
        {
            Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Character.ExchangeType = ExchangeTypeEnum.BIDHOUSE_BUY;
            Client.Send(new ExchangeStartedBidBuyerMessage(Descriptor));
        }
        void DeleteGIDFromBidShop(ushort gid)
        {
            BidShopInstance.ForEach(x => x.Send(new ExchangeBidHouseGenericItemRemovedMessage(gid)));
        }
        public void ShowBidHouseType(uint itemtype)
        {
            List<uint> gids = new List<uint>();
            foreach (var item in BidShopItems)
            {
                var template = ItemRecord.GetItem(item.GID);
                if (template.TypeId == itemtype && !gids.Contains(item.GID))
                    gids.Add(item.GID);
            }
            Client.Send(new ExchangeTypesExchangerDescriptionForUserMessage(gids));
        }
        public void ShowItemList(ushort itemgid, bool toInstance = false)
        {
            WatchingItemGID = itemgid;
            if (toInstance)
            {

                var clients = BidShopInstance.FindAll(x => x.Character.BidShopInstance.WatchingItemGID == itemgid);
                clients.ForEach(x => x.Send(new ExchangeTypesItemsExchangerDescriptionForUserMessage(SortedItems(GetAllItemsByGID(itemgid)))));
            }
            else
            {
                Client.Send(new ExchangeTypesItemsExchangerDescriptionForUserMessage(SortedItems(GetAllItemsByGID(itemgid))));
            }
        }
        #endregion
        #region ItemsSort
        bool ItemGIDExistInBid(ushort gid)
        {
            if (BidShopItems.Find(x => x.GID == gid) == null)
                return false;
            else
                return true;
        }
        List<BidShopItemRecord> GetAllItemsByGID(ushort gid)
        {
            return BidShopItems.FindAll(x => x.GID == gid);

        }
        Dictionary<List<BidShopItemRecord>, string> GetItemSortedByEffects(List<BidShopItemRecord> items)
        {
            Dictionary<List<BidShopItemRecord>, string> dic = new Dictionary<List<BidShopItemRecord>, string>();
            foreach (var item in items)
            {
                var data = dic.FirstOrDefault(x => x.Value == item.EffectsLinkedToList);
                if (data.Key != null && data.Key.Count() > 0)
                {
                    data.Key.Add(item);
                }
                else
                    dic.Add(new List<BidShopItemRecord> { item }, item.EffectsLinkedToList);
            }
            return dic;
        }
        List<BidExchangerObjectInfo> SortedItems(List<BidShopItemRecord> items)
        {
            List<BidExchangerObjectInfo> result = new List<BidExchangerObjectInfo>();
            uint[] bidShopQuantites = Descriptor.quantities.ToArray();
            foreach (var itemsData in GetItemSortedByEffects(items))
            {

                var firstQuantityItemsDatas = itemsData.Key.FindAll(x => x.Quantity == bidShopQuantites[0]);
                var secondQuantityItemsDatas = itemsData.Key.FindAll(x => x.Quantity == bidShopQuantites[1]);
                var thirdQuantityItemsDatas = itemsData.Key.FindAll(x => x.Quantity == bidShopQuantites[2]);

                firstQuantityItemsDatas = firstQuantityItemsDatas.OrderBy(x => x.Price).ToList();
                secondQuantityItemsDatas = secondQuantityItemsDatas.OrderBy(x => x.Price).ToList();
                thirdQuantityItemsDatas = thirdQuantityItemsDatas.OrderBy(x => x.Price).ToList();

                BidShopItemRecord sample = null;
                int firstQuantityPrice = 0;
                int secondQuantityPrice = 0;
                int thirdQuantiyPrice = 0;
                if (firstQuantityItemsDatas.Count() > 0)
                {
                    firstQuantityPrice = (int)firstQuantityItemsDatas[0].Price;
                    sample = firstQuantityItemsDatas[0];
                }
                if (secondQuantityItemsDatas.Count() > 0)
                {
                    secondQuantityPrice = (int)secondQuantityItemsDatas[0].Price;
                    sample = secondQuantityItemsDatas[0];
                }
                if (thirdQuantityItemsDatas.Count() > 0)
                {
                    thirdQuantiyPrice = (int)thirdQuantityItemsDatas[0].Price;
                    sample = thirdQuantityItemsDatas[0];
                }
                if (sample != null)
                {
                    var prices = new int[3] { firstQuantityPrice, secondQuantityPrice, thirdQuantiyPrice };
                    result.Add(new BidExchangerObjectInfo(sample.UID, sample.GetEffects(), prices));
                }

            }
            return result;
        }
        #endregion

        public void CancelExchange()
        {
            Client.Character.BidShopInstance = null;
            Client = null;
            Descriptor = null;
        }
    }
}
