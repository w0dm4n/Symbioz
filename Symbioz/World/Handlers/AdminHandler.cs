using Symbioz.Network.Messages;
using Symbioz.DofusProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Helper;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Network.Clients;
using Symbioz.World.Records;
using Symbioz.Auth.Models;

namespace Symbioz.World.Handlers
{
    class AdminHandler
    {
        [MessageHandler]
        public static void HandleAdminCommandMessage(AdminCommandMessage message, WorldClient client)
        {
            CommandsHandler.Handle(CommandsHandler.CommandsPrefix + message.content, client);
        }
        [MessageHandler]
        public static void HandleAdminQuietCommand(AdminQuietCommandMessage message, WorldClient client)
        {
            if (message.content.StartsWith("moveto") && !client.Character.IsFighting && client.Account.Role >= ServerRoleEnum.ADMINISTRATOR)
            {
                int mapid;
                if (int.TryParse(message.content.Split(null).Last(), out mapid))
                {
                    var map = MapRecord.GetMap(mapid);
                    if (map.WalkableCells.Contains(client.Character.Record.CellId))
                        client.Character.Teleport(mapid);
                    else
                        client.Character.Teleport(mapid, map.RandomWalkableCell());
                }
            }
        }
    }
}
