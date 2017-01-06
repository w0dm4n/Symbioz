using Symbioz.Providers.ActorIA.Actions;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA
{
    public class MonsterBrain
    {
        List<AbstractIAAction> Actions = new List<AbstractIAAction>();

        MonsterFighter m_fighter { get; set; }

        int m_action_Index = 0;
  
        public MonsterBrain(MonsterFighter fighter, List<int> actions)
        {
            this.m_fighter = fighter;
            this.Actions = actions.ConvertAll<AbstractIAAction>(x => IAActionsProvider.GetAction((IAActionsEnum)x));
        }

        protected void NextAction()
        {
            try
            {
                Actions[m_action_Index].Execute(m_fighter);
            }
            catch (Exception ex)
            {
                Logger.Error("Actor AI error " + ex);
            }
            finally
            {
                //pass a l'action suivante dans tout les cas
                OnActionEnded();
            }
        }
        protected void OnActionEnded()
        {
            try
            {
                m_action_Index++;
                //Si index action >= Count() return
                if (Actions.Count() <= m_action_Index)
                {
                    m_action_Index = 0;
                    return;
                }
                if (!m_fighter.Dead && !m_fighter.Fight.Ended)
                {
                    NextAction();
                }
            }
            catch (Exception error)
            {
                Logger.Error(error);
            }
        }
        public void StartPlay()
        {
            try
            {
                int action = 0;
                while (action <= 4)
                {
                    NextAction();
                    action++;
                }
            }
            catch (Exception error)
            {
                Logger.Error(error);
            }
        }
    }
}
