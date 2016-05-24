// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class MapScrollAction : ID2OClass
    {
        [Cache]
        public static List<MapScrollAction> MapScrollActions = new List<MapScrollAction>();
        public Int32 id;
        public Boolean rightExists;
        public Boolean bottomExists;
        public Boolean leftExists;
        public Boolean topExists;
        public Int32 rightMapId;
        public Int32 bottomMapId;
        public Int32 leftMapId;
        public Int32 topMapId;
        public MapScrollAction(Int32 id, Boolean rightExists, Boolean bottomExists, Boolean leftExists, Boolean topExists, Int32 rightMapId, Int32 bottomMapId, Int32 leftMapId, Int32 topMapId)
        {
            this.id = id;
            this.rightExists = rightExists;
            this.bottomExists = bottomExists;
            this.leftExists = leftExists;
            this.topExists = topExists;
            this.rightMapId = rightMapId;
            this.bottomMapId = bottomMapId;
            this.leftMapId = leftMapId;
            this.topMapId = topMapId;
        }
    }
}
