using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges
{
    public class NpcTradeExchange
    {
        public ExchangeTypeEnum ExchangeType
        {
            get { return ExchangeTypeEnum.NPC_TRADE; }
        }

        public NpcActionsRecord Action { get; set; }

        public NpcSpawnRecord Npc { get; set; }

        public WorldClient Client { get; set; }

        public List<CharacterItemRecord> Items { get; set; }

        public List<int> itemsUID { get; set; }
        
        public int ContextualId { get; set; }

        public ushort TokenId { get; set; }

        public int Kamas { get; set;  }

        public NpcTradeExchange(WorldClient client, NpcActionsRecord action, NpcSpawnRecord npc, int ContextualId)
        {
            this.Action = action;
            this.Client = client;
            this.Items = new List<CharacterItemRecord>();
            this.itemsUID = new List<int>();
            this.Npc = npc;
            this.TokenId = ushort.Parse(action.OptionalValue2);
            this.ContextualId = ContextualId;
        }

        public void OpenExchange()
        {

            Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Character.ExchangeType = ExchangeType;
            Client.Send(new ExchangeStartOkNpcTradeMessage(this.ContextualId));
        }

        public void MoveItem(uint uid, int quantity)
        {
            if (quantity > 0)
            {
                if (this.itemsUID.Contains((int)uid))
                    return;
                var obj = Client.Character.Inventory.GetItem(uid);
                if (obj != null)
                    this.AddItemToPanel(obj, (uint)quantity);
            }
            else
                this.RemoveItemFromPanel(Client.Character.Inventory.GetItem(uid));
        }

        public void AddItemToPanel(CharacterItemRecord obj, uint quantity)
        {
            if (quantity <= obj.Quantity)
            {
                var addedItem = obj.CloneWithUID();
                var itemRecord = ItemRecord.GetItem(addedItem.GID);
                int[] TypeInventoryId = new int[] { 11, 9, 10, 1, 17, 16, 23, 82, 151, 18 };
                if (itemRecord != null)
                {
                    if (TypeInventoryId.Contains(itemRecord.TypeId))
                    {
                        Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                        itemsUID.Add((int)obj.UID);
                        Items.Add(obj);
                        this.CheckKamasAmount();
                    }
                }
            }
        }

        public void Ready(bool ready)
        {
            if (ready == true)
            {
                this.CheckKamasAmount();
                foreach (var item in this.Items)
                    Client.Character.Inventory.RemoveItem(item.UID, item.Quantity, false);
                Client.Character.AddKamas(this.Kamas, true);
                Client.Character.LeaveDialog();
            }
        }

        public void RemoveItemFromPanel(CharacterItemRecord obj)
        {
            if (obj != null)
            {
                Client.Send(new ExchangeObjectRemovedMessage(false, obj.UID));
                Items.Remove(obj);
                itemsUID.Remove((int)obj.UID);
                this.CheckKamasAmount();
            }
        }

        public void CheckKamasAmount()
        {
            int kamas = 0;
            foreach (var item in this.Items)
            {
                var itemRecord = ItemRecord.GetItem(item.GID);
                if (itemRecord.Level >= 190)
                    kamas += 200000;
                if (itemRecord.Level >= 180)
                    kamas += 100000;
                else if (itemRecord.Level >= 150)
                    kamas += 30000;
                else if (itemRecord.Level >= 120)
                    kamas += 15000;
                else if (itemRecord.Level >= 100)
                    kamas += 10000;
                else if (itemRecord.Level >= 50)
                    kamas += 5000;
                else if (itemRecord.Level >= 30)
                    kamas += 3000;
                else
                    kamas += 1500;
            }
            this.Kamas = kamas;
            Client.Send(new ExchangeKamaModifiedMessage(true, (uint)kamas));

        }

        public void CancelExchange()
        {
            Client.Character.NpcTradeExchange = null;
            this.Client = null;
            this.Items = null;
        }
    }
}
