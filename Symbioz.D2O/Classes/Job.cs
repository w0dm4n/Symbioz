// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Job : ID2OClass
    {
        [Cache]
        public static List<Job> Jobs = new List<Job>();
        public Int32 id;
        public Int32 nameId;
        public Int32 iconId;
        public Job(Int32 id, Int32 nameId, Int32 iconId)
        {
            this.id = id;
            this.nameId = nameId;
            this.iconId = iconId;
        }
    }
}
