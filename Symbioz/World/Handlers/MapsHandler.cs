using Symbioz.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Clients;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Records;
using Symbioz.World.PathProvider;
using Symbioz.Enums;
using Symbioz.World.Models.Maps;
using Symbioz.Core;
using Symbioz.Provider;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Maps;

namespace Symbioz.World.Handlers
{
    class MapsHandler
    {
        public const string MapKey = "649ae451ca33ec53bbcbcc33becf15f4";

        [MessageHandler]
        public static void HandleMapInformations(MapInformationsRequestMessage message, WorldClient client)
        {
            client.Character.Map = MapRecord.GetMap(message.mapId);
            if (client.Character.Map == null)
            {
                client.Character.TeleportToSpawnPoint();
                client.Character.NotificationError("Unknown Map...(" + message.mapId + ")");
                return;
            }
            client.Character.Map.Instance.SyncMonsters();
            client.Character.SubAreaId = MapRecord.GetSubAreaId(message.mapId);

            client.Send(new MapComplementaryInformationsDataMessage(client.Character.SubAreaId, message.mapId, new List<HouseInformations>(),
               client.Character.Map.Instance.GetActors(), client.Character.Map.Instance.GetInteractiveElements(), new List<StatedElement>(),
                new List<MapObstacle>(), client.Character.Map.Instance.Fights));

            client.Character.Map.Instance.ShowFightsCount(client);
            client.Character.CheckMapTip(message.mapId);
            client.Character.Map.Instance.AddClient(client);
            client.Character.RefreshMap();
        }
        [MessageHandler]
        public static void HandleMapMovement(GameMapMovementRequestMessage message, WorldClient client)
        {
            sbyte direction = PathParser.GetDirection(message.keyMovements.Last());
            short cellid = PathParser.ReadCell(message.keyMovements.Last());
            if (client.Character.IsFighting)
            {
                client.Character.FighterInstance.Move(message.keyMovements.ToList(), cellid, direction);
            }
            else
            {
                if (client.Character.Busy)
                    return;
                client.Character.Look.UnsetAura();
                client.Character.RefreshOnMapInstance();
                client.Character.Record.Direction = direction;
                client.Character.MovedCell = cellid;
                client.Character.SendMap(new GameMapMovementMessage(message.keyMovements, client.Character.Id));
            }
        }
        [MessageHandler]
        public static void HandleMapMovementCancel(GameMapMovementCancelMessage message, WorldClient client)
        {
            var mobGroup = client.Character.Map.Instance.MonstersGroups.Find(x => x.CellId == client.Character.MovedCell);
            if (mobGroup != null)
            {
                client.Character.CancelMonsterAgression = true;
            }
            client.Character.Record.CellId = (short)message.cellId;
        }
        [MessageHandler]
        public static void HandleMapMovementConfirm(GameMapMovementConfirmMessage message, WorldClient client)
        {
            client.Character.Record.CellId = client.Character.MovedCell;
            client.Character.MovedCell = 0;
        }
        [MessageHandler]
        public static void HandleChangeMap(ChangeMapMessage message, WorldClient client)
        {
            MapScrollType scrollType = MapScrollType.UNDEFINED;
            if (client.Character.Map.LeftMap == message.mapId)
                scrollType = MapScrollType.LEFT;
            if (client.Character.Map.RightMap == message.mapId)
                scrollType = MapScrollType.RIGHT;
            if (client.Character.Map.DownMap == message.mapId)
                scrollType = MapScrollType.BOTTOM;
            if (client.Character.Map.TopMap == message.mapId)
                scrollType = MapScrollType.TOP;

            if (scrollType != MapScrollType.UNDEFINED)
            {
                int overrided = MapScrollActionRecord.GetOverrideScrollMapId(client.Character.Map.Id, scrollType);
                short cellid = MapScrollActionRecord.GetScrollDefaultCellId(client.Character.Record.CellId, scrollType);
                client.Character.Record.Direction = MapScrollActionRecord.GetScrollDirection(scrollType);

                int teleportMapId = overrided != -1 ? overrided : message.mapId;
                if (overrided == 0)
                    teleportMapId = message.mapId;
                MapRecord teleportedMap = MapRecord.GetMap(teleportMapId);
                if (teleportedMap != null)
                {
                    cellid = teleportedMap.Walkable(cellid) ? cellid : MapScrollActionRecord.SearchScrollCellId(cellid, scrollType, teleportedMap);
                    client.Character.Teleport(teleportMapId, cellid);
                }
                else
                {
                    client.Character.NotificationError("This map cannot be founded");
                }
            }
            else
            {
                scrollType = MapScrollActionRecord.GetScrollTypeFromCell(client.Character.Record.CellId);
                if (scrollType == MapScrollType.UNDEFINED)
                {
                    client.Character.NotificationError("Unknown Map Scroll Action...");
                }
                else
                {
                    int overrided = MapScrollActionRecord.GetOverrideScrollMapId(client.Character.Map.Id, scrollType);
                    short cellid = MapScrollActionRecord.GetScrollDefaultCellId(client.Character.Record.CellId, scrollType);
                    MapRecord teleportedMap = MapRecord.GetMap(overrided);
                    if (teleportedMap != null)
                    {
                        client.Character.Record.Direction = MapScrollActionRecord.GetScrollDirection(scrollType);
                        cellid = teleportedMap.Walkable(cellid) ? cellid : MapScrollActionRecord.SearchScrollCellId(cellid, scrollType, teleportedMap);
                        client.Character.Teleport(overrided, cellid);
                    }
                    else
                    {
                        client.Character.NotificationError("This map cannot be founded");
                    }
                }
            }
        }
        [MessageHandler]
        public static void HandleGameMapChangeOriantation(GameMapChangeOrientationRequestMessage message, WorldClient client)
        {
            client.Character.Record.Direction = message.direction;
            client.Character.SendMap(new GameMapChangeOrientationMessage(new ActorOrientation(client.Character.Id, message.direction)));
        }
        [MessageHandler]
        public static void HandleTeleportRequestMessage(TeleportRequestMessage message, WorldClient client)
        {
            if ((TeleporterTypeEnum)message.teleporterType == TeleporterTypeEnum.TELEPORTER_ZAAP)
            {
                InteractiveRecord destinationZaap = MapRecord.GetMap(message.mapId).Zaap;
                if (client.Character.Map != null)
                {
                    InteractiveRecord currentZaap = client.Character.Map.Zaap;
                    if (destinationZaap.OptionalValue1 != currentZaap.OptionalValue1)
                        return;
                }
                else
                    return;
            }
            client.Character.Teleport(message.mapId, (short)InteractiveRecord.GetTeleporterCellId(message.mapId, (TeleporterTypeEnum)message.teleporterType));
            switch ((TeleporterTypeEnum)message.teleporterType)
            {
                case TeleporterTypeEnum.TELEPORTER_ZAAP:
                    client.Character.RemoveKamas(InteractiveActionProvider.ZaapCost, true);
                    break;
                case TeleporterTypeEnum.TELEPORTER_SUBWAY:
                    client.Character.RemoveKamas(InteractiveActionProvider.ZaapiCost, true);
                    break;
                case TeleporterTypeEnum.TELEPORTER_PRISM:
                    break;
            }
            client.Character.LeaveDialog();
            client.Character.Reply("Vous avez été téleporté");
        }
        [MessageHandler]
        public static void HandleMapNotFound(ErrorMapNotFoundMessage message, WorldClient client)
        {
            client.Character.Reply("Cette carte (" + message.mapId + ") n'éxiste pas...");
            client.Character.Teleport(ConfigurationManager.Instance.StartMapId, ConfigurationManager.Instance.StartCellId);
        }
        [MessageHandler]
        public static void HandleEmotePlay(EmotePlayRequestMessage message, WorldClient client)
        {
            EmoteRecord template = EmoteRecord.GetEmote(message.emoteId);
            if (template.IsAura)
            {
                client.Character.PlayAura(message.emoteId);
            }
            else
            {
                client.Character.Look.UnsetAura();
                client.Character.RefreshOnMapInstance();
                client.Character.SendMap(new EmotePlayMessage(message.emoteId, 12, client.Character.Id, client.Account.Id));
            }
        }
        [MessageHandler]
        public static void HandleCautiousMapMovement(GameCautiousMapMovementRequestMessage message, WorldClient client)
        {
            if (client.Character.IsFighting)
            {
                Logger.Error("Client try a cautious move while fighting, aborting");
                return;
            }
            sbyte direction = PathParser.GetDirection(message.keyMovements.Last());
            short cellid = PathParser.ReadCell(message.keyMovements.Last());
            if (client.Character.Busy)
                return;
            client.Character.Look.UnsetAura();
            client.Character.RefreshOnMapInstance();
            client.Character.Record.Direction = direction;
            client.Character.MovedCell = cellid;
            client.Character.SendMap(new GameCautiousMapMovementMessage(message.keyMovements, client.Character.Id));
        }
        public static void SendCurrentMapMessage(WorldClient client, int mapid)
        {
            client.Send(new CurrentMapMessage(mapid, MapKey));
        }
    }
}
