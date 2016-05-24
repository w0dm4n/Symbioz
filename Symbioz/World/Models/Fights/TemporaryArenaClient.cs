using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights
{
    public class TemporaryArenaClient
    {
        public PvpArenaStepEnum ActualStep { get; set; }

        public WorldClient WorldClient { get; set; }

        public TeamColorEnum TeamColor { get; set; }

        public bool Ready { get; set; }

        public bool Master { get; set; }

        public bool Requested { get; set; }
        public TemporaryArenaClient(WorldClient client, TeamColorEnum teamColor, bool master)
        {
            this.WorldClient = client;
            this.TeamColor = teamColor;
            this.Ready = false;
            this.Master = master;
        }

        public void RequestFight(List<int> allieIds)
        {
            WorldClient.Send(new GameRolePlayArenaFightPropositionMessage(0, allieIds, ArenaProvider.ARENA_REQUEST_TIMOUT));
            Requested = true;
        }

        public void UpdateFighterStatus(List<TemporaryArenaClient> team)
        {
            team.ForEach(x => x.WorldClient.Send(new GameRolePlayArenaFighterStatusMessage(0, WorldClient.Character.Id, Ready)));
        }

        public void UpdateRegistrationStatus(bool registered, PvpArenaStepEnum step)
        {
            this.WorldClient.Send(new GameRolePlayArenaRegistrationStatusMessage(registered, (sbyte)step, ArenaProvider.FIGHTERS_PER_TEAM));
            this.ActualStep = step;
        }
        public void AbortRequest(int playerId)
        {
            WorldClient.Send(new GameRolePlayArenaFighterStatusMessage(0, playerId, false));
            Ready = false;
            WorldClient.Character.Reply("Un des joueur n'a pas accepter le kolizeum.");
        }
    }
}
