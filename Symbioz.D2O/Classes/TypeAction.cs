// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class TypeAction : ID2OClass
    {
        [Cache]
        public static List<TypeAction> TypeActions = new List<TypeAction>();
        public Int32 id;
        public String elementName;
        public Int32 elementId;
        public TypeAction(Int32 id, String elementName, Int32 elementId)
        {
            this.id = id;
            this.elementName = elementName;
            this.elementId = elementId;
        }
    }
}
