// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class TransformData : ID2OInternalClass
    {
        public Double x;
        public Double y;
        public Double scaleX;
        public Double scaleY;
        public Int32 rotation;
        public String originalClip;
        public String overrideClip;
        public TransformData(Double x, Double y, Double scaleX, Double scaleY, Int32 rotation, String originalClip, String overrideClip)
        {
            this.x = x;
            this.y = y;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.rotation = rotation;
            this.originalClip = originalClip;
            this.overrideClip = overrideClip;
        }

       
    }
}
