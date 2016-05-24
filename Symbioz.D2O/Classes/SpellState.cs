// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SpellState : ID2OClass
    {
        [Cache]
        public static List<SpellState> SpellStates = new List<SpellState>();
        public Int32 id;
        public Int32 nameId;
        public Boolean preventsSpellCast;
        public Boolean preventsFight;
        public Boolean isSilent;
        public ArrayList effectsIds;
        public SpellState(Int32 id, Int32 nameId, Boolean preventsSpellCast, Boolean preventsFight, Boolean isSilent, ArrayList effectsIds)
        {
            this.id = id;
            this.nameId = nameId;
            this.preventsSpellCast = preventsSpellCast;
            this.preventsFight = preventsFight;
            this.isSilent = isSilent;
            this.effectsIds = effectsIds;
        }
    }
}
