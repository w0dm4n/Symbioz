using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.Providers;
using Symbioz.World.Records;
using Symbioz.World.Records.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    public class InventoryHandler
    {
        [MessageHandler]
        public static void HandleObjectUse(ObjectUseMessage message, WorldClient client)
        {
            var item = client.Character.Inventory.GetItem(message.objectUID);
            if (item != null)
            {
                var template = ItemRecord.GetItem(item.GID);
                if (template == null)
                    return;

                if (CustomObjectUseHandler.CustomHandlerExistForItemId(item.GID))
                {
                    CustomObjectUseHandler.HandleByItemGID(client, item);
                    client.Character.RefreshShortcuts();
                    return;
                }
                else if (CustomObjectUseHandler.CustomHandlerExistForItemTypeId(item))
                {
                    CustomObjectUseHandler.HandleByItemTypeId(client, item);
                    client.Character.RefreshShortcuts();
                    return;
                }
                else if (ItemUseEffectsProvider.HandleEffects(client, item))
                {
                    client.Character.Inventory.RemoveItem(item.UID, 1);
                    client.Character.RefreshShortcuts();
                }
            }
        }

        [MessageHandler]
        public static void HandleObjectUseMultiple(ObjectUseMultipleMessage message, WorldClient client)
        {
            var item = client.Character.Inventory.GetItem(message.objectUID);
            var template = ItemRecord.GetItem(item.GID);
            if (template == null)
                return;

            bool handledByCustomHandler = false;

            if (CustomObjectUseHandler.CustomHandlerExistForItemId(item.GID))
            {
                handledByCustomHandler = true;
                CustomObjectUseHandler.HandleByItemGID(client, item, (int)message.quantity);
                client.Character.RefreshShortcuts();
                return;
            }
            else if (CustomObjectUseHandler.CustomHandlerExistForItemTypeId(item))
            {
                handledByCustomHandler = true;
                CustomObjectUseHandler.HandleByItemTypeId(client, item, (int)message.quantity);
                client.Character.RefreshShortcuts();
                return;
            }
            else
            {
                for (int i = 0; i < message.quantity; i++)
                {
                    ItemUseEffectsProvider.HandleEffects(client, item);
                }
            }

            if(!handledByCustomHandler)
            {
                client.Character.Inventory.RemoveItem(item.UID, message.quantity);
            }
        }

        [MessageHandler]
        public static void HandleObjectSetPosition(ObjectSetPositionMessage message, WorldClient client)
        {
            if (client.Character.ShopStockInstance == null && client.Character.Trader == null)
                client.Character.Inventory.MoveItem(message.objectUID, message.position, message.quantity);
        }

        [MessageHandler]
        public static void HandleObjectDelete(ObjectDeleteMessage message, WorldClient client)
        {
            client.Character.Inventory.RemoveItem(message.objectUID, message.quantity);
        }
    }
}
