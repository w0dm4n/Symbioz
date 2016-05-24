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
        private static Dictionary<NpcActionTypeEnum, Action<WorldClient, NpcSpawnRecord, NpcActionsRecord>> NpcActions = new Dictionary<NpcActionTypeEnum, Action<WorldClient, NpcSpawnRecord, NpcActionsRecord>>();
        [StartupInvoke(StartupInvokeType.Others)]
        public static void LoadHandlers()
        {
            NpcActions.Add(NpcActionTypeEnum.ACTION_TALK, Talk);
            NpcActions.Add(NpcActionTypeEnum.ACTION_BUY, Buy);
            NpcActions.Add(NpcActionTypeEnum.ACTION_SELL, Sell);
            NpcActions.Add(NpcActionTypeEnum.ACTION_BUY_SELL, BuySell);
        }
        public static void Handle(WorldClient client, NpcSpawnRecord npc, sbyte clientnpcactionid)
        {
            var actionType = (NpcActionTypeEnum)clientnpcactionid;
            var handler = NpcActions.FirstOrDefault(x => x.Key == actionType);

            if (handler.Value != null)
            {
                var action = NpcActionsRecord.GetNpcAction(npc.Id, actionType);
                if (action != null)
                    handler.Value(client, npc, action);
                else
                    client.Character.NotificationError("Unable to find npc action record (" + clientnpcactionid + ") for Npc " + npc.Id);

            }
            else
            {
                client.Character.NotificationError("Unable to find npc generic action handler (" + clientnpcactionid + ") for Npc " + npc.Id);
            }
        }
        static void Buy(WorldClient client, NpcSpawnRecord npc, NpcActionsRecord action)
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
                client.Character.NotificationError("Cet hotel de vente n'est pas encore disponible!");
        }

        static void Sell(WorldClient client, NpcSpawnRecord npc, NpcActionsRecord action)
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
                client.Character.Reply("Cet hotel de vente n'est pas encore disponible!");
        }
        static void BuySell(WorldClient client, NpcSpawnRecord npc, NpcActionsRecord action)
        {
            client.Character.NpcShopExchange = new NpcBuySellExchange(client,action,npc);
            client.Character.NpcShopExchange.OpenPanel();

           
        }
        static void Talk(WorldClient client, NpcSpawnRecord npc, NpcActionsRecord action)
        {
            if (action == null)
            {
                client.Character.Reply("Ce PNJ n'est pas apte a parler.");
                return;
            }
            ushort messageId =ushort.Parse(action.OptionalValue1);

            List<NpcReplyRecord> replies = NpcsRepliesProvider.GetPossibleReply(client, NpcReplyRecord.GetNpcReplies(messageId));
            client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_DIALOG;

            client.Send(new NpcDialogCreationMessage(npc.MapId, -npc.Id));

            client.Send(new NpcDialogQuestionMessage(messageId, new string[] { "0" }, replies.ConvertAll<ushort>(x=>(ushort)x.ReplyId)));
        }
    }
}
