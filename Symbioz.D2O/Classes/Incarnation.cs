// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Incarnation : ID2OClass
    {
        [Cache]
        public static List<Incarnation> Incarnations = new List<Incarnation>();
        public Int32 id;
        public String lookMale;
        public String lookFemale;
        public Incarnation(Int32 id, String lookMale, String lookFemale)
        {
            this.id = id;
            this.lookMale = lookMale;
            this.lookFemale = lookFemale;
        }
    }
}
