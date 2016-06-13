using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.ORM;
using Symbioz.World.Handlers;
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.World.Records.Alliances.Prisms.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges
{
    public class PrismExchange : Exchange
    {
        private ushort[] PrismModulesIds = new ushort[] { 14552 };

        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.STORAGE;
            }
        }

        private PrismRecord Prism { get; set; }
        private WorldClient Client { get; set; }

        public List<PrismModuleRecord> PrismModules { get { return PrismModuleRecord.GetPrismModules(this.Prism.Id); } }

        public PrismExchange(PrismRecord prismRecord, WorldClient client)
        {
            this.Prism = prismRecord;
            this.Client = client;
        }

        public void OpenPanel()
        {
            Client.Send(new ExchangeStartedWithStorageMessage((sbyte)ExchangeTypeEnum.STORAGE, 300));
            Client.Send(new StorageInventoryContentMessage(PrismModules.GetObjects(), 0));
            Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Character.ExchangeType = this.ExchangeType;
        }

        public override void MoveItem(uint uid, int quantity)
        {
            if (quantity > 0)
            {
                AddItemToPanel(Client.Character.Inventory.GetItem(uid), 1);
            }
            else
            {
                RemoveItemFromPanel(PrismModules.Find(x => x.UID == uid), 1);
            }
        }

        public override void Ready(bool ready, ushort step)
        {
            throw new NotImplementedException();
        }

        public override void CancelExchange()
        {
            Client.Character.PrismStorageInstance = null;
            Client = null;
        }

        public override void AddItemToPanel(CharacterItemRecord obj, uint quantity)
        {
            if (obj.IsNull())
                return;

            if (PrismModulesIds.Contains(obj.GID))
            {
                var existing = PrismModules.ExistingItem(obj);
                if (existing.IsNull())
                {
                    var newPrismModule = obj.GetPrismModule(this.Prism.Id);
                    Client.Send(new StorageObjectUpdateMessage(newPrismModule.GetObjectItem()));
                    SaveTask.AddElement(newPrismModule, false);
                    Client.Character.Inventory.RemoveItem(obj.UID, obj.Quantity);
                }
                else
                {
                    Client.Character.Reply("Vous ne pouvez pas associer plus d'une fois le même module sur un prisme.", Color.Orange);
                }
            }
            else
            {
                Client.Character.Reply("Cet objet ne fait pas partie des catégories acceptées par ce prisme.", Color.Orange);
            }
            this.Refresh();
        }

        public void RemoveItemFromPanel(PrismModuleRecord obj, int quantity)
        {
            if (obj.IsNull())
                return;

            SaveTask.RemoveElement(obj, false);
            var existing = Client.Character.Inventory.Items.ExistingItem(obj);
            if (existing != null)
            {
                existing.Quantity += (uint)-quantity;
                SaveTask.UpdateElement(existing, false);
                Client.Character.Inventory.Refresh();
            }
            else
            {
                Client.Character.Inventory.Add(obj);
            }
            Client.Send(new StorageObjectRemoveMessage((uint)obj.UID));
            this.Refresh();
        }

        private void Refresh()
        {
            this.Prism.RefreshOnMapInstance();
            PrismsHandler.SendPrismListUpdateMessage(true, this.Prism.AllianceId);
        }
    }
}
