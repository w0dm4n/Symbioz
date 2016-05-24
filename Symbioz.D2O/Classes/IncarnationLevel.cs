// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class IncarnationLevel : ID2OClass
    {
        [Cache]
        public static List<IncarnationLevel> IncarnationLevels = new List<IncarnationLevel>();
        public Int32 id;
        public Int32 incarnationId;
        public Int32 level;
        public Int32 requiredXp;
        public IncarnationLevel(Int32 id, Int32 incarnationId, Int32 level, Int32 requiredXp)
        {
            this.id = id;
            this.incarnationId = incarnationId;
            this.level = level;
            this.requiredXp = requiredXp;
        }
    }
}
