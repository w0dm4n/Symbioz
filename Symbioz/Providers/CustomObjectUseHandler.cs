using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.World.Models;
using Symbioz.World.Models.Alliances.Prisms;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.World.Records.Guilds;
using Symbioz.World.Records.SubAreas;
using Symbioz.World.Records.Tracks;
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
        public static Dictionary<ushort, Action<WorldClient, CharacterItemRecord>> CustomItemIdsHandlers = new Dictionary<ushort, Action<WorldClient, CharacterItemRecord>>();
        public static Dictionary<int, Action<WorldClient, CharacterItemRecord>> CustomItemTypesIdsHandlers = new Dictionary<int, Action<WorldClient, CharacterItemRecord>>();

        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
            //Handlers for ItemId [ItemGID, CatchMethod(WorldClient, CharacterItemRecord)]
            CustomItemIdsHandlers.Add(14485, HandleMimicry); //Mimibiotes
            CustomItemIdsHandlers.Add(14293, HandleAlliancePrism); //Prismes d'alliance
            CustomItemIdsHandlers.Add(7400, HandleLinkedParchment); //Parchemins liés

            //Handlers for ItemTypeId [ItemTypeID, CatchMethod(WorldClient, CharacterItemRecord)]
            CustomItemTypesIdsHandlers.Add(33, HandleBread); //Pain
        }

        #region CustomItemIdsHandlers

        public static bool CustomHandlerExistForItemId(ushort itemGID)
        {
            return CustomItemIdsHandlers.ContainsKey(itemGID);
        }

        public static void HandleByItemGID(WorldClient client, CharacterItemRecord item, int repeatCount = 0)
        {
            var handler = CustomItemIdsHandlers.FirstOrDefault(x => x.Key == item.GID);
            if (repeatCount <= 0)
            {
                handler.Value(client, item);
            }
            else
            {
                for (int i = 0; i < repeatCount; i++)
                {
                    handler.Value(client, item);
                }
            }
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
            if (client.Character.HasGuild && client.Character.HasAlliance)
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

        #region LinkedParchment

        public static void HandleLinkedParchment(WorldClient client, CharacterItemRecord item)
        {
            var target = WorldServer.Instance.GetOnlineClient(TrackRecord.GetCharacterIdTrackedFromItemUID((int)item.UID));
            if (target != null)
            {
                var targetPosition = MapRecord.GetMap(target.Character.Record.MapId);
                var targetCoordinates = new MapCoordinates((short)targetPosition.WorldX, (short)targetPosition.WorldY);
                client.Character.Reply("<b>" + target.Character.Record.Name + "</b> se trouve actuellement en : (<b>" + targetPosition.WorldX + "," + targetPosition.WorldY + "</b>)");
                client.Send(new CompassUpdatePvpSeekMessage(0, targetCoordinates, (uint)target.Character.Id, target.Character.Record.Name));
            }
            else
            {
                client.Send(new TextInformationMessage(1, 211, new string[0]));
            }
        }

        #endregion

        #endregion

        #region CustomItemTypesHandlers

        private static string DefaultErrorMessageForUnavailableItemRecord = "(HandleByItemTypeId) Can't handle ItemGlobalId({1}) because ItemRecord is null";

        public static bool CustomHandlerExistForItemTypeId(CharacterItemRecord item)
        {
            bool exist = false;
            var itemRecord = ItemRecord.GetItem(item.GID);
            if (itemRecord != null)
            {
                int itemTypeId = itemRecord.TypeId;
                exist = CustomItemTypesIdsHandlers.ContainsKey(itemTypeId);
            }
            else
            {
                Logger.Error(string.Format(DefaultErrorMessageForUnavailableItemRecord, item.GID));
            }
            return exist;
        }

        public static void HandleByItemTypeId(WorldClient client, CharacterItemRecord item, int repeatCount = 0)
        {
            var itemRecord = ItemRecord.GetItem(item.GID);
            if (itemRecord != null)
            {
                int itemTypeId = itemRecord.TypeId;
                var handler = CustomItemTypesIdsHandlers.FirstOrDefault(x => x.Key == itemTypeId);
                if (repeatCount <= 0)
                {
                    handler.Value(client, item);
                }
                else
                {
                    for (int i = 0; i < repeatCount; i++)
                    {
                        handler.Value(client, item);
                    }
                }
            }
            else
            {
                Logger.Error(string.Format(DefaultErrorMessageForUnavailableItemRecord, item.GID));
            }
        }

        #region Bread

        public static void HandleBread(WorldClient client, CharacterItemRecord item)
        {
            if (client.Character.IsFighting)
                return;

            var template = ItemRecord.GetItem(item.GID);
            if (template == null)
                return;

            if (client.Character.CurrentStats.LifePoints >= client.Character.CharacterStatsRecord.LifePoints)
            {
                client.Character.Reply("Vos points de vie sont déjà au maximum !");
                return;
            }

            var lifeBack = 0;
            if (client.Character.CurrentStats.LifePoints <= client.Character.CharacterStatsRecord.LifePoints)
            {
                var effects = item.Effects.Split('|');
                if (effects == null || effects.Length == 0)
                    return;
                foreach (var effect in effects)
                {
                    var current = effect.Split(new string[] { "70#110#" }, StringSplitOptions.None);
                    if (current != null && current.Length == 2)
                        lifeBack += int.Parse(current[1]);
                }
            }

            if((client.Character.CurrentStats.LifePoints + lifeBack) > client.Character.CharacterStatsRecord.LifePoints)
            {
                lifeBack = (int)(client.Character.CharacterStatsRecord.LifePoints - client.Character.CurrentStats.LifePoints);
            }

            client.Character.CurrentStats.LifePoints = (uint)(client.Character.CurrentStats.LifePoints + lifeBack);

            string characterReply = "Vous avez récupéré {0} points de vie !";
            if (lifeBack <= 1)
            {
                characterReply = "Vous avez récupéré {0} point de vie !";
            }
            client.Character.Reply(string.Format(characterReply, lifeBack));


            client.Character.Record.CurrentLifePoint = client.Character.CurrentStats.LifePoints;
            client.Character.RefreshStats();
            client.Character.Inventory.RemoveItem(item.UID, 1, true);
        }

        #endregion

        #endregion
    }
}

