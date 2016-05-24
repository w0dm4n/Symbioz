// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Dungeon : ID2OClass
    {
        [Cache]
        public static List<Dungeon> Dungeons = new List<Dungeon>();
        public Int32 id;
        public Int32 nameId;
        public Int32 optimalPlayerLevel;
        public Int32[] mapIds;
        public Int32 entranceMapId;
        public Int32 exitMapId;
        public Dungeon(Int32 id, Int32 nameId, Int32 optimalPlayerLevel, Int32[] mapIds, Int32 entranceMapId, Int32 exitMapId)
        {
            this.id = id;
            this.nameId = nameId;
            this.optimalPlayerLevel = optimalPlayerLevel;
            this.mapIds = mapIds;
            this.entranceMapId = entranceMapId;
            this.exitMapId = exitMapId;
        }
    }
}
