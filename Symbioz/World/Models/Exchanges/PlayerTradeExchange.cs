using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges
{
    public class PlayerTradeExchange : Exchange
    {
        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.PLAYER_TRADE;
            }
        }
        public bool IsReady = false;
        public List<CharacterItemRecord> ExchangedItems = new List<CharacterItemRecord>();
        public int ExchangedKamas = 0;
        WorldClient Client { get; set; }
        WorldClient SecondTrader { get; set; }
        public PlayerTradeExchange(WorldClient client, WorldClient target)
        {
            this.Client = client;
            this.SecondTrader = target;
            this.SecondTrader.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            this.Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            this.Client.Character.ExchangeType = ExchangeType;
            this.SecondTrader.Character.ExchangeType = ExchangeType;
        }
        public void Ask()
        {
            Client.Send(new ExchangeRequestedTradeMessage((sbyte)ExchangeType, (uint)Client.Character.Id, (uint)SecondTrader.Character.Id));
            SecondTrader.Send(new ExchangeRequestedTradeMessage((sbyte)ExchangeType, (uint)Client.Character.Id, (uint)SecondTrader.Character.Id));
        }
        public override void CancelExchange()
        {
            Client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, false));
            SecondTrader.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, false));
            Client.Character.ExchangeType = null;
            SecondTrader.Character.ExchangeType = null;
            Client.Character.PlayerTradeInstance = null;
            SecondTrader.Character.PlayerTradeInstance = null;
            Client = null;
            SecondTrader = null;
            ExchangedItems = null;
        }
        public void AcceptExchange()
        {
            Client.Send(new ExchangeStartedWithPodsMessage((sbyte)ExchangeType, Client.Character.Id, (uint)Client.Character.Inventory.GetWeight(), 5000,
                SecondTrader.Character.Id, SecondTrader.Character.Inventory.GetWeight(), 5000));
            SecondTrader.Send(new ExchangeStartedWithPodsMessage((sbyte)ExchangeType, Client.Character.Id, (uint)Client.Character.Inventory.GetWeight(), 5000,
               SecondTrader.Character.Id, SecondTrader.Character.Inventory.GetWeight(), 5000));
        }

        public override void MoveItem(uint uid, int quantity)
        {
            CharacterItemRecord obj;
            if (quantity > 0)
            {
                obj = Client.Character.Inventory.GetItem(uid);
                if (ExchangedItems.Find(x => x.UID == uid) != null)
                    return;
                else
                    AddItemToPanel(obj, (uint)quantity);
            }
            else
            {
                obj = ExchangedItems.Find(x => x.UID == uid);
                RemoveItemFromPanel(obj, quantity);
            }
        }
        public override void AddItemToPanel(CharacterItemRecord obj, uint quantity)
        {
            if (quantity == obj.Quantity)
            {
                var addedItem = obj.CloneWithUID();
                Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                SecondTrader.Send(new ExchangeObjectAddedMessage(true, addedItem.GetObjectItem()));
                ExchangedItems.Add(addedItem);
                return;
            }
            else
            {
                var existing = ExchangedItems.ExistingItem(obj);
                if (existing != null)
                {

                    existing.Quantity += (uint)quantity;
                    Client.Send(new ExchangeObjectModifiedMessage(false, existing.GetObjectItem()));
                    SecondTrader.Send(new ExchangeObjectModifiedMessage(true, existing.GetObjectItem()));
                    return;
                }
                else
                {
                    var addedItem = obj.CloneWithUID();
                    addedItem.Quantity = (uint)quantity;
                    Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                    SecondTrader.Send(new ExchangeObjectAddedMessage(true, addedItem.GetObjectItem()));
                    ExchangedItems.Add(addedItem);
                    return;
                }
            }
        }
        public override void RemoveItemFromPanel(CharacterItemRecord obj, int quantity)
        {
            if (obj.Quantity == (uint)-quantity)
            {
                Client.Send(new ExchangeObjectRemovedMessage(false, obj.UID));
                SecondTrader.Send(new ExchangeObjectRemovedMessage(true, obj.UID));
                ExchangedItems.Remove(obj);
                return;
            }
            else
            {
                obj.Quantity = (uint)(obj.Quantity + quantity);
                Client.Send(new ExchangeObjectModifiedMessage(false, obj.GetObjectItem()));
                SecondTrader.Send(new ExchangeObjectModifiedMessage(true, obj.GetObjectItem()));
            }
        }
        public void MoveKamas(int quantity)
        {
            SecondTrader.Send(new ExchangeKamaModifiedMessage(true, (uint)quantity));
            ExchangedKamas = quantity;

        }
        public void Abort()
        {
            SecondTrader.Character.LeaveDialog();
            Client.Character.LeaveDialog();
        }
        public override void Ready(bool ready, ushort step)
        {
            IsReady = ready;
            Client.Send(new ExchangeIsReadyMessage((uint)Client.Character.Id, ready));
            SecondTrader.Send(new ExchangeIsReadyMessage((uint)Client.Character.Id, ready));
            if (IsReady && SecondTrader.Character.PlayerTradeInstance.IsReady)
            {
                foreach (var item in ExchangedItems)
                {
                    item.CharacterId = SecondTrader.Character.Id;
                    SecondTrader.Character.Inventory.Add(item.CloneAndGetNewUID());
                    Client.Character.Inventory.RemoveItem(item.UID, item.Quantity);
                }

                foreach (var item in SecondTrader.Character.PlayerTradeInstance.ExchangedItems)
                {
                    item.CharacterId = Client.Character.Id;
                    Client.Character.Inventory.Add(item.CloneAndGetNewUID());
                    SecondTrader.Character.Inventory.RemoveItem(item.UID, item.Quantity);
                }

                SecondTrader.Character.AddKamas(ExchangedKamas);
                Client.Character.RemoveKamas(ExchangedKamas);

                Client.Character.AddKamas(SecondTrader.Character.PlayerTradeInstance.ExchangedKamas);
                SecondTrader.Character.RemoveKamas(SecondTrader.Character.PlayerTradeInstance.ExchangedKamas);
                SecondTrader.Character.LeaveDialog();
                Client.Character.LeaveDialog();

                Client.Character.Reply("Echange Effectué");
                SecondTrader.Character.Reply("Echange Effectué");

            }
        }
    }
}
