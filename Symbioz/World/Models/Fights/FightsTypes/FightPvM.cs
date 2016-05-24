using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Providers.FightResults;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.PathProvider;
using Symbioz.World.Records;
using Symbioz.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights
{
    public class FightPvM : Fight
    {
        public override FightTypeEnum FightType
        {
            get
            {
                return FightTypeEnum.FIGHT_TYPE_PvM;
            }
        }

        public override bool SpawnJoin { get { return true; } }

        public override bool PvP { get { return false; } }

        public override bool AddFightSword
        {
            get { return true; ; }
        }
        public MonsterGroup MonsterGroup { get; set; }
        public FightPvM(int id, MapRecord map, FightTeam blueteam, FightTeam redteam, short fightcellid, MonsterGroup group)
            : base(id, map, blueteam, redteam, fightcellid)
        {
            this.MonsterGroup = group;
        }
        public override void StartPlacement()
        {
            Map.Instance.RemoveMonsterGroup(MonsterGroup.MonsterGroupId);
            base.StartPlacement();
        }
        public override FightCommonInformations GetFightCommonInformations()
        {
            List<FightTeamInformations> teams = new List<FightTeamInformations>();
            teams.Add(BlueTeam.GetFightTeamInformations());
            teams.Add(RedTeam.GetFightTeamInformations());
            List<ushort> positions = new List<ushort>();
            positions.Add((ushort)FightCellId);
            positions.Add((ushort)Map.CloseCell(FightCellId));
            return new FightCommonInformations(Id, (sbyte)FightType, teams, positions, new List<FightOptionsInformations>() { BlueTeam.TeamOptions, RedTeam.TeamOptions });
        }
        public override void TryJoin(WorldClient client, int mainfighterid)
        {
            var mainFighter = GetAllFighters().Find(x => x.ContextualId == mainfighterid);
            if (mainFighter != null)
            {
                if (mainFighter.Team.TeamColor == TeamColorEnum.RED_TEAM)
                    return;
            }
            if (CanJoin(client, BlueTeam, mainFighter))
            {
                var newFighter = client.Character.CreateFighter(BlueTeam);
                BlueTeam.AddFighter(newFighter);
                GetAllFighters().ForEach(x => x.ShowFighter(client));
                Map.Instance.OnFighterAdded(Id, BlueTeam.Id, newFighter.GetFightMemberInformations());
            }
        }
        public override void ShowFightResults(List<FightResultListEntry> results, WorldClient client)
        {
            client.Send(new GameFightEndMessage((ushort)TimeLine.m_round, MonsterGroup.AgeBonus, 0, results, new NamedPartyTeamWithOutcome[0]));
        }


    }
}
