// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class CompanionSpell : ID2OClass
    {
        [Cache]
        public static List<CompanionSpell> CompanionSpells = new List<CompanionSpell>();
        public Int32 id;
        public Int32 spellId;
        public Int32 companionId;
        public String gradeByLevel;
        public CompanionSpell(Int32 id, Int32 spellId, Int32 companionId, String gradeByLevel)
        {
            this.id = id;
            this.spellId = spellId;
            this.companionId = companionId;
            this.gradeByLevel = gradeByLevel;
        }
    }
}
