// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Challenge : ID2OClass
    {
        [Cache]
        public static List<Challenge> Challenges = new List<Challenge>();
        public Int32 id;
        public Int32 nameId;
        public Int32 descriptionId;
        public Challenge(Int32 id, Int32 nameId, Int32 descriptionId)
        {
            this.id = id;
            this.nameId = nameId;
            this.descriptionId = descriptionId;
        }
    }
}
