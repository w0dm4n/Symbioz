using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models;
using Symbioz.World.Models.Exchanges.Craft;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances.Prisms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Provider
{
    public class InteractiveActionProvider
    {
        public const int ZaapiCost = 20;

        private static Dictionary<string, Action<WorldClient, InteractiveRecord>> InteractiveActionsManager = new Dictionary<string, Action<WorldClient, InteractiveRecord>>();
        [StartupInvoke(StartupInvokeType.Internal)]
        public static void LoadSkills()
        {
            InteractiveActionsManager.Add("paddock",Paddock);
            InteractiveActionsManager.Add("zaap", Zaap);
            InteractiveActionsManager.Add("teleport", Teleport);
            InteractiveActionsManager.Add("document", Document);
            InteractiveActionsManager.Add("zaapi", Zaapi);
            InteractiveActionsManager.Add("craft", Craft);
            InteractiveActionsManager.Add("emote", Emote);
            InteractiveActionsManager.Add("title", Title);
            InteractiveActionsManager.Add("ornament",Ornament);
            InteractiveActionsManager.Add("duty", Duty);
            InteractiveActionsManager.Add("smithmagic", Smithmagic);

        }
        static void Ornament(WorldClient client,InteractiveRecord ele)
        {
            client.Character.AddOrnament(ushort.Parse(ele.OptionalValue1));
        }
        static void Title(WorldClient client,InteractiveRecord ele)
        {
            client.Character.AddTitle(ushort.Parse(ele.OptionalValue1));
        }
        static void Duty(WorldClient client,InteractiveRecord ele)
        {
            foreach (var ornament in OrnamentRecord.Ornaments)
            {
                client.Character.AddOrnament(ornament.Id);
            }
            foreach (var title in TitleRecord.Titles)
            {
                client.Character.AddTitle((ushort)title.Id);
            }
        }
        static void Emote(WorldClient client,InteractiveRecord ele)
        {
            client.Character.LearnEmote(byte.Parse(ele.OptionalValue1));
        }
        static void Smithmagic(WorldClient client,InteractiveRecord ele)
        {
            client.Character.SmithMagicInstance = new SmithMagicExchange(client,uint.Parse(ele.OptionalValue1));
            client.Character.SmithMagicInstance.OpenPanel();
        }
        static void Craft(WorldClient client,InteractiveRecord ele)
        {
            client.Character.CraftInstance = new CraftExchange(client, uint.Parse(ele.OptionalValue1),sbyte.Parse(ele.OptionalValue2));
            client.Character.CraftInstance.OpenPanel();
        }
        static void Zaapi(WorldClient client,InteractiveRecord ele)
        {
            if (client.Character.CurrentDialogType == DialogTypeEnum.DIALOG_TELEPORTER)
                return;
            client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_TELEPORTER;

            var maps = new List<int>();
            var subareas = new List<ushort>();
            var cost = new List<ushort>();
            var tptype = new List<sbyte>();
            foreach (InteractiveRecord interactive in InteractiveRecord.GetInteractivesByActionType("Zaapi"))
            {
                if (interactive.OptionalValue1 == ele.OptionalValue1)
                {
                    maps.Add(interactive.MapId);
                    subareas.Add(MapRecord.GetSubAreaId(interactive.MapId));
                    cost.Add(ZaapiCost);
                    tptype.Add(0);
                }
            }
            client.Send(new TeleportDestinationsListMessage((sbyte)TeleporterTypeEnum.TELEPORTER_SUBWAY, maps, subareas, cost, tptype));
        }
        static void Document(WorldClient client,InteractiveRecord ele)
        {
            client.Send(new DocumentReadingBeginMessage(ushort.Parse(ele.OptionalValue1)));
        }
        static void Teleport(WorldClient client,InteractiveRecord ele)
        {
            client.Character.Teleport(int.Parse(ele.OptionalValue1),short.Parse(ele.OptionalValue2));
        }
        static void Paddock(WorldClient client,InteractiveRecord ele)
        {
            client.Character.OpenPaddock();
        }

        #region Zaap

        public static void Zaap(WorldClient client, InteractiveRecord ele)
        {
            if (client.Character.CurrentDialogType == DialogTypeEnum.DIALOG_TELEPORTER)
                return;

            client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_TELEPORTER;
            var maps = new List<int>();
            var subareas = new List<ushort>();
            var costs = new List<ushort>();
            var tptype = new List<sbyte>();

            foreach (InteractiveRecord interactive in InteractiveRecord.GetInteractivesByActionType("Zaap"))
            {
                if (interactive.OptionalValue1 == ele.OptionalValue1)
                {
                    maps.Add(interactive.MapId);
                    subareas.Add(MapRecord.GetSubAreaId(interactive.MapId));
                    costs.Add(GetTeleportationCost(client.Character.Map, MapRecord.GetMap(interactive.MapId)));
                    tptype.Add((sbyte)TeleporterTypeEnum.TELEPORTER_ZAAP);
                }
            }

            if(client.Character.HasGuild && client.Character.HasAlliance)
            {
                List<PrismRecord> prisms = PrismRecord.GetAlliancePrisms(client.Character.AllianceId);
                if(prisms.Count > 0)
                {
                    foreach(PrismRecord prism in prisms)
                    {
                        if (prism.HasTeleportationModule)
                        {
                            maps.Add(prism.MapId);
                            subareas.Add((ushort)prism.SubAreaId);
                            costs.Add(GetTeleportationCost(client.Character.Map, prism.Map.Record));
                            tptype.Add((sbyte)TeleporterTypeEnum.TELEPORTER_PRISM);
                        }
                    }
                }
            }
            
            client.Send(new ZaapListMessage((sbyte)TeleporterTypeEnum.TELEPORTER_ZAAP, maps, subareas, costs, tptype, client.Character.Record.SpawnPointMapId));
        }

        public static void Prism(WorldClient client, PrismRecord prismRecord)
        {
            if (client.Character.HasGuild && client.Character.HasAlliance)
            {
                if (client.Character.CurrentDialogType == DialogTypeEnum.DIALOG_TELEPORTER)
                    return;

                client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_TELEPORTER;
                var maps = new List<int>();
                var subareas = new List<ushort>();
                var costs = new List<ushort>();
                var tptype = new List<sbyte>();

                sbyte teleporterType = (sbyte)TeleporterTypeEnum.TELEPORTER_PRISM;

                foreach (InteractiveRecord interactive in InteractiveRecord.GetInteractivesByActionType("Zaap"))
                {
                    if (interactive.OptionalValue1 == "Global")
                    {
                        maps.Add(interactive.MapId);
                        subareas.Add(MapRecord.GetSubAreaId(interactive.MapId));
                        costs.Add(GetTeleportationCost(client.Character.Map, MapRecord.GetMap(interactive.MapId)));
                        tptype.Add((sbyte)TeleporterTypeEnum.TELEPORTER_ZAAP);
                    }
                }

                List<PrismRecord> prisms = PrismRecord.GetAlliancePrisms(client.Character.AllianceId);
                if ((prisms.Count - 1) > 0)
                {
                    foreach (PrismRecord prism in prisms)
                    {
                        if (prism.HasTeleportationModule)
                        {
                            maps.Add(prism.MapId);
                            subareas.Add((ushort)prism.SubAreaId);
                            costs.Add(GetTeleportationCost(client.Character.Map, prism.Map.Record));
                            tptype.Add((sbyte)TeleporterTypeEnum.TELEPORTER_PRISM);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Zaap");
                    teleporterType = (sbyte)TeleporterTypeEnum.TELEPORTER_ZAAP;
                }

                client.Send(new TeleportDestinationsListMessage(teleporterType, maps, subareas, costs, tptype));
            }
            else
            {
                client.Character.Reply("Vous n'appartenez à aucune alliance.", Color.Orange);
            }
        }

        public static ushort GetTeleportationCost(MapRecord departureMap, MapRecord destinationMap)
        {
            return (ushort)System.Math.Floor(System.Math.Sqrt((double)((departureMap.WorldX - destinationMap.WorldX) *
                (departureMap.WorldY - destinationMap.WorldX) + (departureMap.WorldY - destinationMap.WorldY) * (departureMap.WorldY - destinationMap.WorldY))) * 10.0);
        }

        #endregion

        public static void Handle(WorldClient client,InteractiveRecord ele)
        {
            var interaction = InteractiveActionsManager.FirstOrDefault(x => x.Key ==ele.ActionType.ToLower());
            if (interaction.Value != null)
            {
                client.Send(new InteractiveUsedMessage((uint)client.Character.Id,(uint) ele.ElementId,(ushort)ele.SkillId,30));
                client.Send(new InteractiveUseEndedMessage((uint)ele.ElementId,(ushort) ele.SkillId));
                try
                {
                    interaction.Value(client, ele);
                }
                catch (Exception ex)
                {
                    client.Character.NotificationError( ele.ActionType + ": " + ex.Message);
                }
            }
            else
                client.Character.Reply("[Interactives] Impossible de traiter (" +ele.ActionType + ")",System.Drawing.Color.Red);
        }
    }
}
