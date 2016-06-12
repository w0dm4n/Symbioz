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

namespace Symbioz.World.Models.Exchanges
{
    public class BankExchange : Exchange
    {
        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.STORAGE;
            }
        }
        
        WorldClient Client { get; set; }
        public List<BankItemRecord> BankItems { get { return BankItemRecord.GetCharacterItems(Client.Account.Id); } }
        public BankExchange(WorldClient client)
        {
            this.Client = client;
        }
        public void OpenPanel()
        {
            Client.Send(new ExchangeStartedWithStorageMessage((sbyte)ExchangeTypeEnum.STORAGE, 300));
            Client.Send(new StorageInventoryContentMessage(BankItems.GetObjects(), Client.Account.Informations.BankKamas));
            Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Character.ExchangeType = ExchangeType;
        }
        public override void MoveItem(uint uid, int quantity)
        {
     
            if (quantity > 0)
            {
                AddItemToPanel(Client.Character.Inventory.GetItem(uid),(uint)quantity);
            }
            else
            {
                RemoveItemFromPanel(BankItems.Find(x => x.UID == uid), quantity);
            }
        }

        public override void Ready(bool ready, ushort step)
        {
            throw new NotImplementedException();
        }

        public override void CancelExchange()
        {
            Client.Character.BankInstance = null;
            Client = null;
        }
        public void MoveKamas(int amount)
        {
            Client.Account.Informations.BankKamas += (uint)amount;
            Client.Send(new StorageKamasUpdateMessage((int)Client.Account.Informations.BankKamas));
            if (amount < 0)
                Client.Character.AddKamas(-amount);
            else
                Client.Character.RemoveKamas(amount);
            SaveTask.UpdateElement(Client.Account.Informations);
        }
        public override void AddItemToPanel(CharacterItemRecord obj, uint quantity)
        {
            if (obj.IsNull())  // il ajoute a son propre panel
                return;
            if (quantity == obj.Quantity)  
            {
                var existing = BankItems.ExistingItem(obj);
                if (!existing.IsNull())
                {
                    existing.Quantity += quantity;
                    SaveTask.UpdateElement(existing);
                    Client.Character.Inventory.RemoveItem(obj.UID,obj.Quantity);
                    Client.Send(new StorageObjectUpdateMessage(existing.GetObjectItem()));
                    return;
                }
                else
                {
                    var newBankItem = obj.GetBankItem(Client.Account.Id);
                    Client.Send(new StorageObjectUpdateMessage(newBankItem.GetObjectItem()));
                    SaveTask.AddElement(newBankItem);
                    Client.Character.Inventory.RemoveItem(obj.UID, obj.Quantity);
                    return;
                }
            }
            else 
            {
                var existing = BankItems.ExistingItem(obj);
                if (existing != null)
                {
                    existing.Quantity += (uint)quantity;
                    Client.Send(new StorageObjectUpdateMessage(existing.GetObjectItem())); 
                    Client.Character.Inventory.RemoveItem(obj.UID, quantity);
                    SaveTask.UpdateElement(existing);
                    return;
                }
                else
                {
                    var addedItem = obj.CloneWithUID(); // fonctionne
                    addedItem.Quantity = (uint)quantity;
                    Client.Send(new StorageObjectUpdateMessage(addedItem.GetObjectItem()));
                    SaveTask.AddElement(addedItem.GetBankItem(Client.Account.Id));
                    Client.Character.Inventory.RemoveItem(obj.UID, quantity);
                    return;
                }
            }
        }

        public void RemoveItemFromPanel(BankItemRecord obj, int quantity)
        {
            if (obj.IsNull())
                return;
            if (obj.Quantity == (uint)-quantity)
            {
                SaveTask.RemoveElement(obj);
                var existing = Client.Character.Inventory.Items.ExistingItem(obj);
                if (existing != null)
                {
                    existing.Quantity += (uint)-quantity;
                    SaveTask.UpdateElement(existing);
                    Client.Character.Inventory.Refresh();
                }
                else
                {
                    Client.Character.Inventory.Add(obj);
                }
                Client.Send(new StorageObjectRemoveMessage(obj.UID));
                
            }
            else
            {
                obj.Quantity = (uint)(obj.Quantity + quantity);
                SaveTask.UpdateElement(obj);
                Client.Character.Inventory.Add(obj,(uint)-quantity);
                Client.Send(new StorageObjectUpdateMessage(obj.GetObjectItem()));
            }
        }
    }
}
