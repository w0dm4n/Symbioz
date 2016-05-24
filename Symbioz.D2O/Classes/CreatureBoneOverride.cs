// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class CreatureBoneOverride : ID2OClass
    {
        [Cache]
        public static List<CreatureBoneOverride> CreatureBoneOverrides = new List<CreatureBoneOverride>();
        public Int32 boneId;
        public Int32 creatureBoneId;
        public CreatureBoneOverride(Int32 boneId, Int32 creatureBoneId)
        {
            this.boneId = boneId;
            this.creatureBoneId = creatureBoneId;
        }
    }
}
