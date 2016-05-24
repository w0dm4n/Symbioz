using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Enums;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Records;
using Symbioz.Network.Servers;

namespace Symbioz.World.Handlers
{
    public class GuildsHandler
    {
        [MessageHandler]
        public static void HandleGuildCreationRequest(GuildCreationValidMessage message, WorldClient client)
        {
            if (GuildProvider.Instance.HasGuild(client.Character.Id))
            {
                client.Send(new GuildCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD));
                return;
            }
            if (GuildRecord.CanCreateGuild(message.guildName))
            {
                GuildRecord newGuild = GuildProvider.Instance.CreateGuild(client.Character, message);
                client.Send(new GuildCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_OK));
            }
            else
            {
                client.Send(new GuildCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_ALREADY_EXISTS));
            }
        }
        [MessageHandler]
        public static void HandleGuildInvitationMessage(GuildInvitationMessage message, WorldClient client)
        {
            WorldClient target = WorldServer.Instance.GetOnlineClient((int)message.targetId);

            GuildInvitationDialog dialog = new GuildInvitationDialog(client, target);

            dialog.Request();
        }
        [MessageHandler]
        public static void HandleGuildKickRequestMessage(GuildKickRequestMessage message, WorldClient client)
        {
            GuildProvider.Instance.LeaveGuild(WorldServer.Instance.GetOnlineClient((int)message.kickedId).Character);
        }
        [MessageHandler]
        public static void HandleGuildInvitationAnswerMessage(GuildInvitationAnswerMessage message, WorldClient client)
        {
            client.Character.GuildInvitationDialog.Answer(message.accept);
        }
        [MessageHandler]
        public static void HandleGuildChangeMemberParameters(GuildChangeMemberParametersMessage message, WorldClient client)
        {
            CharacterGuildRecord member = CharacterGuildRecord.GetCharacterGuild((int)message.memberId);
            member.ChangeParameters(client, message.rank, message.experienceGivenPercent, message.rights);
            SendGuildInformationsMembers(client);
            if (WorldServer.Instance.IsConnected(member.CharacterId))
            {
                WorldClient c = WorldServer.Instance.GetOnlineClient(member.CharacterId);
                c.Send(new GuildMembershipMessage(c.Character.GetGuild().GetGuildInformations(),message.rights,true));
            }
        }
        [MessageHandler]
        public static void HandleGuildGetInformations(GuildGetInformationsMessage message, WorldClient client)
        {
            switch ((GuildInformationsTypeEnum)message.infoType)
            {
                case GuildInformationsTypeEnum.INFO_GENERAL:
                    SendGuildInformationsGeneral(client);
                    break;
                case GuildInformationsTypeEnum.INFO_MEMBERS:
                    SendGuildInformationsMembers(client);
                    break;
                case GuildInformationsTypeEnum.INFO_BOOSTS:
                    break;
                case GuildInformationsTypeEnum.INFO_PADDOCKS:
                    break;
                case GuildInformationsTypeEnum.INFO_HOUSES:
                    break;
                case GuildInformationsTypeEnum.INFO_TAX_COLLECTOR_GUILD_ONLY:
                    break;
                case GuildInformationsTypeEnum.INFO_TAX_COLLECTOR_ALLIANCE:
                    break;
                case GuildInformationsTypeEnum.INFO_TAX_COLLECTOR_LEAVE:
                    break;
            }
        }

        #region GuildInformations

        public static void SendGuildInformationsMembers(WorldClient client)
        {
            var members = CharacterGuildRecord.GetMembers(client.Character.GuildId);
            client.Send(new GuildInformationsMembersMessage(
                       members));
        }

        public static void SendGuildInformationsGeneral(WorldClient client)
        {
            GuildRecord guild = GuildRecord.GetGuild(client.Character.GuildId);

            ulong expFloor = ExperienceRecord.GetExperienceForGuild((ushort)(guild.Level));

            ulong expNextFloor = ExperienceRecord.GetExperienceForGuild((ushort)(guild.Level + 1));

            client.Send(new GuildInformationsGeneralMessage(true, false,
                (byte)guild.Level, expFloor, guild.Experience, expNextFloor, 0,
                (ushort)CharacterGuildRecord.MembersCount(guild.Id),
                (ushort)GuildProvider.Instance.ConnectedMembersCount(guild.Id)));
        }

        #endregion

    }
}
