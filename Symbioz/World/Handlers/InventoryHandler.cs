using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class InventoryHandler
    {

        [MessageHandler]
        public static void HandleObjectUse(ObjectUseMessage message, WorldClient client)
        {

            var item = client.Character.Inventory.GetItem(message.objectUID);
            if (item != null)
            {
                if (CustomObjectUseHandler.CustomHandlerExist(item.GID))
                {
                    CustomObjectUseHandler.Handle(client, item);
                    client.Character.RefreshShortcuts();
                    return;
                }
                if (ItemUseEffectsProvider.HandleEffects(client, item))
                    client.Character.Inventory.RemoveItem(item.UID, 1);
                client.Character.RefreshShortcuts();
            }
        }
        [MessageHandler]
        public static void HandleObjectUseMultiple(ObjectUseMultipleMessage message, WorldClient client)
        {
            var item = client.Character.Inventory.GetItem(message.objectUID);
            for (int i = 0; i < message.quantity; i++)
            {
                ItemUseEffectsProvider.HandleEffects(client, item);
            }
            client.Character.Inventory.RemoveItem(item.UID, message.quantity);
        }
        [MessageHandler]
        public static void HandleObjectSetPosition(ObjectSetPositionMessage message, WorldClient client)
        {
            client.Character.Inventory.MoveItem(message.objectUID, message.position, message.quantity);
        }
        [MessageHandler]
        public static void HandleObjectDelete(ObjectDeleteMessage message, WorldClient client)
        {
            client.Character.Inventory.RemoveItem(message.objectUID, message.quantity);

        }
    }
}
