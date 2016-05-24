using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights
{
    public class TimeLine
    {
        public delegate void NewRoundDelegate(uint round);
        public event NewRoundDelegate OnNewRound;

        internal List<Fighter> m_fighters = new List<Fighter>();

        internal uint m_round = 1;

        internal int m_currentIndex = 0;

        public void Add(Fighter fighter)
        {
            m_fighters.Add(fighter);
            Sort();
        }
        public void Insert(Fighter inserted,Fighter master)
        {
            int index = m_fighters.IndexOf(master);
            m_fighters.Insert(index + 1,inserted);
        }
        public void Sort()
        {
            m_fighters = m_fighters.OrderByDescending(x => x.GetInitiative()).ToList();
        }
        public Fighter GetPlayingFighter()
        {
            if (m_currentIndex >= 0)
                return m_fighters[m_currentIndex];
            else
            {
                if (m_fighters.Count > 0)
                    return m_fighters[0];
                else
                    return null;
            }
        }
        public void StartFight()
        {
            OnNewRound(m_round);
        }
        public Fighter GetFirstFighter()
        {
            return m_fighters[0];
        }
        public void RemoveFighter(Fighter fighter)
        {
            var removed = m_fighters.Find(x => x == fighter);
            if (removed == null)
                return;
            int num = this.m_fighters.IndexOf(fighter);
            m_fighters.Remove(fighter);
            if (num <= m_currentIndex)
            {
                this.m_currentIndex--;
            }
            
        }
        public Fighter PopNextFighter()
        {
            m_currentIndex++;
            if (m_currentIndex == m_fighters.Count())
            {
                m_round++;
                if (OnNewRound != null)
                OnNewRound(m_round);
                m_currentIndex = 0;
            }
            if (m_currentIndex == -1)
                m_currentIndex++;
          
            return m_fighters[m_currentIndex];
        }
        public int[] GenerateTimeLine(bool sort = true)
        {
            if (sort)
            Sort();
            return m_fighters.ConvertAll<int>(x => x.ContextualId).ToArray();
        }
        
    }
}
