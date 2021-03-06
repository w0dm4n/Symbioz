﻿using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.World.Models;
using Symbioz.World.Models.Exchanges;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    public class MerchantHandler
    {
        [MessageHandler]
        public static void HandleExchangeRequestOnShopStockMessage(ExchangeRequestOnShopStockMessage message, WorldClient client)
        {
            if (client.Character.Restrictions.isDead || client.Character.Record.Energy == 0)
            {
                client.Character.Reply("Impossible car vous êtes mort !");
                return;
            }
            var CharacterMerchants = CharactersMerchantsRecord.GetCharactersItems(client.Character.Id);
            var ItemsList = new List<ObjectItemToSell>();

            foreach (var m in CharacterMerchants)
            {
                var item = CharacterItemRecord.GetItemByUID((uint)m.ItemUID);
                if (item != null)
                    ItemsList.Add(new ObjectItemToSell(item.GID, item.GetEffects(), item.UID, (uint)m.Quantity, (uint)m.Price));
            }
            client.Character.ShopStockInstance = new ShopStockExchange(client);
            client.Send(new ExchangeShopStockStartedMessage(ItemsList));
        }

        [MessageHandler]
        public static void HandleExchangeObjectMovePricedMessage(ExchangeObjectMovePricedMessage message, WorldClient client)
        {
            if (client.Character.BidShopInstance != null)
            {
                client.Character.BidShopInstance.AddItem(message.objectUID, message.quantity, message.price);
            }
            if (client.Character.ShopStockInstance != null)
            {
                client.Character.ShopStockInstance.AddItem(message.objectUID, (uint)message.quantity, message.price);
            }
        }

        [MessageHandler]
        public static void HandleExchangeObjectModifyPricedMessage(ExchangeObjectModifyPricedMessage message, WorldClient client)
        {
            if (client.Character.ShopStockInstance != null)
            {
                client.Character.ShopStockInstance.UpdateItem(message.objectUID, message.quantity, message.price);
            }
        }

        [MessageHandler]
        public static void HandleExchangeShowVendorTaxMessage(ExchangeShowVendorTaxMessage message, WorldClient client)
        {
            client.Send(new ExchangeReplyTaxVendorMessage(0, client.Character.GetTaxCost()));
        }

        [MessageHandler]
        public static void HandleExchangeStartAsVendorMessage(ExchangeStartAsVendorMessage message, WorldClient client)
        {
            if (client.Character.MerchantItems.Count == 0)
            {
                string[] data = new string[0];
                client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_DIALOG, true));
                client.Send(new TextInformationMessage(1, 23, data));
            }
            else
            {
                if (client.Character.Map.HaveZaap)
                {
                    client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_DIALOG, true));
                    client.Character.Reply("Impossible de passer en mode marchand sur cette map", Color.Red);
                    return;
                }
                if (client.Character.Record.Kamas < client.Character.GetTaxCost())
                {
                    string[] data = new string[0];
                    client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_DIALOG, true));
                    client.Send(new TextInformationMessage(0, 57, data));
                    return;
                }
                client.SendRaw("merchant");
            }
           
        }

        [MessageHandler]
        public static void HandleExchangeOnHumanVendorRequestMessage(ExchangeOnHumanVendorRequestMessage message, WorldClient client)
        {
            var ItemsList = CharactersMerchantsRecord.GetItemsFromCharacterId(message.humanVendorId);
            List<ObjectItemToSellInHumanVendorShop> ItemsObject = new List<ObjectItemToSellInHumanVendorShop>();
            if (ItemsList != null)
            {
                foreach (var itemList in ItemsList)
                {
                    var item = CharacterItemRecord.GetItemByUID((uint)itemList.ItemUID);
                    if (item != null)
                        ItemsObject.Add(new ObjectItemToSellInHumanVendorShop(item.GID, item.GetEffects(), item.UID, itemList.Quantity, itemList.Price, itemList.Price));
                }
                client.Character.ShopStockInstance = new ShopStockExchange(client);
                client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
                client.Send(new ExchangeStartOkHumanVendorMessage(message.humanVendorId, ItemsObject));
            }
            else
            {
                client.Character.Reply("Impossible car ce joueur n'a plus aucun item a vendre !", Color.Red);
            }
        }
    }
}
