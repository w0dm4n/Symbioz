using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Alliances
{
    public class AllianceInvitationDialog : IDisposable
    {
        public WorldClient Recruter { get; set; }

        public WorldClient Recruted { get; set; }

        public AllianceInvitationDialog(WorldClient recruter, WorldClient recruted)
        {
            this.Recruter = recruter;
            this.Recruted = recruted;
            this.Recruter.Character.CurrentDialogType = DialogTypeEnum.DIALOG_ALLIANCE_INVITATION;
            this.Recruter.Character.AllianceInvitationDialog = this;
            this.Recruted.Character.CurrentDialogType = DialogTypeEnum.DIALOG_ALLIANCE_INVITATION;
            this.Recruted.Character.AllianceInvitationDialog = this;

        }

        public void Request()
        {
            Recruted.Send(new AllianceInvitedMessage((uint)Recruter.Character.Id, Recruter.Character.Record.Name, Recruter.Character.GetAlliance().GetBasicNamedAllianceInformations()));
            Recruter.Send(new AllianceInvitationStateRecruterMessage(Recruted.Character.Record.Name, (sbyte)GuildInvitationStateEnum.GUILD_INVITATION_SENT));
        }

        public void Answer(bool accept)
        {
            if (accept)
            {
                var newAlliance = Recruter.Character.GetAlliance();
                newAlliance.SendChatMessage(string.Format("La guilde <b>{0}</b> vient de rejoindre l'alliance.", newAlliance.Name));
                AllianceProvider.JoinAlliance(Recruted.Character.GetGuild(), newAlliance);
                Recruter.Send(new AllianceInvitationStateRecrutedMessage((sbyte)GuildInvitationStateEnum.GUILD_INVITATION_OK));
            }
            else
            {
                Recruter.Send(new AllianceInvitationStateRecrutedMessage((sbyte)GuildInvitationStateEnum.GUILD_INVITATION_CANCELED));
            }
            this.Dispose();
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
