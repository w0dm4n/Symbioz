using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    public class PvPHandler
    {
        [MessageHandler]
        public static void SetEnlablePvP(SetEnablePVPRequestMessage message,WorldClient client)
        {
            client.Character.TooglePvPMode(message.enable);  
        }
    }
}
