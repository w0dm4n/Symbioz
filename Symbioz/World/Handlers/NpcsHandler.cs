using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Messages;
using Symbioz.Network.Clients;
using Symbioz.Providers;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.World.Models.Alliances.Prisms;
using Symbioz.World.Models.Exchanges;

namespace Symbioz.World.Handlers
{
    public class NpcsHandler
    {
        [MessageHandler]
        public static void HandleNpcGenericAction(NpcGenericActionRequestMessage message, WorldClient client)
        {
            if (client.Character.Map.Id == message.npcMapId)
            {
                switch (message.npcId)
                {
                    case PrismRecord.ConstantContextualId:
                        PrismsManager.Instance.TalkToPrism(client, message.npcMapId);
                        break;

                    default:
                        NpcSpawnRecord record = NpcSpawnRecord.GetNpcByContextualId(message.npcId);

                        if (record.Id == 560 && message.npcActionId == 3) // PNJ Trok
                        {
                            client.Character.ShowNotification("Salut, je suis Trok !<br/>Je trok tes items dont tu veux te séparer contre quelque kamas, plus ton objet sera haut niveau, plus je t'en donnerais !<br/>Par contre je t'avoue que je ne veux pas trop de tes ressources.. ça ne se vend pas trop par ici");
                            return;
                        }
                        else if ((record.Id == 561 || record.Id == 562 || record.Id == 563) && message.npcActionId == 1) // Pnj Boutique
                        {
                            var action = NpcActionsRecord.GetNpcAction(record.Id, Enums.NpcActionTypeEnum.ACTION_BUY_SELL);
                            client.Character.NpcPointsExchange = new NpcShopExchange(client, action, record);
                            client.Character.NpcPointsExchange.OpenPanel();
                            return;
                        }

                        if (record != null)
                            NpcsActionsProvider.Handle(client, record, message.npcActionId, message.npcId);
                        break;
                }
            }
        }

        [MessageHandler]
        public static void HandleNpcDialogReply(NpcDialogReplyMessage message, WorldClient client)
        {
            client.Character.LeaveDialog();
            NpcsRepliesProvider.Handle(client, NpcReplyRecord.GetNpcRepliesData(message.replyId));            
        }
    }
}
