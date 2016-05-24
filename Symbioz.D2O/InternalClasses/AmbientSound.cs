// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class AmbientSound : ID2OInternalClass
    {
        public Int32 id;
        public Int32 volume;
        public Int32 criterionId;
        public Int32 silenceMin;
        public Int32 silenceMax;
        public Int32 channel;
        public Int32 type_id;
        public AmbientSound(Int32 id, Int32 volume, Int32 criterionId, Int32 silenceMin, Int32 silenceMax, Int32 channel, Int32 type_id)
        {
            this.id = id;
            this.volume = volume;
            this.criterionId = criterionId;
            this.silenceMin = silenceMin;
            this.silenceMax = silenceMax;
            this.channel = channel;
            this.type_id = type_id;
        }

       
    }
}
