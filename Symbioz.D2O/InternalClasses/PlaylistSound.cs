// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class PlaylistSound : ID2OInternalClass
    {
        public String id;
        public Int32 volume;
        public PlaylistSound(String id, Int32 volume)
        {
            this.id = id;
            this.volume = volume;
        }

        
    }
}
