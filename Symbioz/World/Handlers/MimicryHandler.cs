using Symbioz.Network.Messages;
using Symbioz.DofusProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Network.Clients;
using Symbioz.World.Models;
using Symbioz.World.Records;
using Symbioz.Enums;
using Symbioz.ORM;

namespace Symbioz.World.Handlers
{
    class MimicryHandler
    {


        [MessageHandler]
        public static void HandleFeedAndAssociate(MimicryObjectFeedAndAssociateRequestMessage message, WorldClient client)
        {
            CharacterItemRecord hostItem = client.Character.Inventory.GetItem(message.hostUID);
            CharacterItemRecord foodItem = client.Character.Inventory.GetItem(message.foodUID);

            if (hostItem.GetTemplate().Type == ItemTypeEnum.PET || foodItem.GetTemplate().Type == ItemTypeEnum.PET)
            {
                client.Character.Reply("Impossible de mimibioter les familiers.");
                return;
            }
            if (hostItem.ContainEffect(EffectsEnum.Eff_Mimicry) || foodItem.ContainEffect(EffectsEnum.Eff_Mimicry))
            {
                client.Character.Reply("Impossible d'associer ces objets car l'un d'eux possède déja un mimibiote.");
                return;
            }
            if (hostItem.GID == foodItem.GID)
            {
                client.Character.Reply("Impossible d'associer ces objets car ce sont les mêmes...");
                return;
            }
            if (hostItem.GetTemplate().Type != foodItem.GetTemplate().Type)
            {
                client.Character.Reply("Impossible d'associer ces objets car ils ne sont pas du même types");
                return;
            }
            if (hostItem.GetTemplate().Level < foodItem.GetTemplate().Level)
            {
                client.Character.Reply("Impossible d'associer ces objets car le niveau de l'item d'appearence est inferieur au niveau de l'item de base.");
                return;
            }
            CharacterItemRecord mimicry = hostItem.ToMimicry(foodItem.GID);
            if (message.preview)
            {
                client.Send(new MimicryObjectPreviewMessage(mimicry.GetObjectItem()));
            }
            else
            {
                client.Character.Inventory.RemoveItem(message.hostUID, 1);
                client.Character.Inventory.RemoveItem(message.foodUID, 1);
                client.Character.Inventory.RemoveItem(message.symbioteUID, 1);
                client.Character.Inventory.Add(mimicry);
                client.Send(new MimicryObjectAssociatedMessage(mimicry.UID));
            }
        }
        [MessageHandler]
        public static void HandleMimicryObjectErase(MimicryObjectEraseRequestMessage message, WorldClient client)
        {
            CharacterItemRecord item = client.Character.Inventory.GetItem(message.hostUID);
            if (message.hostPos != 63)
            {
                client.Character.Inventory.UnequipItem(item, 63, item.GetTemplate(), item.Quantity);
                client.Character.RefreshOnMapInstance();
                client.Character.RefreshStats();
            }
           
            item.RemoveAllEffect(EffectsEnum.Eff_Mimicry);
            var newItem = item.CloneAndGetNewUID();
            client.Character.Inventory.RemoveItem(item.UID, item.Quantity,false);
            client.Character.Inventory.Add(newItem);
        }
    }
}
