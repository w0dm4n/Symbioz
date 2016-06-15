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
    class ObjectOnCellHandler
    {
        [MessageHandler]
        public static void HandleObjectUseOnCellMessage(ObjectUseOnCellMessage message, WorldClient client)
        {
            var itemRecord = CharacterItemRecord.GetItemByUID(message.objectUID);
            if (itemRecord != null)
            {
                if (itemRecord.GID == 7401) // Parchemin de recherche
                    client.Character.Reply("Vous devez cibler un joueur pour pouvoir le traquer !");
                else
                    client.Character.Reply("Impossible pour le moment");
            }
        }

        private static void StopDelayedAnimations(WorldClient client, WorldClient target, System.Timers.Timer _timer, CharacterItemRecord itemRecord)
        {
            if (target.Character.CurrentlyInTrackRequest)
            {
                client.Character.SendEndDelayedMessageToMap(target.Character.Record.Id, DelayedActionTypeEnum.DELAYED_ACTION_OBJECT_USE);
                target.Character.Reply("<b>" + client.Character.Record.Name + "</b>" + " détient désormais un parchemin de recherche à votre nom !");
                List<ObjectEffect> objectEffects = new List<ObjectEffect>();
                objectEffects.Add(new ObjectEffectString(989, target.Character.Record.Name));
                CharacterItemRecord newItem = new CharacterItemRecord(CharacterItemRecord.PopNextUID(), 63, 7400, client.Character.Id, 1, objectEffects);
                client.Character.Inventory.Add(newItem);
                client.Character.Reply("Vous avez obtenu 1 <b>Parchemin lié !</b>");
                client.Character.Inventory.RemoveItem(itemRecord.UID, 1);
                TracksRecord.AddTracked(target.Character.Record.Id, (int)newItem.UID);
                target.Character.CurrentlyInTrackRequest = false;
            }
            else
                client.Character.Reply("L'action de traque a été annulé car <b>" + target.Character.Record.Name + "</b> a effectué une action !");
            client.Character.isTracking = false;
            _timer.Enabled = false;
        }

        [MessageHandler]
        public static void HandleObjectUseOnCharacterMessage(ObjectUseOnCharacterMessage message, WorldClient client)
        {
            if (client.Character.isTracking)
            {
                client.Character.Reply("Impossible de lancer deux parchemins de recherche en même temps.");
                return;
            }
            var target = WorldServer.Instance.GetOnlineClient((int)message.characterId);
            if (target != null && target != client)
            {
                var itemRecord = CharacterItemRecord.GetItemByUID(message.objectUID);
                if (itemRecord != null)
                {
                    if (itemRecord.GID == 7401) // Parchemin de recherche
                    {
                        client.Character.SendStartDelayedMessageToMap(target.Character.Record.Id, DelayedActionTypeEnum.DELAYED_ACTION_OBJECT_USE, 5, itemRecord.GID);
                        target.Character.CurrentlyInTrackRequest = true;
                        client.Character.isTracking = true;

                        var _timer = new System.Timers.Timer();
                        _timer.Interval = (5000);
                        _timer.Elapsed += (sender, e) => { StopDelayedAnimations(client, target, _timer, itemRecord); };
                        _timer.Enabled = true;
                    }
                    else
                        client.Character.Reply("Impossible pour le moment.");
                }
            }
        }
    }
}
