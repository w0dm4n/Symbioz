// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SpellBomb : ID2OClass
    {
        [Cache]
        public static List<SpellBomb> SpellBombs = new List<SpellBomb>();
        public Int32 id;
        public Int32 chainReactionSpellId;
        public Int32 explodSpellId;
        public Int32 wallId;
        public Int32 instantSpellId;
        public Int32 comboCoeff;
        public SpellBomb(Int32 id, Int32 chainReactionSpellId, Int32 explodSpellId, Int32 wallId, Int32 instantSpellId, Int32 comboCoeff)
        {
            this.id = id;
            this.chainReactionSpellId = chainReactionSpellId;
            this.explodSpellId = explodSpellId;
            this.wallId = wallId;
            this.instantSpellId = instantSpellId;
            this.comboCoeff = comboCoeff;
        }
    }
}
