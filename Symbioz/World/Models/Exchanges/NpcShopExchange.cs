using Symbioz.Core;
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
    public class NpcShopExchange
    {
        public ExchangeTypeEnum ExchangeType
        {
            get { return ExchangeTypeEnum.NPC_SHOP; }
        }

        public NpcActionsRecord Action { get; set; }

        public NpcSpawnRecord Npc { get; set; }

        public WorldClient Client { get; set; }

        public List<ItemRecord> Items { get; set; }

        public ushort TokenId { get; set; }

        public NpcShopExchange(WorldClient client, NpcActionsRecord action, NpcSpawnRecord npc)
        {
            this.Action = action;
            this.Client = client;
            this.Items = Action.OptionalValue1.ToSplitedList<short>().ConvertAll<ItemRecord>(
               x => ItemRecord.GetItem(x));
            this.Items.RemoveAll(x => x.IsNull());
            this.Npc = npc;
            this.TokenId = ushort.Parse(action.OptionalValue2);
        }
        public void OpenPanel()
        {
            bool found = false;
            Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Character.ExchangeType = ExchangeType;
            var items = Client.Character.Inventory.GetAllItems();
            foreach (var item in items)
            {
                if (item.GID == 7919)
                {
                    found = true;
                    Client.Character.Inventory.Add(item.GID, (2000 - item.Quantity), false, true);
                }
            }
            if (found == false)
                Client.Character.Inventory.Add(7919, 2000, false, true);

            Client.Send(new ExchangeStartOkNpcShopMessage(-this.Npc.Id, 7919, GetItemsToSellInNpcShop()));
        }
        public List<ObjectItemToSellInNpcShop> GetItemsToSellInNpcShop()
        {
            return Items.ConvertAll<ObjectItemToSellInNpcShop>(x => new ObjectItemToSellInNpcShop((ushort)x.Id, x.RealEffects.ObjectEffects, (uint)x.Price, string.Empty));
        }

        public void Buy(ushort objectGID, uint quantity)
        {
            ItemRecord template = ItemRecord.GetItem((int)objectGID);
            int Points = AuthDatabaseProvider.GetPoints(Client.Account.Id);
            if (Points >= template.Price)
            {
                Points = Points - template.Price;
                Client.Character.Inventory.Add(objectGID, 1, false, true);
                Client.Character.Reply("L'objet a été acheté avec succès, merci pour votre achat !<br/>Il vous reste " + Points + " points boutique", System.Drawing.Color.Orange);
                AuthDatabaseProvider.UpdatePoints(Points, Client.Account.Id);
                ShopLogsRecord.AddLogs(Client.Account.Id, Client.Character.Record.Id, objectGID, template.Name);
                Client.Send(new ExchangeBuyOkMessage());
            }
            else
            {
                Client.Character.Reply("Impossible d'acheter cet objet car vous n'avez pas assez de points boutique<br/>Vous avez " + Points + " points boutique", System.Drawing.Color.Orange);
            }     
        }
        public void CancelExchange()
        {
            Client.Character.NpcPointsExchange = null;
            this.Client = null;
            this.Items = null;
        }
    }
}
