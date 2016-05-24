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
    public class NpcBuySellExchange
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

        public NpcBuySellExchange(WorldClient client,NpcActionsRecord action,NpcSpawnRecord npc)
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
     
            Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Character.ExchangeType = ExchangeType;

            Client.Send(new ExchangeStartOkNpcShopMessage(-this.Npc.Id,TokenId,GetItemsToSellInNpcShop()));
        }
        public List<ObjectItemToSellInNpcShop> GetItemsToSellInNpcShop()
        {
            return Items.ConvertAll<ObjectItemToSellInNpcShop>(x => new ObjectItemToSellInNpcShop((ushort)x.Id, x.RealEffects.ObjectEffects, (uint)x.Price, string.Empty));
        }

        public void Buy(ushort objectGID,uint quantity)
        {
            ItemRecord template = ItemRecord.GetItem((int)objectGID);

            if (TokenId == 0)  
            {
                if (!Client.Character.RemoveKamas((int)(template.Price * quantity), true))
                    return;
            }
            else
            {
                CharacterItemRecord item = Client.Character.Inventory.Items.Find(x => x.GID == TokenId);
                if (item.Quantity < quantity)
                {
                    Client.Character.ReplyError("Vous ne possedez pas asser de token.");
                    return;
                }
                else
                {
                    Client.Character.Inventory.RemoveItem(item.UID, (uint)(quantity * template.Price));
                }
            }
            Client.Character.Inventory.Add(objectGID, quantity);
            Client.Send(new ExchangeBuyOkMessage());
        }
        public void CancelExchange()
        {
            Client.Character.NpcShopExchange = null;
            this.Client = null;
            this.Items = null;
        }
    }
}
