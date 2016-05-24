// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class LivingObjectSkinJntMood : ID2OClass
    {
        [Cache]
        public static List<LivingObjectSkinJntMood> LivingObjectSkinJntMoods = new List<LivingObjectSkinJntMood>();
        public Int32 skinId;
        public ArrayList[] moods;
        public LivingObjectSkinJntMood(Int32 skinId, ArrayList[] moods)
        {
            this.skinId = skinId;
            this.moods = moods;
        }
    }
}
