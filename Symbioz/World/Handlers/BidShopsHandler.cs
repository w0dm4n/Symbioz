using Symbioz.Network.Messages;
using Symbioz.DofusProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.World.Records;
using Symbioz.ORM;
using Symbioz.Auth.Handlers;

namespace Symbioz.World.Handlers
{
    class BidShopsHandler
    {
        [MessageHandler]
        public static void HandleExchangeBidHouseSearch(ExchangeBidHouseSearchMessage message,WorldClient client)
        {
            if (client.Character.BidShopInstance.BidShopItems.Find(x => x.GID == message.genId) != null)
            {
                client.Character.BidShopInstance.ShowItemList(message.genId);
                client.Send(new ExchangeBidSearchOkMessage());
            }
            else
            {
                ConnectionHandler.SendSystemMessage(client, "Cet objet n'est pas en vente dans cet hotel de vente");
            }
        }
        [MessageHandler]
        public static void HandleExchangeBidHouseList(ExchangeBidHouseListMessage message,WorldClient client)
        {
            client.Character.BidShopInstance.ShowItemList(message.id);
        }
        [MessageHandler]
        public static void HandleExchangeBidHouseType(ExchangeBidHouseTypeMessage message,WorldClient client)
        {
            client.Character.BidShopInstance.ShowBidHouseType(message.type);
        }
        [MessageHandler]
        public static void HandleAddItemInBid(ExchangeObjectMovePricedMessage message, WorldClient client)
        {
            client.Character.BidShopInstance.AddItem(message.objectUID, message.quantity, message.price);
        }
        [MessageHandler]
        public static void HandleExchangeItemBidHouseBuy(ExchangeBidHouseBuyMessage message,WorldClient client)
        {
            client.Character.BidShopInstance.BuyItem(message.uid, message.qty, message.price);
        }
        public static bool TrySendBishopGainAdded(int ownerid,ushort itemgid,int quantity,int price)
        {
            var client = WorldServer.Instance.GetOnlineClient(ownerid);
            if (client == null)
                return false;
            var itemname = ItemRecord.GetItem(itemgid).Name;
            client.Account.Informations.BankKamas += (uint)price;
            client.Character.Reply("Banque : + " + price + " Kamas (vente de " +quantity+ "  <b>[" + itemname + "]</b>).");
            return true;
        }
        public static void AddEventualBidShopGains(WorldClient client)
        {
            var gains = BidShopGainRecord.GetAllCharacterBidShopGains(client.Character.Id);
            foreach (var gain in gains)
            {
                var itemname = ItemRecord.GetItem(gain.ItemGID).Name;
                client.Account.Informations.BankKamas += gain.ItemPrice;
                client.Character.Reply("Banque : + " + gain.ItemPrice + " Kamas (vente de " + gain.ItemQuantity + "  <b>[" + itemname + "]</b> hors jeu).");
                gain.RemoveElement();
                client.Account.Informations.UpdateElement();
            }
        }
    }
}
