// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class OptionalFeature : ID2OClass
    {
        [Cache]
        public static List<OptionalFeature> OptionalFeatures = new List<OptionalFeature>();
        public Int32 id;
        public String keyword;
        public OptionalFeature(Int32 id, String keyword)
        {
            this.id = id;
            this.keyword = keyword;
        }
    }
}
