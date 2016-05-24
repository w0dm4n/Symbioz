using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class PrismsHandler
    {
        [MessageHandler]
        public static void HandlePrismList(PrismsListRegisterMessage message,WorldClient client)
        {
            client.Send(new PrismsListMessage(new PrismSubareaEmptyInfo[0]));
        }
    }
}
