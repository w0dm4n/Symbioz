// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class PointOfInterest : ID2OClass
    {
        [Cache]
        public static List<PointOfInterest> PointOfInterests = new List<PointOfInterest>();
        public Int32 id;
        public Int32 nameId;
        public Int32 categoryId;
        public PointOfInterest(Int32 id, Int32 nameId, Int32 categoryId)
        {
            this.id = id;
            this.nameId = nameId;
            this.categoryId = categoryId;
        }
    }
}
