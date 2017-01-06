using Shader.Helper;
using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.Providers;
using Symbioz.World.Handlers;
using Symbioz.World.Models.Fights.FightsTypes;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.World.Records.SubAreas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Alliances.Prisms
{
    public class PrismsManager : Singleton<PrismsManager>
    {

        #region Load

        [StartupInvoke("SpawnPrisms", StartupInvokeType.Others)]
        public static void SpawnPrisms()
        {
            foreach (var prism in PrismRecord.Prisms)
            {
                if (prism.Map != null)
                    prism.Map.AddPrism(prism);
            }
        }

        #endregion

        #region ManagePrims

        public List<PrismGeolocalizedInformation> GetWorldPrismGeolocalizedInformation(WorldClient client)
        {
            List<PrismGeolocalizedInformation> worldPrismGeolocalizedInformation = new List<PrismGeolocalizedInformation>();
            foreach (var prism in PrismRecord.Prisms)
            {
                PrismGeolocalizedInformation prismInformations = null;
                if (client.Character.HasAlliance && client.Character.AllianceId == prism.AllianceId)
                {
                    prismInformations = new PrismGeolocalizedInformation((ushort)prism.SubAreaId, (uint)prism.AllianceId,
                        (short)prism.Map.Record.WorldX, (short)prism.Map.Record.WorldY, prism.Map.Record.Id, prism.GetAllianceInsiderPrismInformation());
                }
                else
                {
                    prismInformations = new PrismGeolocalizedInformation((ushort)prism.SubAreaId, (uint)prism.AllianceId,
                    (short)prism.Map.Record.WorldX, (short)prism.Map.Record.WorldY, prism.Map.Record.Id, prism.GetAlliancePrismInformation());
                }
                worldPrismGeolocalizedInformation.Add(prismInformations);
            }
            return worldPrismGeolocalizedInformation;
        }

        #endregion

        #region TalkToPrims

        public void TalkToPrism(WorldClient client, int mapId)
        {
            client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_DIALOG;
            client.Send(new NpcDialogCreationMessage(mapId, PrismRecord.ConstantContextualId));
            client.Send(new AlliancePrismDialogQuestionMessage());
        }

        #endregion

        #region AddingPrisms

        public CanAddPrismOnSubAreaResult CanAddPrismOnSubArea(int subAreaId, out string allianceOwnerName)
        {
            CanAddPrismOnSubAreaResult result = CanAddPrismOnSubAreaResult.SUBAREA_ERROR;
            var subArea = SubAreaRecord.GetSubArea(subAreaId);
            allianceOwnerName = null;
            if(subArea != null)
            {
                if(subArea.Capturable)
                {
                    var existingPrism = PrismRecord.GetPrismBySubAreaId(subAreaId);
                    if(existingPrism == null)
                    {
                        result = CanAddPrismOnSubAreaResult.SUBAREA_AVAILABLE;
                    }
                    else
                    {
                        allianceOwnerName = existingPrism.Alliance.Name;
                        result = CanAddPrismOnSubAreaResult.SUBAREA_ALREADY_TAKEN;
                    }
                }
                else
                {
                    result = CanAddPrismOnSubAreaResult.SUBAREA_NOT_CONQUERABLE;
                }
            }
            return result;
        }

        public void AddPrismOnSubArea(Character character, int subAreaId, int mapId)
        {
            PrismRecord newPrismRecord = new PrismRecord(PrismRecord.PopNextId(), character.AllianceId, subAreaId, mapId,
                1, (int)PrismStateEnum.PRISM_STATE_NORMAL, 70, DateTime.Now.ToEpochTime(), 0, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0, 0)).ToEpochTime(), DateTime.Now.ToEpochTime(),
                character.GuildId, character.Id, character.Record.Name);
            newPrismRecord.Save();
            this.OnPrismAdded(newPrismRecord, character);
        }

        public void OnPrismAdded(PrismRecord newPrismRecord, Character character)
        {
            var map = newPrismRecord.Map;
            if(map != null)
            {
                map.AddPrism(newPrismRecord, character.Record.CellId);
                PrismsHandler.SendPrismListUpdateMessage();
            }
        }

        #endregion

        #region AttackPrisms

        public void AttackPrism(WorldClient client, PrismRecord prism)
        {
            if (client.Character.Map == null)
                return;

            if (!client.Character.Map.IsValid())
            {
                if (client.Character.isDebugging)
                    client.Character.Reply("Impossible de démarrer la phase de placement sur cette carte.");
                return;
            }

            var alliance = prism.Alliance;
            if (alliance != null)
            {
                var AlliancePlayers = AllianceProvider.GetClients(prism.AllianceId);
                var map = MapRecord.GetMap(prism.MapId);
                foreach (var tmp in AlliancePlayers)
                    tmp.Character.Reply("Un prisme de votre alliance est attaqué en (<b>" + map.WorldX + "," + map.WorldY + "</b>, " + SubAreaRecord.GetSubAreaName(map.SubAreaId) + "), rendez vous à cette position et battez vous pour conserver votre territoire !", Color.Orange);
                FightAvAPrism fight = FightProvider.Instance.CreateAvAPrismFight(prism, client.Character.Map, (short)prism.CellId, client.Character.Record.CellId);
                fight.AllianceId = alliance.Id;
                fight.BlueTeam.AddFighter(client.Character.CreateFighter(fight.BlueTeam, fight));
                fight.RedTeam.AddFighter(prism.CreateFighter(fight.RedTeam));
                fight.StartPlacement();

                alliance.AddPrismFight(fight);
            }
        }

        public void TryJoinLeavePrismFight(WorldClient client, PrismRecord prism, bool join)
        {
            if (prism.Fighter != null && prism.Fighter.Fight != null)
            {
                var fight = prism.Fighter.Fight as FightAvAPrism;
                if (join)
                {
                    if (fight.IsAttackersPlacementPhase && fight.DefendersSlotAvailable())
                    {
                        if (fight.AddDefenderToQueue(client.Character))
                        {
                            client.Character.GetAlliance().Send(new PrismFightDefenderAddMessage((ushort)prism.SubAreaId, (ushort)fight.Id,
                                client.Character.GetCharacterMinimalPlusLookInformations()));
                        }
                    }
                }
                else
                {
                    if (fight.IsAttackersPlacementPhase && fight.IsInDefendersQueue(client.Character))
                    {
                        fight.RemoveDefenderFromQueue(client.Character);
                        client.Character.GetAlliance().Send(new PrismFightDefenderLeaveMessage((ushort)prism.SubAreaId, (ushort)prism.Id,
                            (uint)client.Character.Id));
                    }
                }
            }
        }

        #endregion
    }
}
