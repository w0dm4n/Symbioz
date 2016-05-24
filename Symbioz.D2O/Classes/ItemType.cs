// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class ItemType : ID2OClass
    {
        [Cache]
        public static List<ItemType> ItemTypes = new List<ItemType>();
        public Int32 id;
        public Int32 nameId;
        public Int32 superTypeId;
        public Boolean plural;
        public Int32 gender;
        public String rawZone;
        public Boolean mimickable;
        public Int32 craftXpRatio;
        public ItemType(Int32 id, Int32 nameId, Int32 superTypeId, Boolean plural, Int32 gender, String rawZone, Boolean mimickable, Int32 craftXpRatio)
        {
            this.id = id;
            this.nameId = nameId;
            this.superTypeId = superTypeId;
            this.plural = plural;
            this.gender = gender;
            this.rawZone = rawZone;
            this.mimickable = mimickable;
            this.craftXpRatio = craftXpRatio;
        }
    }
}
