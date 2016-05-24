// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class PresetIcon : ID2OClass
    {
        [Cache]
        public static List<PresetIcon> PresetIcons = new List<PresetIcon>();
        public Int32 id;
        public Int32 order;
        public PresetIcon(Int32 id, Int32 order)
        {
            this.id = id;
            this.order = order;
        }
    }
}
