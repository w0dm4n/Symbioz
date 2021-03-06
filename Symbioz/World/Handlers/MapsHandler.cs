﻿using Symbioz.Network.Messages;
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
using Shader.Helper;
using Symbioz.World.Records.Alliances.Prisms;
using System.Drawing;
using Symbioz.World.Models;

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
                client.Character.NotificationError("Map inconnu ...(" + message.mapId + ")");
                return;
            }
            client.Character.Map.Instance.SyncMonsters();
            client.Character.SubAreaId = MapRecord.GetSubAreaId(message.mapId);

            client.Send(new MapComplementaryInformationsDataMessage(client.Character.SubAreaId, message.mapId, new List<HouseInformations>(),
               client.Character.Map.Instance.GetActors(client), client.Character.Map.Instance.GetInteractiveElements(), new List<StatedElement>(),
                new List<MapObstacle>(), client.Character.Map.Instance.Fights));
            
            AvAState.GetState(client, client.Character.Map.Instance.GetPlayers());
            if (client.Character.Party != null)
            {
                client.Character.Party.UpdateFollowingMap(client.Character);
            }
            //TODO:TaxCollectors
            client.Character.Map.Instance.ShowFightsCount(client);
            client.Character.CheckMapTip(message.mapId);
            client.Character.Map.Instance.AddClient(client);
            client.Character.RefreshMap();
        }

        [MessageHandler]
        public static void HandleMapMovement(GameMapMovementRequestMessage message, WorldClient client)
        {
            if (CharactersInvisibleRecord.CheckInvisible(client.Character.Record.Id))
            {
                if (CharactersInvisibleRecord.CanStillBeInvisible(client.Character.Record.Id))
                {
                    CharactersInvisibleRecord.DecreaseMovementAuthorized(client.Character.Record.Id);
                    client.Character.Reply("Il vous reste <b>" + CharactersInvisibleRecord.getAuthorizedMovement(client.Character.Record.Id) + " déplacements</b> avant de redevenir visible !");
                }
                else
                {
                    if (CharactersInvisibleRecord.getCharacterLook(client.Character.Record.Id) == "{44|||}")
                    {
                        client.Character.Look = ContextActorLook.Parse(client.Character.Record.OldLook);
                        client.Character.RefreshOnMapInstance();
                    }
                    else
                    {
                        client.Character.Look = ContextActorLook.Parse(CharactersInvisibleRecord.getCharacterLook(client.Character.Record.Id));
                        client.Character.RefreshOnMapInstance();
                    }
                    client.Character.Reply("Votre potion d'invisbilité ne fait plus effet !");
                    CharactersInvisibleRecord.DeleteInvisibleCharacter(client.Character.Record.Id);
                }
            }
            sbyte direction = PathParser.GetDirection(message.keyMovements.Last());
            short cellid = PathParser.ReadCell(message.keyMovements.Last());
            if (client.Character.IsFighting)
            {
                client.Character.FighterInstance.Move(message.keyMovements.ToList(), cellid, direction);
            }
            else
            {
                if (client.Character.Busy || client.Character.Restrictions.cantMove == true)
                    return;

                if (client.Character.CurrentlyInTrackRequest)
                {
                    client.Character.OnEndingUseDelayedObject(DelayedActionTypeEnum.DELAYED_ACTION_OBJECT_USE);
                    client.Character.CurrentlyInTrackRequest = false;
                }

                if (client.Character.Map.Id == message.mapId)
                {
                    if (client.Character.HasGuild)
                    {
                        client.Character.Look.UnsetGuildBanner(client.Character.GetGuild().SymbolShape);
                    }

                    if (client.Character.HasAlliance)
                    {
                        client.Character.Look.UnsetAllianceBanner(client.Character.GetAlliance().SymbolShape);
                    }

                    client.Character.Look.UnsetAura();
                    client.Character.RefreshOnMapInstance();
                    client.Character.Record.Direction = direction;
                    client.Character.MovedCell = cellid;
                    client.Character.CheckRegen();
                    client.Character.SendMap(new GameMapMovementMessage(message.keyMovements, client.Character.Id));
                }
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

            if(client.Character.Map.HasTriggerOnCell(client.Character.Record.CellId))
            {
                var currentTrigger = client.Character.Map.GetTrigger(client.Character.Record.CellId);
                if(currentTrigger != null)
                {
                    client.Character.Teleport(currentTrigger.NextMapId, (short)currentTrigger.NextCellId);
                }
            }
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
                    client.Character.NotificationError("Impossible de charger cette carte.");
                }
            }
            else
            {
                scrollType = MapScrollActionRecord.GetScrollTypeFromCell(client.Character.Record.CellId);
                if (scrollType == MapScrollType.UNDEFINED)
                {
                    if (client.Character.isDebugging)
                        client.Character.NotificationError("Erreur : 'MapScrollAction'");
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
                        client.Character.NotificationError("Impossible de charger cette carte.");
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
            bool canUse = false;

            if ((TeleporterTypeEnum)message.teleporterType == TeleporterTypeEnum.TELEPORTER_ZAAP)
            {
                if (client.Character.Map != null)
                {
                    if (client.Character.Map.Zaap != null)
                    {
                        InteractiveRecord destinationZaap = MapRecord.GetMap(message.mapId).Zaap;
                        InteractiveRecord currentZaap = client.Character.Map.Zaap;
                        if (destinationZaap.OptionalValue1 != currentZaap.OptionalValue1)
                            return;
                    }
                    else if (client.Character.Map.Instance.Prism != null)
                    {
                        if (client.Character.HasGuild && client.Character.HasAlliance)
                        {
                            InteractiveRecord destinationZaap = MapRecord.GetMap(message.mapId).Zaap;
                            if (destinationZaap.OptionalValue1 != "Global" || client.Character.Map.Instance.Prism.AllianceId !=
                                client.Character.AllianceId)
                                return;
                        }
                    }
                }
                else
                    return;
            }

            if ((TeleporterTypeEnum)message.teleporterType == TeleporterTypeEnum.TELEPORTER_PRISM)
            {
                if (client.Character.HasGuild && client.Character.HasAlliance)
                {
                    if (client.Character.Map.Instance.Prism != null || client.Character.Map.Instance.Prism.AllianceId == client.Character.AllianceId)
                    {
                        PrismRecord destinationPrism = PrismRecord.GetPrismByMapId(message.mapId);
                        if (destinationPrism == null || destinationPrism.AllianceId != client.Character.AllianceId)
                        {
                            client.Character.Reply("Impossible d'utiliser ce prisme car vous n'appartenez pas à l'alliance propriétaire de celui-ci.", Color.Red);
                            return;
                        }
                    }
                    else
                        return;
                }
                else
                    return;
            }

            switch ((TeleporterTypeEnum)message.teleporterType)
            {
                case TeleporterTypeEnum.TELEPORTER_SUBWAY:
                    canUse = client.Character.RemoveKamas(InteractiveActionProvider.ZaapiCost, true);
                    break;

                case TeleporterTypeEnum.TELEPORTER_ZAAP:
                    canUse = client.Character.RemoveKamas(InteractiveActionProvider.GetTeleportationCost(client.Character.Map, MapRecord.GetMap(message.mapId)), true);
                    break;
                
                case TeleporterTypeEnum.TELEPORTER_PRISM:
                    canUse = client.Character.RemoveKamas(InteractiveActionProvider.GetTeleportationCost(client.Character.Map, MapRecord.GetMap(message.mapId)), true);
                    break;
            }
    
            if (canUse)
            {
                client.Character.Teleport(message.mapId, (short)InteractiveRecord.GetTeleporterCellId(message.mapId, (TeleporterTypeEnum)message.teleporterType));
                client.Character.LeaveDialog();
                client.Character.Reply("Vous avez été téleporté.");
            }
            else
            {
                client.Character.Reply("Vous n'avez pas assez de kamas pour réaliser cette action.", Color.Red);
            }
        }

        [MessageHandler]
        public static void HandleMapNotFound(ErrorMapNotFoundMessage message, WorldClient client)
        {
            client.Character.Reply("Cette carte (id : " + message.mapId + ") n'existe pas ...");
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

                if (client.Character.HasGuild)
                {
                    client.Character.Look.UnsetGuildBanner(client.Character.GetGuild().SymbolShape);
                }

                if (client.Character.HasAlliance)
                {
                    client.Character.Look.UnsetAllianceBanner(client.Character.GetAlliance().SymbolShape);
                }

                switch (template.Id)
                {
                    case 1: //Émote s'asseoir
                        if (client.Character.Restrictions.isDead)
                            return;
                        if (client.Character.IsRegeneratingLife && client.Character.RegenRate == 10)
                            client.Character.StopRegenLife();
                        if (client.Character.IsRegeneratingLife && client.Character.RegenRate == 3)
                        {
                            client.Character.StopRegenLife();
                            client.Character.RegenLife(3);
                        }
                        else
                            client.Character.RegenLife(3);
                        break;

                    case 97: //Émote étendard de guilde
                        if (client.Character.HasGuild)
                            client.Character.Look.SetBanner(client.Character.GetGuild().SymbolShape);
                        break;

                    case 98: //Émote étendard d'alliance
                        if (client.Character.HasAlliance)
                        {
                            client.Character.Look.SetBanner(client.Character.GetAlliance().SymbolShape, false);
                        }
                        break;
                }

                client.Character.RefreshOnMapInstance();
                client.Character.SendMap(new EmotePlayMessage(message.emoteId, DateTimeUtils.GetEpochFromDateTime(DateTime.Now), client.Character.Id, client.Account.Id));
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
