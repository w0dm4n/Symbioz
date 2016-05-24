using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models;
using Symbioz.World.Models.Exchanges.Craft;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Provider
{
    class InteractiveActionProvider
    {
        public const int ZaapCost = 300;
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
        public static void Zaap(WorldClient client,InteractiveRecord ele)
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
                    costs.Add(ZaapCost);
                    tptype.Add((sbyte)TeleporterTypeEnum.TELEPORTER_ZAAP);
                }

            }
            client.Send(new ZaapListMessage((sbyte)TeleporterTypeEnum.TELEPORTER_ZAAP, maps, subareas, costs, tptype,client.Character.Record.SpawnPointMapId));
        }
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
                client.Character.Reply("[Interactives] Unable to handle (" +ele.ActionType + ")",System.Drawing.Color.Red);
        }
    }
}
