// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class VeteranReward : ID2OClass
    {
        [Cache]
        public static List<VeteranReward> VeteranRewards = new List<VeteranReward>();
        public Int32 id;
        public UInt32 requiredSubDays;
        public UInt32 itemGID;
        public UInt32 itemQuantity;
        public VeteranReward(Int32 id, UInt32 requiredSubDays, UInt32 itemGID, UInt32 itemQuantity)
        {
            this.id = id;
            this.requiredSubDays = requiredSubDays;
            this.itemGID = itemGID;
            this.itemQuantity = itemQuantity;
        }
    }
}
