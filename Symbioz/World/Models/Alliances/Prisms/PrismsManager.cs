using Shader.Helper;
using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.World.Handlers;
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
            PrismRecord.Prisms.ForEach(x => x.Map.AddPrism(x));
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
                1, (int)PrismStateEnum.PRISM_STATE_INVULNERABLE, 70, DateTime.Now.ToEpochTime(), 0, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0, 0)).ToEpochTime(), DateTime.Now.ToEpochTime(),
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
    }
}
