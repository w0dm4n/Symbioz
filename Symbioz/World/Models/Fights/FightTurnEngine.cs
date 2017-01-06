using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.World.Models.Fights
{
    public class FightTurnEngine
    {
        public const int TURN_TIMEMOUT = 35000;

        public Timer m_timer { get; set; }
        public Fighter m_fighter { get; set; }

        public FightTurnEngine(Fighter fighter)
        {
            this.m_fighter = fighter;
        }
        public void StartTurn()
        {
            m_timer = new Timer(TURN_TIMEMOUT);
            m_timer.Elapsed += m_timer_Elapsed;
            m_timer.Enabled = true;
        }
        public void EndTurn()
        {
            if (m_timer != null)
            {
                m_timer.Stop();
                m_timer.Dispose();
            }
        }
        void m_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(this.m_fighter != null)
            {
                m_fighter.EndTurn();
            }
        }

        ~FightTurnEngine()
        {
           if(this.m_timer != null)
            {
                this.m_timer.Stop();
                this.m_timer.Dispose();
            }
        }
    }
}
