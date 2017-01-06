using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Alliances.Prisms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Maps
{
    public class AvAState
    {
        public static IEnumerable<uint> PlayersIEnumerable(uint CharacterContextualId)
        {
            yield return CharacterContextualId;
        }

        public static IEnumerable<sbyte> StateIEnumerable(AggressableStatusEnum State)
        {
            yield return (sbyte)State;
        }

        public static void GetState(WorldClient client, List<GameRolePlayCharacterInformations> actors)
        {
            var MapInstance = client.Character.Map.Instance;
            bool OwnTerritory = false;
            if (MapInstance != null)
            {
                if (MapInstance.Record.Id == 115609089)
                    return;
                if (PrismRecord.PrismOnSubArea(MapInstance.Record.SubAreaId))
                {
                    /*if (client.Character.HasAlliance)
                    {
                        var alliance = client.Character.GetAlliance();
                        if (alliance != null)
                        {
                            var prisms = PrismRecord.GetAlliancePrisms(alliance.Id);
                            foreach (var prism in prisms)
                            {
                                if (prism.SubAreaId == MapInstance.Record.SubAreaId)
                                {
                                    OwnTerritory = true;
                                    break;
                                }
                            }
                            if (OwnTerritory == true)
                            {
                                foreach (var actor in actors)
                                {
                                    client.Send(new UpdateMapPlayersAgressableStatusMessage(PlayersIEnumerable((uint)actor.contextualId), StateIEnumerable(AggressableStatusEnum.AvA_ENABLED_AGGRESSABLE)));
                                }
                            }
                        }
                    }*/
                    if (client.Character.HasAlliance)
                    {
                        foreach (var actor in actors)
                        {
                            client.Send(new UpdateMapPlayersAgressableStatusMessage(PlayersIEnumerable((uint)actor.contextualId), StateIEnumerable(AggressableStatusEnum.AvA_ENABLED_AGGRESSABLE)));
                        }
                    }
                }
                else
                {
                    if (client.Character.HasAlliance)
                    {
                        foreach (var actor in actors)
                        {
                            client.Send(new UpdateMapPlayersAgressableStatusMessage(PlayersIEnumerable((uint)actor.contextualId), StateIEnumerable(AggressableStatusEnum.AvA_ENABLED_AGGRESSABLE)));
                        }
                    }
                }
            }
        }

        public static bool CanJoinFight(Fighter Fighter, WorldClient client)
        {
            if (Fighter != null)
            {
                var FightMaster = WorldServer.Instance.GetOnlineClient(Fighter.FighterStats.RealStats.CharacterId);
                if (FightMaster != null)
                {
                    if (FightMaster.Character.HasAlliance)
                    {
                        if (client.Character.HasAlliance)
                        {
                            if (client.Character.GetAlliance() == FightMaster.Character.GetAlliance())
                                return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
