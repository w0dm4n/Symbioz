using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    public class TreeFighter
    {
        public Fighter Master { get; set; }

        public MonsterFighter Monster { get; set; }

        public sbyte Grade { get; set; }

        public bool isFlored { get; set; }

        public bool wait = false;

        public TreeFighter(Fighter master, FightTeam team, MonsterFighter monster, short cellid, sbyte grade)
        {
            this.Monster = monster;
            this.Master = master;
            this.Grade = grade;
            this.isFlored = false;
        }

        public void floor()
        {
            if (this.isFlored)
                return;
            if (wait)
            {
                wait = false;
                return;
            }
            World.Models.ContextActorLook lo = World.Models.ContextActorLook.Parse("{3164}");
            Monster.FighterLook = lo;
            Monster.Fight.TryStartSequence(Monster.ContextualId, 1);
            Monster.Fight.Send(new GameActionFightChangeLookMessage(0, Monster.ContextualId, Monster.ContextualId, lo.ToEntityLook()));
            Monster.Fight.TryEndSequence(1, 0);
            this.isFlored = true;
        }
    }
}
