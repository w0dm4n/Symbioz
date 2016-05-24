// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class CompanionCharacteristic : ID2OClass
    {
        [Cache]
        public static List<CompanionCharacteristic> CompanionCharacteristics = new List<CompanionCharacteristic>();
        public Int32 id;
        public Int32 caracId;
        public Int32 companionId;
        public Int32 order;
        public Int32 initialValue;
        public Int32 levelPerValue;
        public Int32 valuePerLevel;
        public CompanionCharacteristic(Int32 id, Int32 caracId, Int32 companionId, Int32 order, Int32 initialValue, Int32 levelPerValue, Int32 valuePerLevel)
        {
            this.id = id;
            this.caracId = caracId;
            this.companionId = companionId;
            this.order = order;
            this.initialValue = initialValue;
            this.levelPerValue = levelPerValue;
            this.valuePerLevel = valuePerLevel;
        }
    }
}
