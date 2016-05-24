using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class OnSadidaSummonDieEffect : Buff
    {
        public sbyte SpellGrade { get; set; }

        public OnSadidaSummonDieEffect(uint uid, short delta, short duration, int sourceid, short sourcespellid,sbyte spellGrade, int delay)
            : base(uid, delta, duration, sourceid, sourcespellid, delay)
        {
            this.SpellGrade = spellGrade;
        }
        public override FighterEventType EventType
        {
            get
            {
                return FighterEventType.BEFORE_DIED;
            }
        }
        public override void SetBuff()
        {
            
        }
        public override bool OnEventCalled(object arg1, object arg2, object arg3)
        {
            ReturnToTree();
            return base.OnEventCalled(arg1, arg2, arg3);
        }
        public override void RemoveBuff()
        {
           
        }
        /// <summary>
        /// Sadida, invocation arbre apres la mort
        /// </summary>
        private void ReturnToTree()
        {
            var summoned = Fighter as MonsterFighter;
            Fighter master = Fighter.Fight.GetFighter(summoned.FighterStats.SummonerId);
            master.Fight.AddSummon(master, 282, 1, Fighter.CellId, Fighter.Team);
        }
    }
}
