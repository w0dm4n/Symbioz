using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights
{
    public class FightTeam
    {
        public sbyte Id { get; set; }
        public TeamTypeEnum TeamType { get; set; }
        public TeamColorEnum TeamColor { get; set; }
        public FightOptionsInformations TeamOptions = new FightOptionsInformations(false, false, false, false);
        public FightTeam(sbyte id, List<short> placementcells, TeamColorEnum teamcolor, TeamTypeEnum teamtype)
        {
            this.PlacementCells = placementcells;
            this.TeamColor = teamcolor;
            this.TeamType = teamtype;
            this.Id = id;
        }
        List<Fighter> m_fighters = new List<Fighter>();
        public List<short> PlacementCells = new List<short>();
        public Fight Fight { get; set; }
        public int FightersCount { get { return m_fighters.Count; } }
        public List<CharacterFighter> GetCharacterFighters(bool deaths = false)
        {
            return GetFighters(deaths).FindAll(x => x is CharacterFighter).ConvertAll<CharacterFighter>(x => (CharacterFighter)x);
        }
        public void Dispose()
        {
            m_fighters.Clear();
        }
        public List<Fighter> GetFighters(bool deaths = false)
        {
            if (!deaths)
                return m_fighters.FindAll(x => !x.Dead);
            else
                return m_fighters;
        }
        public void Send(Message message)
        {
            GetCharacterFighters(true).ForEach(x => x.Client.Send(message));
        }
        public void AddSummon(Fighter fighter)
        {
            m_fighters.Add(fighter);
            fighter.Fight = Fight;
        }
        public void AddBomb(BombFighter fighter)
        {
            m_fighters.Add(fighter);
            fighter.Fight = Fight;
        }
        public void AddFighter(Fighter fighter)
        {
            m_fighters.Add(fighter);
            fighter.Fight = Fight;
            fighter.OnAdded();
            Fight.UpdateTeam(this);
            fighter.ShowFighter(); // ?????
        }
        public void RemoveFighter(Fighter fighter)
        {
            Fight.TimeLine.m_fighters.Remove(fighter);
            m_fighters.Remove(fighter);
        }
        public FightTeamInformations GetFightTeamInformations()
        {
            List<FightTeamMemberInformations> members = new List<FightTeamMemberInformations>();
            m_fighters.ForEach(x => members.Add(x.GetFightMemberInformations()));
            var team = new FightTeamInformations(Id, m_fighters[0].ContextualId, (sbyte)TeamColor, (sbyte)TeamType, 0, members);
            return team;

        }
        public Fighter LowerFighter()
        {
            var sorted = m_fighters.Alives().OrderByDescending(x => x.FighterStats.Stats.LifePoints);
            return sorted.Count() > 0 ? sorted.Last() : null;
        }

    }
}
