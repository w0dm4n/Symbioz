using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Guilds
{
    public class GuildInvitationDialog : IDisposable
    {
        public WorldClient Recruter { get; set; }

        public WorldClient Recruted { get; set; }

        public GuildInvitationDialog(WorldClient recruter, WorldClient recruted)
        {
            this.Recruter = recruter;
            this.Recruted = recruted;
            this.Recruter.Character.GuildInvitationDialog = this;
            this.Recruted.Character.GuildInvitationDialog = this;

        }
        public void Request()
        {
            Recruted.Send(new GuildInvitedMessage((uint)Recruter.Character.Id, Recruter.Character.Record.Name, Recruter.Character.GetGuild().GetBasicInformations()));
            Recruter.Send(new GuildInvitationStateRecruterMessage(Recruted.Character.Record.Name, (sbyte)GuildInvitationStateEnum.GUILD_INVITATION_SENT));
        }
        public void Answer(bool accept)
        {
            if (accept)
            {
                var newGuild = Recruter.Character.GetGuild();
                GuildProvider.Instance.JoinGuild(newGuild, Recruted.Character, GuildRightsBitEnum.GUILD_RIGHT_NONE, (ushort)GuildProvider.DEFAULT_JOIN_RANK);

                Recruter.Send(new GuildInvitationStateRecrutedMessage((sbyte)GuildInvitationStateEnum.GUILD_INVITATION_OK));
            }
            else
            {
                Recruter.Send(new GuildInvitationStateRecrutedMessage((sbyte)GuildInvitationStateEnum.GUILD_INVITATION_CANCELED));
            }
        }
        public void Dispose()
        {
            this.Recruter.Character.GuildInvitationDialog = null;
            this.Recruted.Character.GuildInvitationDialog = null;
            this.Recruted = null;
            this.Recruter = null;
        }
    }
}
