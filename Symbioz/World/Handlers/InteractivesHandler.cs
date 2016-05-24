using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Provider;
using Symbioz.World.Models.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class InteractivesHandler
    {
        [MessageHandler]
        public static void HandleInteractiveUse(InteractiveUseRequestMessage message,WorldClient client)
        {
            var interactive = InteractiveRecord.GetInteractive(message.elemId);
            InteractiveActionProvider.Handle(client, interactive);
        }
        [MessageHandler]
        public static void HandleZaapSave(ZaapRespawnSaveRequestMessage message,WorldClient client)
        {
            client.Character.Record.SpawnPointMapId = client.Character.Map.Id;
            client.Send(new ZaapRespawnUpdatedMessage(client.Character.Record.SpawnPointMapId));
        }
    }
}
