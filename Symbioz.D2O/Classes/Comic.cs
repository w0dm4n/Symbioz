// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Comic : ID2OClass
    {
        [Cache]
        public static List<Comic> Comics = new List<Comic>();
        public Int32 id;
        public String remoteId;
        public Comic(Int32 id, String remoteId)
        {
            this.id = id;
            this.remoteId = remoteId;
        }
    }
}
