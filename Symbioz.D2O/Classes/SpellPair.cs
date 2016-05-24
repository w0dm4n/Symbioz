// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SpellPair : ID2OClass
    {
        [Cache]
        public static List<SpellPair> SpellPairs = new List<SpellPair>();
        public Int32 id;
        public Int32 nameId;
        public Int32 descriptionId;
        public Int32 iconId;
        public SpellPair(Int32 id, Int32 nameId, Int32 descriptionId, Int32 iconId)
        {
            this.id = id;
            this.nameId = nameId;
            this.descriptionId = descriptionId;
            this.iconId = iconId;
        }
    }
}
