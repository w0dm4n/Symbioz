using Symbioz.Network.Messages;
using Symbioz.DofusProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Network.Clients;

namespace Symbioz.World.Handlers
{
    class CosmeticHandler
    {
        [MessageHandler]
        public static void HandleTitleAndOrnamentsListRequest(TitlesAndOrnamentsListRequestMessage message,WorldClient client)
        {
            client.Send(new TitlesAndOrnamentsListMessage(client.Character.Record.KnownTiles, client.Character.Record.KnownOrnaments,client.Character.Record.ActiveTitle, client.Character.Record.ActiveOrnament));
        }
        [MessageHandler]
        public static void HandleOrnamentSelect(OrnamentSelectRequestMessage message,WorldClient client)
        {
            client.Character.SelectOrnament(message.ornamentId);
            client.Send(new OrnamentSelectedMessage(message.ornamentId));
        }
        [MessageHandler]
        public static void HandleTitleSelect(TitleSelectRequestMessage message,WorldClient client)
        {
            client.Character.SelectTitle(message.titleId);
            client.Send(new TitleSelectedMessage(message.titleId));
        }
        

    }
}
