// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class AnimFunMonsterData : ID2OInternalClass
    {
       
        public String animName;
        public Int32 animWeight;
        public AnimFunMonsterData(String animName, Int32 animWeight)
        {
            this.animName = animName;
            this.animWeight = animWeight;
        }

    }
}
