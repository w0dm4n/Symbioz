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
using Symbioz.Network.Servers;
using Symbioz.World.Models;
using Symbioz.Enums;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Symbioz.World.Records.Tracks;
using Symbioz.DofusProtocol.Types;
using Shader.Helper;

namespace Symbioz.World.Handlers
{
    public class TargetableObjectHandler
    {
        [MessageHandler]
        public static void HandleObjectUseOnCellMessage(ObjectUseOnCellMessage message, WorldClient client)
        {
            var item = client.Character.Inventory.GetItem(message.objectUID);
            if (item != null)
            {
                var template = ItemRecord.GetItem(item.GID);
                if (template == null)
                    return;

                if (CustomTargetableObjectHandler.CustomHandlerExistForItemId(item.GID))
                {
                    CustomTargetableObjectHandler.HandleByItemGID(client, item, (int)message.cells, true);
                    client.Character.RefreshShortcuts();
                    return;
                }
            }
        }

        [MessageHandler]
        public static void HandleObjectUseOnCharacterMessage(ObjectUseOnCharacterMessage message, WorldClient client)
        {
            var item = client.Character.Inventory.GetItem(message.objectUID);
            if (item != null)
            {
                var template = ItemRecord.GetItem(item.GID);
                if (template == null)
                    return;

                if (CustomTargetableObjectHandler.CustomHandlerExistForItemId(item.GID))
                {
                    CustomTargetableObjectHandler.HandleByItemGID(client, item, (int)message.characterId);
                    client.Character.RefreshShortcuts();
                    return;
                }
            }
        }
    }
}
