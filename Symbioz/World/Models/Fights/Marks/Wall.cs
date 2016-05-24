using Symbioz.Enums;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Marks
{
    public class Wall : MarkTrigger
    {
        public BombFighter FirstBomb { get; set; }

        public BombFighter SecondBomb { get; set; }

        public Wall(Fighter caster,short centercell,short entitycell,char shapetype,
            short radius,short associatedspellid,sbyte spellgrade,
            int markcolor,BombFighter firstBomb,BombFighter secondBomb):base(caster,centercell,entitycell,shapetype,radius,
            associatedspellid,spellgrade,markcolor)
        {
            this.FirstBomb = firstBomb;
            this.SecondBomb = secondBomb;

        }
        public override GameActionMarkTypeEnum MarkType
        {
            get { return GameActionMarkTypeEnum.WALL; }
        }

      
        [Interaction(FighterEventType.AFTER_MOVE)]
        public void AfterMove(Fighter fighter, object arg1, object arg2, object arg3)
        {
            fighter.TakeDamages(new Damages.TakenDamages(100, Damages.ElementType.Fire), Caster.ContextualId);
        }
        [Interaction(FighterEventType.ON_TURN_STARTED)]
        public void OnTurnStarted(Fighter fighter, object arg1, object arg2, object arg3)
        {
            fighter.TakeDamages(new Damages.TakenDamages(100, Damages.ElementType.Fire), Caster.ContextualId);
        }
        public override void OnCasted(IEnumerable<Fighter> affecteds)
        {
            foreach (var fighter in affecteds)
            {
                fighter.TakeDamages(new Damages.TakenDamages(100, Damages.ElementType.Water),Caster.ContextualId);
            }
        }
    }
}
