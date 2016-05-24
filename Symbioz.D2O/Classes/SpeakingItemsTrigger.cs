// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SpeakingItemsTrigger : ID2OClass
    {
        [Cache]
        public static List<SpeakingItemsTrigger> SpeakingItemsTriggers = new List<SpeakingItemsTrigger>();
        public Int32 triggersId;
        public Int32[] textIds;
        public Int32[] states;
        public SpeakingItemsTrigger(Int32 triggersId, Int32[] textIds, Int32[] states)
        {
            this.triggersId = triggersId;
            this.textIds = textIds;
            this.states = states;
        }
    }
}
