using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Providers;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class InventoryHandler
    {
        public static void HandleBread(ObjectUseMessage message, WorldClient client)
        {
            var item = client.Character.Inventory.GetItem(message.objectUID);
            if (item == null)
                return;
            var template = ItemRecord.GetItem(item.GID);
            if (template == null)
                return;
            if (client.Character.CurrentStats.LifePoints >= client.Character.StatsRecord.LifePoints)
            {
                client.Character.Reply("Vos points de vie sont déjà au maximum !");
                return;
            }
            var lifeBack = 0;
            if (client.Character.CurrentStats.LifePoints <= client.Character.StatsRecord.LifePoints)
            {
                var effects = item.Effects.Split('|');
                foreach (var effect in effects)
                {
                    var current = effect.Split(new string[] { "70#110#" }, StringSplitOptions.None);
                    if (current != null && current.Length == 2)
                        lifeBack += int.Parse(current[1]);
                }
            }
            client.Character.CurrentStats.LifePoints += (uint)lifeBack;
            if (client.Character.CurrentStats.LifePoints >= client.Character.StatsRecord.LifePoints)
            {
                client.Character.CurrentStats.LifePoints = (uint)client.Character.StatsRecord.LifePoints;
                client.Character.Reply("Tous vos points de vie ont été restaurés !");
            }
            else
                client.Character.Reply("Vous avez récupéré " + lifeBack + " points de vie !");
            client.Character.Record.CurrentLifePoint = client.Character.CurrentStats.LifePoints;
            client.Character.RefreshStats();
            client.Character.Inventory.RemoveItem(item.UID, 1, true);
        }

        [MessageHandler]
        public static void HandleObjectUse(ObjectUseMessage message, WorldClient client)
        {
            var item = client.Character.Inventory.GetItem(message.objectUID);
            if (item != null)
            {
                var template = ItemRecord.GetItem(item.GID);
                if (template == null)
                    return;
                if (template.TypeId == 33) // PAIN
                {
                    HandleBread(message, client);
                    return;
                }
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
            var template = ItemRecord.GetItem(item.GID);
            if (template == null)
                return;
            for (int i = 0; i < message.quantity; i++)
            {
                if (template.TypeId == 33) // PAIN
                    HandleBread(message, client);
                else
                    ItemUseEffectsProvider.HandleEffects(client, item);
            }
            if (template.TypeId != 33)
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
