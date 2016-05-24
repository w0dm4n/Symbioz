using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.World.Models.Fights
{
    public class ClientsSynchronizer
    {
        public const int Timeout = 5000;

        Action m_action { get; set; }
        Fight m_fight { get; set; }

        public Timer m_timout_timer { get; set; }
        public ClientsSynchronizer(Fight fight)
        {
            this.m_fight = fight;
            this.m_timout_timer = new Timer(Timeout);
            this.m_timout_timer.Elapsed += m_timout_timer_Elapsed;
        }

        void m_timout_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var laggingFighters = m_fight.GetAllCharacterFighters(true).FindAll(x => x.Available && !x.ReadyToSee);
            if (m_fight.TimeLine.m_fighters.Count > 0)
            {
                if (laggingFighters.Count > 0)
                {
                    if (laggingFighters.Count == 1)
                    {

                     //   m_fight.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 28, new string[] { laggingFighters[0].GetName() }));
                    }
                    else
                    {
                    //    string[] array = new string[1] { string.Join(",", from entry in laggingFighters select entry.GetName()) };

                    //    m_fight.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 29, array));
                    }
                }
            }
            m_timout_timer.Stop();
            m_action();

        }
        public void ToggleReady(CharacterFighter fighter)
        {
            fighter.ReadyToSee = true;
            if (m_fight.GetAllCharacterFighters(true).FindAll(x =>x.Available).All(x => x.ReadyToSee))
            {
                // Action
                m_fight.OnCharacterFighters(x => x.Character.FighterInstance.ReadyToSee = false);
                m_timout_timer.Stop();
                m_action();
            }
        }
        public void Start(Action action)
        {
            if (m_fight.Ended)
                return;
            this.m_action = action;
            if (m_fight.TimeLine.m_fighters.Count > 0)
            {
                m_fight.OnCharacterFighters(x => x.Send(new GameFightTurnReadyRequestMessage(m_fight.TimeLine.GetPlayingFighter().ContextualId)));
                m_timout_timer.Start();
            }
            else
            {
                m_fight.OnCharacterFighters(x => x.Send(new GameFightTurnReadyRequestMessage(0)));
                m_timout_timer.Start();
            }

        }
    }
}
