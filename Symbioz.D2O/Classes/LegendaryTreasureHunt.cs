// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class LegendaryTreasureHunt : ID2OClass
    {
        [Cache]
        public static List<LegendaryTreasureHunt> LegendaryTreasureHunts = new List<LegendaryTreasureHunt>();
        public Int32 id;
        public Int32 nameId;
        public Int32 level;
        public UInt32 chestId;
        public UInt32 monsterId;
        public UInt32 mapItemId;
        public Double xpRatio;
        public LegendaryTreasureHunt(Int32 id, Int32 nameId, Int32 level, UInt32 chestId, UInt32 monsterId, UInt32 mapItemId, Double xpRatio)
        {
            this.id = id;
            this.nameId = nameId;
            this.level = level;
            this.chestId = chestId;
            this.monsterId = monsterId;
            this.mapItemId = mapItemId;
            this.xpRatio = xpRatio;
        }
    }
}
