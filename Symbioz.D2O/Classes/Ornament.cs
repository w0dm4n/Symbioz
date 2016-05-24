// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Ornament : ID2OClass
    {
        [Cache]
        public static List<Ornament> Ornaments = new List<Ornament>();
        public Int32 id;
        public Int32 nameId;
        public Boolean visible;
        public Int32 assetId;
        public Int32 iconId;
        public Int32 rarity;
        public Int32 order;
        public Ornament(Int32 id, Int32 nameId, Boolean visible, Int32 assetId, Int32 iconId, Int32 rarity, Int32 order)
        {
            this.id = id;
            this.nameId = nameId;
            this.visible = visible;
            this.assetId = assetId;
            this.iconId = iconId;
            this.rarity = rarity;
            this.order = order;
        }
    }
}
