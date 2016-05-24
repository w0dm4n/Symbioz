// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class ItemSet : ID2OClass
    {
        [Cache]
        public static List<ItemSet> ItemSets = new List<ItemSet>();
        public Int32 id;
        public UInt32[] items;
        public Int32 nameId;
        public Boolean bonusIsSecret;
        public ArrayList[] effects;
        public ItemSet(Int32 id, UInt32[] items, Int32 nameId, Boolean bonusIsSecret, ArrayList[] effects)
        {
            this.id = id;
            this.items = items;
            this.nameId = nameId;
            this.bonusIsSecret = bonusIsSecret;
            this.effects = effects;
        }
    }
}
