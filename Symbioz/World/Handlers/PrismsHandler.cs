using Shader.Helper;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.Provider;
using Symbioz.World.Models.Alliances.Prisms;
using Symbioz.World.Models.Exchanges;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances;
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    public class PrismsHandler
    {
        [MessageHandler]
        public static void HandlePrismsListRegisterMessage(PrismsListRegisterMessage message, WorldClient client)
        {
            client.Send(new PrismsListMessage(PrismsManager.Instance.GetWorldPrismGeolocalizedInformation(client)));
        }

        [MessageHandler]
        public static void HandlePrismSettingsRequestMessage(PrismSettingsRequestMessage message, WorldClient client)
        {
            if (client.Character.HasGuild && client.Character.HasAlliance)
            {
                var prism = PrismRecord.GetPrismBySubAreaId(message.subAreaId);
                if (prism != null)
                {
                    if(client.Character.AllianceId == prism.AllianceId)
                    {
                        if (CharacterGuildRecord.GetCharacterGuild(client.Character.Id).HasRight(Enums.GuildRightsBitEnum.GUILD_RIGHT_SET_ALLIANCE_PRISM))
                        {
                            TimeSpan timeSpanDifference = (DateTime.Now - prism.LastTimeSettingsModificationDate.GetDateTimeFromEpoch());
                            if (timeSpanDifference.TotalHours >= 24)
                            {
                                prism.ChangeSettings(client.Character, message.startDefenseTime);
                                prism.Save();
                                prism.RefreshOnMapInstance();
                                SendPrismListUpdateMessage();
                            }
                            else
                            {
                                TimeSpan baseTimeSpan = new TimeSpan(24, 0, 0);
                                client.Character.Reply("Vous ne pouvez changer l'heure de vulnérabilité d'un prisme qu'<b>une seule fois toutes les 24 heures</b>.", Color.Orange);
                                client.Character.Reply(string.Format("Il vous reste encore à attendre : <u>{0} heure(s), {1} minute(s) et {2} seconde(s)</u>",
                                    baseTimeSpan.Subtract(timeSpanDifference).ToString(@"hh"), baseTimeSpan.Subtract(timeSpanDifference).ToString(@"mm"),
                                    baseTimeSpan.Subtract(timeSpanDifference).ToString(@"ss")), Color.Orange);
                            }
                        }
                        else
                        {
                            client.Character.Reply("Vous n'avez pas les droits nécessaires pour effectuer cette action.");
                        }
                    }
                    else
                    {
                        SendSettingsErrorMessage(client);
                    }
                }
                else
                {
                    SendSettingsErrorMessage(client);
                }
            }
            else
            {
                SendSettingsErrorMessage(client);
            }
        }

        [MessageHandler]
        public static void HandlePrismModuleExchangeRequestMessage(PrismModuleExchangeRequestMessage message, WorldClient client)
        {
            if (client.Character.HasGuild && client.Character.HasAlliance)
            {
                var prism = PrismRecord.GetPrismByMapId(client.Character.Map.Id);
                if (prism != null)
                {
                    if (client.Character.AllianceId == prism.AllianceId)
                    {
                        if (CharacterGuildRecord.GetCharacterGuild(client.Character.Id).HasRight(Enums.GuildRightsBitEnum.GUILD_RIGHT_SET_ALLIANCE_PRISM))
                        {
                            client.Character.PrismStorageInstance = new PrismExchange(prism, client);
                            client.Character.PrismStorageInstance.OpenPanel();
                        }
                    }
                }
            }
        }

        [MessageHandler]
        public static void HandlePrismUseRequestMessage(PrismUseRequestMessage message, WorldClient client)
        {
            if(client.Character.HasGuild && client.Character.HasAlliance)
            {
                var prism = PrismRecord.GetPrismByMapId(client.Character.Map.Id);
                if(prism != null)
                {
                    if (client.Character.AllianceId == prism.AllianceId)
                    {
                        InteractiveActionProvider.Prism(client, prism);
                    }
                }
            }
        }

        [MessageHandler]
        public static void HandlePrismAttackRequestMessage(PrismAttackRequestMessage message, WorldClient client)
        {
            if(client.Character.HasGuild && client.Character.HasAlliance)
            {
                var prism = PrismRecord.GetPrismByMapId(client.Character.Map.Id);
                if (prism != null)
                {
                    if (client.Character.HasAlliance)
                    {
                        if (client.Character.Restrictions.isDead)
                        {
                            client.Character.Reply("Impossible en étant mort !");
                            return;
                        }
                        if (client.Character.Busy || client.Character.IsFighting)
                            return;
                        if (client.Character.AllianceId != prism.AllianceId)
                        {
                            PrismsManager.Instance.AttackPrism(client, prism);
                        }
                    }
                    /*if(prism.ParsedState == PrismStateEnum.PRISM_STATE_NORMAL)
                    {
                        PrismsManager.Instance.AttackPrism(client, prism);
                    }
                    else
                    {
                        if(prism.ParsedState == PrismStateEnum.PRISM_STATE_INVULNERABLE)
                        {
                            int minutesToWait = 60 - (int)(DateTime.Now - prism.PlacementDate.GetDateTimeFromEpoch()).TotalMinutes;
                            client.Send(new TextInformationMessage(1, 409, new string[] { minutesToWait.ToString() }));
                        }
                        else
                        {
                            client.Character.Reply("Impossible d'attaquer ce prisme pour le moment.", Color.Orange);
                        }
                    }*/
                }
            }
            else
            {
                client.Character.Reply("Vous devez appartenir à une alliance pour pouvoir attaquer ce prisme.", Color.Orange);
            }
        }

        #region Send
        public static void SendSettingsErrorMessage(WorldClient client)
        {
            client.Send(new PrismSettingsErrorMessage());
        }

        public static void SendPrismListUpdateMessage(bool sendOnlyToAlliance = false, int? allianceId = null)
        {
            if(!sendOnlyToAlliance)
            {
                WorldServer.Instance.GetAllClientsOnline().ForEach(x => x.Send(new PrismsListUpdateMessage(PrismsManager.Instance.GetWorldPrismGeolocalizedInformation(x))));
            }
            else
            {
                if (allianceId != null)
                {
                    AllianceRecord allianceRecord = AllianceRecord.GetAlliance(allianceId.Value);
                    if(allianceRecord != null)
                    {
                        WorldServer.Instance.GetAllClientsOnline().ForEach((x) =>
                        {
                            if(x.Character.HasGuild && x.Character.HasAlliance)
                            {
                                if(x.Character.AllianceId == allianceId.Value)
                                {
                                    x.Send(new PrismsListUpdateMessage(PrismsManager.Instance.GetWorldPrismGeolocalizedInformation(x)));
                                }
                            }
                        });
                    }
                }
                else
                {
                    //throw new Exception("Unable to find 'allianceId' when accessing SendPrismListUpdateMessage()");
                }
            }
            
        }
        #endregion
    }
}
