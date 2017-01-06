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
                AddItemToPanel(Client.Character.Inventory.GetItem(uid), (uint)quantity);
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
            if (amount < 0)
            {
                if ((-amount) <= Client.Account.Informations.BankKamas)
                {
                    Client.Character.AddKamas(-amount);
                    Client.Account.Informations.BankKamas = Client.Account.Informations.BankKamas - (uint)(-amount);
                }
                else
                {
                    Client.SendRaw("hibernate");
                }           
            }
            else
            {
                if (Client.Character.Record.Kamas >= amount)
                {
                    Client.Character.RemoveKamas(amount);
                    Client.Account.Informations.BankKamas += (uint)amount;
                }
                else
                {
                    Client.SendRaw("hibernate");
                }
            }
            Client.Send(new StorageKamasUpdateMessage((int)Client.Account.Informations.BankKamas));
            SaveTask.UpdateElement(Client.Account.Informations, this.Client.CharacterId);
        }
        public override void AddItemToPanel(CharacterItemRecord obj, uint quantity)
        {
            if (obj == null)  // il ajoute a son propre panel
                return;
            if (quantity == obj.Quantity)
            {
                var existing = BankItems.ExistingItem(obj);
                if (existing != null)
                {
                    existing.Quantity += quantity;
                    SaveTask.UpdateElement(existing, this.Client.CharacterId);
                    Client.Character.Inventory.RemoveItem(obj.UID, obj.Quantity);
                    Client.Send(new StorageObjectUpdateMessage(existing.GetObjectItem()));
                    return;
                }
                else
                {
                    var newBankItem = obj.GetBankItem(Client.Account.Id, (int)quantity);
                    Client.Send(new StorageObjectUpdateMessage(newBankItem.GetObjectItem()));
                    SaveTask.AddElement(newBankItem, this.Client.CharacterId);
                    Client.Character.Inventory.RemoveItem(obj.UID, obj.Quantity);
                    return;
                }
            }
            else
            {
                var existing = BankItems.ExistingItem(obj);
                if (existing != null)
                {
                    if (quantity <= obj.Quantity)
                    {
                        existing.Quantity += (uint)quantity;
                        Client.Send(new StorageObjectUpdateMessage(existing.GetObjectItem()));
                        Client.Character.Inventory.RemoveItem(obj.UID, quantity);
                        SaveTask.UpdateElement(existing, this.Client.CharacterId);
                        return;
                    }
                }
                else
                {
                    if (quantity <= obj.Quantity)
                    {
                        var addedItem = obj.CloneWithUID();
                        addedItem.Quantity = (uint)quantity;
                        Client.Send(new StorageObjectUpdateMessage(addedItem.GetObjectItem()));
                        SaveTask.AddElement(addedItem.GetBankItem(Client.Account.Id), this.Client.CharacterId);
                        Client.Character.Inventory.RemoveItem(obj.UID, quantity);
                        return;
                    }
                }
            }
        }

        public void RemoveItemFromPanel(BankItemRecord obj, int quantity)
        {
            if (obj.IsNull() || obj == null)
                return;
            //if (obj.Quantity == nbrItem)
            //{
               
                var existing = Client.Character.Inventory.Items.ExistingItem(obj);
                if (existing != null)
                {
                    existing.Quantity += obj.Quantity;
                    SaveTask.UpdateElement(existing, this.Client.CharacterId);
                    Client.Character.Inventory.Refresh();
                }
                else
                {
                    Client.Character.Inventory.Add(obj, obj.Quantity);
                }
                SaveTask.RemoveElement(obj, this.Client.CharacterId);
                Client.Send(new StorageObjectRemoveMessage(obj.UID));

            /*}
            else
            {
                var nbr = -quantity;
                var total = obj.Quantity - nbr;
                if (total > 0)
                {
                    obj.Quantity = obj.Quantity - (uint)nbr;
                    SaveTask.UpdateElement(obj, this.Client.CharacterId);
                    Client.Character.Reply("GIVE u " + nbr);
                    Client.Character.Inventory.Add(obj, (uint)nbr);
                    Client.Send(new StorageObjectUpdateMessage(obj.GetObjectItem()));
                }
            }*/

        }
    }
}
