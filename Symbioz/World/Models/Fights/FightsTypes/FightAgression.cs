using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.FightsTypes
{
    class FightAgression : Fight
    {
        public int AcceptorId { get; set; }

        public int InitiatorId { get; set; }

        public short SecondFighterCellId { get; set; }

        public override bool PvP { get { return true; } }

        public FightAgression(int id, MapRecord map, FightTeam blueteam, FightTeam redteam, short fightcellid, short secondplayercellid)
            : base(id, map, blueteam, redteam, fightcellid)
        {
            this.SecondFighterCellId = secondplayercellid;
        }

        public override FightTypeEnum FightType
        {
            get
            {
                return FightTypeEnum.FIGHT_TYPE_AGRESSION;
            }
        }

        public override bool AddFightSword
        {
            get
            {
                return true;
            }
        }

        public override FightCommonInformations GetFightCommonInformations()
        {
            List<FightTeamInformations> teams = new List<FightTeamInformations>();
            teams.Add(BlueTeam.GetFightTeamInformations());
            teams.Add(RedTeam.GetFightTeamInformations());
            List<ushort> positions = new List<ushort>();
            positions.Add((ushort)SecondFighterCellId);
            positions.Add((ushort)FightCellId);
            return new FightCommonInformations(Id, (sbyte)8, teams, positions, new List<FightOptionsInformations>() { BlueTeam.TeamOptions, RedTeam.TeamOptions });
        }

        public void StartFight(System.Timers.Timer Timer, FightAgression fight)
        {
            fight.StartFight();
            Timer.Enabled = false;
            Timer.Stop();
            Timer.Dispose();
        }

        public void StartPhaseReady()
        {
            var Timer = new System.Timers.Timer();
            Timer.Interval = 60000;
            Timer.Elapsed += (sender, e) => { StartFight(Timer, this); };
            Timer.Enabled = true;
        }

        public override void StartPlacement()
        {
            this.StartPhaseReady();
            base.StartPlacement();
        }
        public override void TryJoin(WorldClient client, int mainfighterid)
        {

            var mainFighter = GetAllFighters().Find(x => x.ContextualId == mainfighterid);

            if (CanJoin(client, mainFighter.Team, mainFighter))
            {
                var newFighter = client.Character.CreateFighter(mainFighter.Team, this);
                mainFighter.Team.AddFighter(newFighter);
                GetAllFighters().ForEach(x => x.ShowFighter(client));
                Map.Instance.OnFighterAdded(Id, mainFighter.Team.Id, newFighter.GetFightMemberInformations());
            }
        }
        public override void ShowFightResults(List<FightResultListEntry> results, WorldClient client)
        {
            client.Send(new GameFightEndMessage(this.GetFightDuration(), 0, 0, results, new NamedPartyTeamWithOutcome[0]));
        }
    }
}
