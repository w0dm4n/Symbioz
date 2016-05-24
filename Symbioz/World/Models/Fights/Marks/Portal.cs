using Symbioz.Enums;
using Symbioz.PathProvider;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Marks
{
    public class Portal : MarkTrigger
    {
        public Portal(Fighter caster, short centercell, char shapetype, short radius, short associatedspellid, sbyte spellgrade, int markcolor)
            : base(caster, centercell, centercell, shapetype, radius, associatedspellid, spellgrade, markcolor)
        {
        }

        public override GameActionMarkTypeEnum MarkType
        {
            get { return GameActionMarkTypeEnum.PORTAL; }
        }
        [Interaction(FighterEventType.AFTER_MOVE)]
        public void AfterMove(Fighter fighter, object arg1, object arg2, object arg3)
        {

            var list = fighter.Fight.GetMarks<Portal>().OrderByDescending(x => PathHelper.GetDistanceBetween(this.CenterCell, x.CenterCell)).ToList();
            list.Remove(this);
            if (list.Count() > 0)
            {

                fighter.Teleport(list.First().CenterCell);
            }
        }
    }
}
