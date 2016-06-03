using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.World.Models.Alliances;
using Symbioz.World.Models.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    public class AlllianceHandler
    {
        [MessageHandler]
        public static void HandleAllianceCreationValidMessage(AllianceCreationValidMessage message, WorldClient client)
        {
            if(AllianceProvider.CanCreateAlliance(client, message))
            {
                AllianceProvider.CreateAlliance(client.Character.GetGuild(), message.allianceTag, message.allianceName, message.allianceEmblem);
            }
        }

        [MessageHandler]
        public static void HandleAllianceInsiderInfoRequestMessage(AllianceInsiderInfoRequestMessage message, WorldClient client)
        {
            if(client.Character.HasAlliance)
            {
                client.Send(AllianceProvider.GetAllianceInsiderInfoMessage(client.Character.GetAlliance()));
            }
        }

        [MessageHandler]
        public static void HandleAllianceKickRequestMessage(AllianceKickRequestMessage message, WorldClient client)
        {
            if(client.Character.HasAlliance)
            {
                if (client.Character.HasGuild)
                {
                    var characterGuild = client.Character.GetGuild();

                    if (GuildProvider.GetLeader(characterGuild.Id).CharacterId == client.Character.Id)
                    {
                        var characterAlliance = client.Character.GetAlliance();

                        bool fromAllianceLeaderGuild = false;
                        if (characterAlliance.LeaderGuildId == client.Character.GetGuild().Id)
                        {
                            fromAllianceLeaderGuild = true;
                        }

                        if (fromAllianceLeaderGuild)
                        {
                            if(characterAlliance.LeaderGuildId == message.kickedId)
                            {
                                AllianceProvider.DeleteAlliance(characterAlliance.Id);
                            }
                            else
                            {
                                AllianceProvider.LeaveAlliance((int)message.kickedId);
                            }
                        }
                        else
                        {
                            if(characterGuild.Id == message.kickedId)
                            {
                                AllianceProvider.LeaveAlliance((int)message.kickedId);
                            }
                        }
                    }
                }
            }
        }

        [MessageHandler]
        public static void HandleAllianceFactsRequestMessage(AllianceFactsRequestMessage message, WorldClient client)
        {
            var targetAlliance = AllianceProvider.GetAlliance((int)message.allianceId);
            if(targetAlliance != null)
            {
                client.Send(AllianceProvider.GetAllianceFactsMessage(targetAlliance));
            }
        }
    }
}
