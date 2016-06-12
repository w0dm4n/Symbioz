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
                        if (record != null)
                            NpcsActionsProvider.Handle(client, record, message.npcActionId);
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
