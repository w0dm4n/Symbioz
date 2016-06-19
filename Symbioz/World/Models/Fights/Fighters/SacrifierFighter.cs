using Symbioz.DofusProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    public class SacrifierFighter
    {
        public Fighter Master { get; set; }

        public MonsterFighter Monster { get; set; }

        public sbyte Grade { get; set; }

        public bool isFlored { get; set; }

        public bool wait = false;

        public SacrifierFighter(Fighter master, FightTeam team, MonsterFighter monster, short cellid, sbyte grade)
        {
            this.Monster = monster;
            this.Master = master;
            this.Grade = grade;
        }
        
        public void Increment()
        {
            if (Monster.RealFighterLook.scales.Count == 0)
                Monster.RealFighterLook.scales.Add(100);
            if (Monster.RealFighterLook.scales[0] >= 300)
                return;
            Monster.FighterStats.Stats.Agility += (short)(150 * Grade);
            Monster.RealFighterLook.SetScale((short)(Monster.RealFighterLook.scales[0]+40));
            Monster.FighterLook = Monster.RealFighterLook;
            Monster.Fight.TryStartSequence(Monster.ContextualId, 1);
            Monster.Fight.Send(new GameActionFightChangeLookMessage(0, Monster.ContextualId, Monster.ContextualId, Monster.FighterLook.ToEntityLook()));
            Monster.Fight.TryEndSequence(1, 0);
        }
    }
}
