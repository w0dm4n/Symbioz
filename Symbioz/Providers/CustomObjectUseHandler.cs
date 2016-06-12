using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models;
using Symbioz.World.Models.Alliances.Prisms;
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.World.Records.Guilds;
using Symbioz.World.Records.SubAreas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers
{
    public class CustomObjectUseHandler
    {
        public static Dictionary<ushort, Action<WorldClient,CharacterItemRecord>> CustomHandlers = new Dictionary<ushort, Action<WorldClient,CharacterItemRecord>>();

        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
            CustomHandlers.Add(14485, HandleMimicry);
            CustomHandlers.Add(14293, HandleAlliancePrism);
        }
        public static bool CustomHandlerExist(ushort itemgid)
        {
            if (CustomHandlers.ContainsKey(itemgid))
                return true;
            else
                return false;
        }

        public static void Handle(WorldClient client, CharacterItemRecord item)
        {
            var handler = CustomHandlers.FirstOrDefault(x=>x.Key == item.GID);
            handler.Value(client,item);
        }

        #region Mimicry

        private static void HandleMimicry(WorldClient client, CharacterItemRecord item)
        {
            client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            client.Send(new ClientUIOpenedByObjectMessage(3, item.UID));
        }

        #endregion

        #region AlliancePrism

        private static void HandleAlliancePrism(WorldClient client, CharacterItemRecord item)
        {
            if(client.Character.HasGuild && client.Character.HasAlliance)
            {
                if (CharacterGuildRecord.GetCharacterGuild(client.Character.Id).HasRight(GuildRightsBitEnum.GUILD_RIGHT_SET_ALLIANCE_PRISM))
                {
                    int subAreaId = client.Character.Map.SubAreaId;
                    string allianceOwnerName = null;
                    switch (PrismsManager.Instance.CanAddPrismOnSubArea(subAreaId, out allianceOwnerName))
                    {
                        case CanAddPrismOnSubAreaResult.SUBAREA_AVAILABLE:
                            PrismsManager.Instance.AddPrismOnSubArea(client.Character, client.Character.Map.SubAreaId, client.Character.Map.Id);
                            client.Character.Inventory.RemoveItem(item.UID, 1);
                            client.Character.Reply(string.Format("Vous venez de poser un prisme dans la zone \"<b>{0}</b>\".", SubAreaRecord.GetSubAreaName(subAreaId)));
                            client.Character.Reply("L'heure de vulnérabilitée par défaut est à <b>17h30</b>, vous pouvez changer cet horraire à tout moment depuis l'onglet <b>\"Conquêtes\"</b> du panel de votre alliance.", Color.Orange);
                            break;

                        case CanAddPrismOnSubAreaResult.SUBAREA_ALREADY_TAKEN:
                            client.Character.Reply(string.Format("Cette zone appartient déjà à l'alliance <b>{0}</b>.<br/>Battez-vous pour conquérir ce territoire !",
                               allianceOwnerName, Color.Orange));
                            break;

                        case CanAddPrismOnSubAreaResult.SUBAREA_NOT_CONQUERABLE:
                            client.Character.Reply("Cette zone n'est pas conquérable !", Color.Orange);
                            break;

                        case CanAddPrismOnSubAreaResult.SUBAREA_ERROR:
                            client.Character.Reply("Une erreur est survenue lors de l'ajout de votre prisme d'alliance. Merci de réessayer ultérieurement !", Color.Red);
                            break;
                    }
                }
                else
                {
                    client.Character.Reply("Vous ne disposez pas des droits nécessaires pour poser un prisme d'alliance");
                }
            }
            else
            {
                client.Character.Reply("Vous devez appartenir à une alliance pour utiliser cet objet !");
            }
        }

        #endregion

    }
}
