// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Characteristic : ID2OClass
    {
        [Cache]
        public static List<Characteristic> Characteristics = new List<Characteristic>();
        public Int32 id;
        public String keyword;
        public Int32 nameId;
        public String asset;
        public Int32 categoryId;
        public Boolean visible;
        public Int32 order;
        public Boolean upgradable;
        public Characteristic(Int32 id, String keyword, Int32 nameId, String asset, Int32 categoryId, Boolean visible, Int32 order, Boolean upgradable)
        {
            this.id = id;
            this.keyword = keyword;
            this.nameId = nameId;
            this.asset = asset;
            this.categoryId = categoryId;
            this.visible = visible;
            this.order = order;
            this.upgradable = upgradable;
        }
    }
}
