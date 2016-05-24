// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class CensoredContent : ID2OClass
    {
        [Cache]
        public static List<CensoredContent> CensoredContents = new List<CensoredContent>();
        public String lang;
        public Int32 type;
        public Int32 oldValue;
        public Int32 newValue;
        public CensoredContent(String lang, Int32 type, Int32 oldValue, Int32 newValue)
        {
            this.lang = lang;
            this.type = type;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }
}
