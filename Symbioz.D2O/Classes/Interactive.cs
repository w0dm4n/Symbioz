// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Interactive : ID2OClass
    {
        [Cache]
        public static List<Interactive> Interactives = new List<Interactive>();
        public Int32 id;
        public Int32 nameId;
        public Int32 actionId;
        public Boolean displayTooltip;
        public Interactive(Int32 id, Int32 nameId, Int32 actionId, Boolean displayTooltip)
        {
            this.id = id;
            this.nameId = nameId;
            this.actionId = actionId;
            this.displayTooltip = displayTooltip;
        }
    }
}
