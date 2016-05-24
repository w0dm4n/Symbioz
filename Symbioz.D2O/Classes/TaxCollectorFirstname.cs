// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class TaxCollectorFirstname : ID2OClass
    {
        [Cache]
        public static List<TaxCollectorFirstname> TaxCollectorFirstnames = new List<TaxCollectorFirstname>();
        public Int32 id;
        public Int32 firstnameId;
        public TaxCollectorFirstname(Int32 id, Int32 firstnameId)
        {
            this.id = id;
            this.firstnameId = firstnameId;
        }
    }
}
