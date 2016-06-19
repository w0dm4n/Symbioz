using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.PathProvider;
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

        public Fighter GetFighterGoodByResistance(List<Fighter> lst)
        {
            Fighter result = null;
            int currentvalue = 0;

            if (lst == null || lst.Count <= 0)
                return null;
            foreach (Fighter f in lst)
            {
                int value = 0;

                if (f is MonsterFighter)
                    if ((f as MonsterFighter).tree != null && lst.Count != 1)
                        continue;
                value -= f.FighterStats.ErosionPercentage;
                value += f.FighterStats.Stats.AirResistPercent;
                value += f.FighterStats.Stats.EarthResistPercent;
                value += f.FighterStats.Stats.FireResistPercent;
                value += f.FighterStats.Stats.WaterResistPercent;
                if (result == null || value < currentvalue)
                {
                    result = f;
                    currentvalue = value;
                }
            }
            if (result != null)
                return (result);
            return lst.First();
        }

        public Fighter LowerProchFighter(Fighter searcher, int pmOrpo)
        {

            var sorted = m_fighters.Alives().OrderByDescending(x => x.FighterStats.Stats.LifePoints);

            var selected = sorted;
            List<Fighter> result = new List<Fighter>();
            List<Fighter> potential = new List<Fighter>();
            foreach (Fighter f in selected)
            {
                int nbr = PathHelper.GetDistanceBetween(searcher.CellId, f.CellId);
                if (nbr <= pmOrpo && f is MonsterFighter)
                {
                    MonsterFighter mob = f as MonsterFighter;
                    if (mob.isSummon)
                        result.Add(f);
                    else
                        potential.Add(f);
                }
                else if (nbr <= pmOrpo)
                    potential.Add(f);
            }
            if (potential.Count > 0)
                return (GetFighterGoodByResistance(potential));
            if (result.Count <= 0)
            {
                result = new List<Fighter>();
                foreach (Fighter f in selected)
                {
                    int nbr = PathHelper.GetDistanceBetween(searcher.CellId, f.CellId);
                    if (f is MonsterFighter)
                    {
                        MonsterFighter mob = f as MonsterFighter;
                        if (!mob.isSummon)
                            result.Add(f);
                    }
                    else
                        result.Add(f);
                }
                if (result.Count > 0)
                    return GetFighterGoodByResistance(result);
                return sorted.Count() > 0 ? sorted.Last() : null;
            }
            return GetFighterGoodByResistance(result);
        }

        public Fighter LowerProchAlignFighter(Fighter searcher, int pmOrpo)
        {

            var sorted = m_fighters.Alives().OrderByDescending(x => x.FighterStats.Stats.LifePoints);

            List<Fighter> selected = new List<Fighter>();
            List<short> lines = PathHelper.Getalldirectionlines(this.Fight, searcher.CellId);
            foreach (Fighter f in sorted)
            {
                foreach (short cell in lines)
                {
                    if (f.CellId == cell)
                    {
                        selected.Add(f);
                        break;
                    }
                }
            }
            if (selected.Count <= 0)
                selected = (List<Fighter>)sorted;
            List<Fighter> result = new List<Fighter>();
            foreach (Fighter f in selected)
            {
                int nbr = PathHelper.GetDistanceBetween(searcher.CellId, f.CellId);
                if (nbr <= pmOrpo && f is MonsterFighter)
                {
                    MonsterFighter mob = f as MonsterFighter;
                    if (mob.isSummon)
                        result.Add(f);
                    else
                        return (f);
                }
                else if (nbr <= pmOrpo)
                    return (f);
            }
            if (result.Count <= 0)
            {
                result = new List<Fighter>();
                foreach (Fighter f in selected)
                {
                    int nbr = PathHelper.GetDistanceBetween(searcher.CellId, f.CellId);
                    if (f is MonsterFighter)
                    {
                        MonsterFighter mob = f as MonsterFighter;
                        if (!mob.isSummon)
                            result.Add(f);
                    }
                    else
                        result.Add(f);
                }
                if (result.Count > 0)
                    return result.Count() > 0 ? result.Last() : null;
                return sorted.Count() > 0 ? sorted.Last() : null;
            }
            return result.Count() > 0 ? result.Last() : null;
        }

    }
}
