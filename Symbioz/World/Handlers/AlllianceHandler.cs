using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.World.Models.Alliances;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    public class AlllianceHandler
    {
        [MessageHandler]
        public static void HandleAllianceCreationValidMessage(AllianceCreationValidMessage message, WorldClient client)
        {
            if(AllianceProvider.CanCreateAlliance(client, message))
            {
                AllianceProvider.CreateAlliance(client.Character.GetGuild(), message.allianceTag, message.allianceName, message.allianceEmblem);
                client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_OK));
            }
        }

        [MessageHandler]
        public static void HandleAllianceInsiderInfoRequestMessage(AllianceInsiderInfoRequestMessage message, WorldClient client)
        {
            if(client.Character.HasAlliance)
            {
                client.Send(AllianceProvider.GetAllianceInsiderInfoMessage(client.Character.GetAlliance()));
            }
        }

        [MessageHandler]
        public static void HandleAllianceInvitationMessage(AllianceInvitationMessage message, WorldClient client)
        {
            if (client.Character.HasAlliance)
            {
                var target = WorldServer.Instance.GetOnlineClient((int)message.targetId);
                if (target != null)
                {
                    if (target.Character.HasGuild)
                    {
                        var targetGuild = target.Character.GetGuild();
                        if (GuildProvider.GetLeader(targetGuild.Id).CharacterId == target.Character.Id)
                        {
                            if (!targetGuild.IsInAlliance)
                            {
                                AllianceInvitationDialog allianceInvitationDialog = new AllianceInvitationDialog(client, target);
                                allianceInvitationDialog.Request();
                            }
                            else
                            {
                                client.Character.Reply("Impossible d'inviter cette guilde dans votre alliance car elle appartient déjà à une autre alliance.");
                            }
                        }
                    }
                    else
                    {
                        client.Character.Reply("Impossible d'inviter ce joueur dans votre alliance car il n'appartient à aucune guilde.");
                    }
                }
                else
                {
                    client.Character.Reply(ConstantsRepertory.UNKKNOWN_OR_OFFLINE_CHARACTER);
                }
            }
        }

        [MessageHandler]
        public static void HandleAllianceInvitationAnswerMessage(AllianceInvitationAnswerMessage message, WorldClient client)
        {
            if(client.Character.AllianceInvitationDialog != null)
            {
                client.Character.AllianceInvitationDialog.Answer(message.accept);
            }
        }

        [MessageHandler]
        public static void HandleAllianceKickRequestMessage(AllianceKickRequestMessage message, WorldClient client)
        {
            if(client.Character.HasAlliance)
            {
                if (client.Character.HasGuild)
                {
                    var characterGuild = client.Character.GetGuild();
                    var targetGuild = GuildRecord.GetGuild((int)message.kickedId);

                    if (GuildProvider.IsLeader(client.Character.Id, characterGuild.Id))
                    {
                        var characterAlliance = client.Character.GetAlliance();

                        if (AllianceProvider.GuildIsInAlliance(targetGuild.Id, characterAlliance.Id))
                        {
                            if (AllianceProvider.IsLeader(client.Character.Id, characterGuild.Id, characterAlliance.Id))
                            {
                                if (characterAlliance.LeaderGuildId == targetGuild.Id)
                                {
                                    AllianceProvider.DeleteAlliance(characterAlliance.Id);
                                }
                                else
                                {
                                    characterAlliance.Send(new AllianceGuildLeavingMessage(true, message.kickedId));
                                    AllianceProvider.LeaveAlliance((int)message.kickedId);
                                    targetGuild.SendChatMessage(string.Format("Votre guilde vient d'être exclue de l'alliance par <b>{0}</b>.",
                                        client.Character.Record.Name));
                                    characterAlliance.SendChatMessage(string.Format("La guilde <b>{0}</b> vient d'être exclue de l'alliance par <b>{1}</b>.",
                                        targetGuild.Name, client.Character.Record.Name));
                                    characterAlliance.Send(AllianceProvider.GetAllianceInsiderInfoMessage(characterAlliance));
                                }
                            }
                            else
                            {
                                if (characterGuild.Id == targetGuild.Id)
                                {
                                    characterAlliance.Send(new AllianceGuildLeavingMessage(false, message.kickedId));
                                    AllianceProvider.LeaveAlliance((int)message.kickedId);
                                    characterAlliance.SendChatMessage(string.Format("La guilde <b>{0}</b> vient de quitter l'alliance.", targetGuild.Name));
                                    characterAlliance.Send(AllianceProvider.GetAllianceInsiderInfoMessage(characterAlliance));
                                }
                            }
                        }
                    }
                }
            }
        }

        [MessageHandler]
        public static void HandleAllianceChangeGuildRightsMessage(AllianceChangeGuildRightsMessage message, WorldClient client)
        {
            if(client.Character.HasAlliance && client.Character.HasGuild)
            {
                var characterGuild = client.Character.GetGuild();
                var characterAlliance = client.Character.GetAlliance();
                var targetGuild = GuildRecord.GetGuild((int)message.guildId);

                if (targetGuild != null)
                {
                    if (AllianceProvider.IsLeader(client.Character.Id, characterGuild.Id, characterAlliance.Id))
                    {
                        if (AllianceProvider.GuildIsInAlliance(targetGuild.Id, characterAlliance.Id))
                        {
                            characterAlliance.LeaderGuildId = targetGuild.Id;
                            client.Character.Reply(string.Format("Vous avez cédé les droits de votre alliance à la guilde <b>{0}</b>.", targetGuild.Name), false, true);
                            characterAlliance.Send(AllianceProvider.GetAllianceInsiderInfoMessage(characterAlliance));
                            characterAlliance.SendChatMessage(string.Format("La guilde <b>{0}</b> dirige désormais l'alliance. Espérons que ces membres seront à la hauteur !", targetGuild.Name));
                        }
                        else
                        {
                            client.Character.Reply("Impossible d'assigner ce droit à cette guilde car elle n'est pas membre de votre alliance.");
                        }
                    }
                }
            }
        }

        [MessageHandler]
        public static void HandleAllianceFactsRequestMessage(AllianceFactsRequestMessage message, WorldClient client)
        {
            var targetAlliance = AllianceProvider.GetAlliance((int)message.allianceId);
            if(targetAlliance != null)
            {
                client.Send(AllianceProvider.GetAllianceFactsMessage(targetAlliance));
            }
            else
            {
                client.Send(new AllianceFactsErrorMessage(message.allianceId));
            }
        }
    }
}
