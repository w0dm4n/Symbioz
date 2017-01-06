using Symbioz.Core.Startup;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Records;
using System;
using Symbioz.DofusProtocol.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Models;
using Symbioz.Helper;
using Symbioz.World.Models.Exchanges;

namespace Symbioz.Providers
{
    class NpcsActionsProvider
    {
        private static Dictionary<NpcActionTypeEnum, Action<WorldClient, NpcSpawnRecord, NpcActionsRecord, int>> NpcActions = new Dictionary<NpcActionTypeEnum, Action<WorldClient, NpcSpawnRecord, NpcActionsRecord, int>>();

        [StartupInvoke(StartupInvokeType.Others)]
        public static void LoadHandlers()
        {
            NpcActions.Add(NpcActionTypeEnum.ACTION_EXCHANGE, ExchangeItem);
            NpcActions.Add(NpcActionTypeEnum.ACTION_TALK, Talk);
            NpcActions.Add(NpcActionTypeEnum.ACTION_BUY, Buy);
            NpcActions.Add(NpcActionTypeEnum.ACTION_SELL, Sell);
            NpcActions.Add(NpcActionTypeEnum.ACTION_BUY_SELL, BuySell);
        }

        public static void Handle(WorldClient client, NpcSpawnRecord npc, sbyte clientnpcactionid, int ContextualId = 0)
        {
            var actionType = (NpcActionTypeEnum)clientnpcactionid;
            var handler = NpcActions.FirstOrDefault(x => x.Key == actionType);

            if (handler.Value != null)
            {
                var action = NpcActionsRecord.GetNpcAction(npc.Id, actionType);
                if (action != null)
                    handler.Value(client, npc, action, ContextualId);
                else if (client.Character.isDebugging)
                    client.Character.NotificationError("Unable to find npc action record (" + clientnpcactionid + ") for Npc " + npc.Id);

            }
            else if (client.Character.isDebugging)
            {
                client.Character.NotificationError("Unable to find npc generic action handler (" + clientnpcactionid + ") for Npc " + npc.Id);
            }
        }
        static void Buy(WorldClient client, NpcSpawnRecord npc, NpcActionsRecord action, int ContextualId = 0)
        {
            if (client.Character.BidShopInstance != null)
            {
                client.Character.BidShopInstance.OpenBuyPanel();
    
                return;
            }
            var bidshop = BidShopRecord.GetBidShop(int.Parse(action.OptionalValue1));
            if (bidshop != null)
            {
                client.Character.BidShopInstance = new BidShopExchange(client, bidshop.GetDescriptor(npc.Id), bidshop.Id);
                client.Character.BidShopInstance.OpenBuyPanel();
           
            }
            else
                client.Character.NotificationError("Hôtel de vente non disponible !");
        }

        static void Sell(WorldClient client, NpcSpawnRecord npc, NpcActionsRecord action, int ContextualId = 0)
        {
            if (client.Character.BidShopInstance != null)
            {
                client.Character.BidShopInstance.OpenSellPanel();
         
                return;
            }
            var bidshop = BidShopRecord.GetBidShop(int.Parse(action.OptionalValue1));
            if (bidshop != null)
            {
                client.Character.BidShopInstance = new BidShopExchange(client, bidshop.GetDescriptor(npc.Id), bidshop.Id);
                client.Character.BidShopInstance.OpenSellPanel();
       
            }
            else
                client.Character.Reply("Hôtel de vente non disponible !");
        }
        static void BuySell(WorldClient client, NpcSpawnRecord npc, NpcActionsRecord action, int ContextualId = 0)
        {
            if (action == null || client.Character.Restrictions.isDead == true)
            {
                client.Character.Reply("Impossible de dialoguer avec ce PNJ !");
                return;
            }
            client.Character.NpcShopExchange = new NpcBuySellExchange(client,action,npc);
            client.Character.NpcShopExchange.OpenPanel();
        }

        static void Talk(WorldClient client, NpcSpawnRecord npc, NpcActionsRecord action, int ContextualId = 0)
        {
            if (action == null || client.Character.Restrictions.cantMove == true)
            {
                client.Character.Reply("Impossible de dialoguer avec ce PNJ !");
                return;
            }
           
            ushort messageId = ushort.Parse(action.OptionalValue1);

            List<NpcReplyRecord> replies = NpcsRepliesProvider.GetPossibleReply(client, NpcReplyRecord.GetNpcReplies(messageId));
            client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_DIALOG;

            client.Send(new NpcDialogCreationMessage(npc.MapId, -npc.Id));

            client.Send(new NpcDialogQuestionMessage(messageId, new string[] { "0" }, replies.ConvertAll<ushort>(x=>(ushort)x.ReplyId)));
        }

        static void ExchangeItem(WorldClient client, NpcSpawnRecord npc, NpcActionsRecord action, int ContextualId = 0)
        {
            if (action == null || client.Character.Restrictions.isDead == true)
            {
                client.Character.Reply("Impossible de dialoguer avec ce PNJ !");
                return;
            }
            client.Character.NpcTradeExchange = new NpcTradeExchange(client, action, npc, ContextualId);
            client.Character.NpcTradeExchange.OpenExchange();
        }
    }
}
