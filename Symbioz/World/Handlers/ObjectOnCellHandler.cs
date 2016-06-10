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

namespace Symbioz.World.Handlers
{
    class ObjectOnCellHandler
    {
        [MessageHandler]
        public static void HandleObjectUseOnCellMessage(ObjectUseOnCellMessage message, WorldClient client)
        {
            client.Character.Reply("Coucou");
        }
    }
}
