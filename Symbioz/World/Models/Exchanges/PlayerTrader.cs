using Symbioz.DofusProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges
{
    public class PlayerTrader
    {
        public Character Character;
        private bool IsReady { get; set; }
        public List<CharacterItemRecord> Items;
        public int Kamas;

        public PlayerTrader(Character character)
        {
            Character = character;
            Items = new List<CharacterItemRecord>();
            IsReady = false;
            Kamas = 0;
        }

        public bool checkItemUID(CharacterItemRecord item)
        {
            foreach (var tmp in Items)
            {
                if (tmp.UID == item.UID)
                    return true;
            }
            return false;
        }

        public void MoveItem(uint uid, int quantity)
        {
            CharacterItemRecord obj;
            
            if (quantity > 0)
            {
                obj = Character.Inventory.GetItem(uid);
                if (obj == null || obj.Quantity < quantity)
                    return;
                else
                {
                    if (Character.Inventory.GetEquipedItems().Contains(obj))
                    {
                        Character.ReplyError("Impossible d'échanger cet objet car il est équipé sur votre personnage !");
                        return;
                    }
                    else
                        AddItemToPanel(obj, (uint)quantity, Character.Id);
                }
            }
            else
            {
                obj = Items.Find(x => x.UID == uid);
                RemoveItemFromPanel(obj, quantity, Character.Id);
            }
        }

        private Character GetMover(int moverId)
        {
            if (Character.PlayerTradeInstance.SecondTrader.Character.Id == moverId)
                return Character.PlayerTradeInstance.FirstTrader.Character;
            else
                return Character.PlayerTradeInstance.SecondTrader.Character;   
        }

        public void AddItemToPanel(CharacterItemRecord obj, uint quantity, int actuelMoverId)
        {
            var SecondTrader = GetMover(actuelMoverId);
            if (SecondTrader.PlayerTradeInstance.FirstTrader.checkItemUID(obj) || SecondTrader.PlayerTradeInstance.SecondTrader.checkItemUID(obj))
                return;
            if (IsReady)
                Ready(false, actuelMoverId);
            if (quantity == obj.Quantity)
            {
                var addedItem = obj.CloneWithUID();
                Character.Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                SecondTrader.Client.Send(new ExchangeObjectAddedMessage(true, addedItem.GetObjectItem()));
                Items.Add(addedItem);
                return;
            }
            else
            {
                var existing = Items.ExistingItem(obj);
                if (existing != null)
                {
                    existing.Quantity += (uint)quantity;
                    Character.Client.Send(new ExchangeObjectModifiedMessage(false, existing.GetObjectItem()));
                    SecondTrader.Client.Send(new ExchangeObjectModifiedMessage(true, existing.GetObjectItem()));
                    return;
                }
                else
                {
                    var addedItem = obj.CloneWithUID();
                    addedItem.Quantity = (uint)quantity;
                    Character.Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                    SecondTrader.Client.Send(new ExchangeObjectAddedMessage(true, addedItem.GetObjectItem()));
                    Items.Add(addedItem);
                    return;
                }
            }
        }

        public void RemoveItemFromPanel(CharacterItemRecord obj, int quantity, int actualMoverId)
        {
            var SecondTrader = GetMover(actualMoverId);
            if (IsReady)
                Ready(false, actualMoverId);
            if (obj.Quantity == (uint)-quantity)
            {
                Character.Client.Send(new ExchangeObjectRemovedMessage(false, obj.UID));
                SecondTrader.Client.Send(new ExchangeObjectRemovedMessage(true, obj.UID));
                Items.Remove(obj);
                return;
            }
            else
            {
                obj.Quantity = (uint)(obj.Quantity + quantity);
                Character.Client.Send(new ExchangeObjectModifiedMessage(false, obj.GetObjectItem()));
                SecondTrader.Client.Send(new ExchangeObjectModifiedMessage(true, obj.GetObjectItem()));
            }
        }

        public void MoveKamas(int quantity, int moveId)
        {
            var SecondTrader = GetMover(moveId);
            SecondTrader.Client.Send(new ExchangeKamaModifiedMessage(true, (uint)quantity));
            Kamas = quantity;
        }

        public void Ready(bool ready, int actualMoverId)
        {
            var SecondTrader = GetMover(actualMoverId);
            IsReady = ready;
            Character.Client.Send(new ExchangeIsReadyMessage((uint)Character.Id, ready));
            SecondTrader.Client.Send(new ExchangeIsReadyMessage((uint)Character.Id, ready));
            if (IsReady && SecondTrader.Trader.IsReady)
            {
                if (Items.All(delegate (CharacterItemRecord x) {
                    var item = Character.Inventory.GetItem(x.UID);
                    return item != null && item.Quantity >= x.Quantity;
                }) && SecondTrader.Trader.Items.All(delegate (CharacterItemRecord x) {
                    var item = SecondTrader.Inventory.GetItem(x.UID);
                    return item != null && item.Quantity >= x.Quantity;
                }))
                {
                    foreach (var item in Items)
                    {
                        item.CharacterId = SecondTrader.Id;
                        SecondTrader.Inventory.Add(item.CloneAndGetNewUID());
                        Character.Inventory.RemoveItem(item.UID, item.Quantity);
                    }
                    foreach (var item in SecondTrader.Trader.Items)
                    {
                        item.CharacterId = Character.Id;
                        Character.Inventory.Add(item.CloneAndGetNewUID());
                        SecondTrader.Inventory.RemoveItem(item.UID, item.Quantity);
                    }
                    if(Kamas > 0)
                    {
                        Character.RemoveKamas(Kamas);
                        SecondTrader.AddKamas(Kamas);
                    }
                    if(SecondTrader.Trader.Kamas > 0)
                    {
                        SecondTrader.RemoveKamas(SecondTrader.Trader.Kamas);
                        Character.AddKamas(SecondTrader.Trader.Kamas);
                    }
                    Character.PlayerTradeInstance.Close(true);
                }
            }
        }
    }
}
