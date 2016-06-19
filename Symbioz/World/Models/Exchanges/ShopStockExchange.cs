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

namespace Symbioz.World.Models.Exchanges
{
    public class ShopStockExchange : Exchange
    {
        public WorldClient Client { get; set; }
        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.STORAGE;
            }
        }

        public ShopStockExchange(WorldClient client)
        {
            this.Client = client;
            this.Client.Character.ExchangeType = Enums.ExchangeTypeEnum.STORAGE;
            this.Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
        }

        public bool InCurrentShop(uint objectUID)
        {
            foreach (var item in Client.Character.MerchantItems)
            {
                if (item.ItemUID == objectUID)
                    return true;
            }
            return false;
        }

        public CharactersMerchantsRecord GetFromUID(uint objectUID)
        {
            foreach (var item in Client.Character.MerchantItems)
            {
                if (item.ItemUID == objectUID)
                    return item;
            }
            return null;
        }

        public void UpdateQuantity(CharactersMerchantsRecord item, uint newQuantity)
        {
            foreach (var tmpItem in Client.Character.MerchantItems)
            {
                if (tmpItem == item)
                {
                    tmpItem.Quantity = newQuantity;
                    SaveTask.UpdateElement(tmpItem, Client.Character.Id);
                }
            }
        }

        public void AddItem(uint objectUID, uint quantity, uint price)
        {
            var item = CharacterItemRecord.GetItemByUID((uint)objectUID);
            uint quantityBeforeUpdate = 0;
            if (item != null)
            {
                if (InCurrentShop(item.UID))
                {
                    if (quantity <= (item.Quantity - GetFromUID(objectUID).Quantity))
                    {
                        var itemInShop = GetFromUID(item.UID);
                        if (itemInShop != null)
                        {
                            quantityBeforeUpdate = itemInShop.Quantity;
                            UpdateQuantity(itemInShop, (quantity + itemInShop.Quantity));
                        }
                        var itemObject = new ObjectItemToSell(item.GID, item.GetEffects(), item.UID, (uint)(quantityBeforeUpdate + quantity), price);
                        this.Client.Send(new ExchangeShopStockMovementUpdatedMessage(itemObject));
                        Client.Character.Inventory.Refresh();
                    }
                }
                else
                {
                    if (quantity <= item.Quantity)
                    {
                        var newItemMerchant = new CharactersMerchantsRecord(CharactersMerchantsRecord.PopNextId(), this.Client.Character.Id, (int)item.UID, price, (uint)quantity);
                        this.Client.Character.MerchantItems.Add(newItemMerchant);
                        SaveTask.AddElement(newItemMerchant, this.Client.Character.Id);
                        var itemObject = new ObjectItemToSell(item.GID, item.GetEffects(), item.UID, (uint)quantity, price);
                        this.Client.Send(new ExchangeShopStockMovementUpdatedMessage(itemObject));
                        Client.Character.Inventory.Refresh();
                    }
                }
            }
        }

        public override void Ready(bool ready, ushort step)
        {
            throw new NotImplementedException();
        }

        public override void MoveItem(uint uid, int quantity)
        {
            var Item = GetFromUID(uid);
            if (Item != null)
            {
                Client.Send(new ExchangeShopStockMovementRemovedMessage((uint)Item.ItemUID));

            }
        }

        public void RemoveItem(uint uid)
        {
            var Item = GetFromUID(uid);
            if (Item != null)
            {
                Client.Send(new ExchangeShopStockMovementRemovedMessage((uint)Item.ItemUID));
                foreach (var item in Client.Character.MerchantItems)
                {
                    if (item == Item)
                    {
                        Client.Character.MerchantItems.Remove(item);
                        SaveTask.RemoveElement(item, Client.Character.Id);
                        Client.Character.Inventory.Refresh();
                        break;
                    }
                }
            }
        }

        public void UpdateItem(uint objectUID, int quantity, uint price)
        {
            var item = CharacterItemRecord.GetItemByUID((uint)objectUID);
            ObjectItemToSell itemObject = null;
            if (quantity > 0)
                itemObject = new ObjectItemToSell(item.GID, item.GetEffects(), item.UID, (uint)quantity, price);
            else
                itemObject = new ObjectItemToSell(item.GID, item.GetEffects(), item.UID, (uint)GetFromUID(objectUID).Quantity, price);
            Client.Send(new ExchangeShopStockMovementUpdatedMessage(itemObject));
            foreach (var itemMerchant in Client.Character.MerchantItems)
            {
                if (itemMerchant.ItemUID == objectUID)
                {
                    itemMerchant.Price = price;
                    if (quantity > 0)
                    {
                        if (quantity <= (item.Quantity - itemMerchant.Quantity))
                        {
                            itemMerchant.Quantity = (uint)quantity;
                            Client.Send(new ExchangeShopStockMovementUpdatedMessage(itemObject));
                            Client.Character.Inventory.Refresh();
                        }
                    }
                    break;
                }
            }
        }

        public override void CancelExchange()
        {
            this.Client.Character.ShopStockInstance = null;
        }
    }
}
