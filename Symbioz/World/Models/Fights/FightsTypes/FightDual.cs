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

namespace Symbioz.World.Models.Fights
{
    public class FightDual : Fight
    {
        public int AcceptorId { get; set; }

        public int InitiatorId { get; set; }

        public short SecondFighterCellId { get; set; }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_CHALLENGE; }
        }

        public override bool AddFightSword
        {
            get { return true; }
        }
        public override bool SpawnJoin { get { return false; }}

        public override bool PvP { get { return true; } }

        public FightDual(int id,MapRecord map,FightTeam blueteam,FightTeam redteam,short fightcellid,short secondplayercellid):base(id,map,blueteam,redteam,fightcellid)
        {
            this.SecondFighterCellId = secondplayercellid;
        }
        public override FightCommonInformations GetFightCommonInformations()
        {
            List<FightTeamInformations> teams = new List<FightTeamInformations>();
            teams.Add(BlueTeam.GetFightTeamInformations());
            teams.Add(RedTeam.GetFightTeamInformations());
            List<ushort> positions = new List<ushort>();
            positions.Add((ushort)SecondFighterCellId);
            positions.Add((ushort)FightCellId);
            return new FightCommonInformations(Id, (sbyte)FightType, teams, positions, new List<FightOptionsInformations>() { BlueTeam.TeamOptions, RedTeam.TeamOptions });
        }
        public override void StartPlacement()
        {
            base.StartPlacement();
        }
        public override void TryJoin(WorldClient client, int mainfighterid)
        {
         
            var mainFighter = GetAllFighters().Find(x => x.ContextualId == mainfighterid);
          
            if (CanJoin(client, mainFighter.Team))
            {
                var newFighter = client.Character.CreateFighter(mainFighter.Team);
                mainFighter.Team.AddFighter(newFighter);
                GetAllFighters().ForEach(x => x.ShowFighter(client));
                Map.Instance.OnFighterAdded(Id, mainFighter.Team.Id, newFighter.GetFightMemberInformations());
            }
        }
        public override void ShowFightResults(List<FightResultListEntry> results,WorldClient client)
        {
            client.Send(new GameFightEndMessage((ushort)TimeLine.m_round,0, 0, results, new NamedPartyTeamWithOutcome[0]));
        }
    }
}
