using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class Rectangle : ID2OInternalClass
    {
        public Int32 x;
        public Int32 y;
        public Int32 width;
        public Int32 height;
        public Rectangle(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

      
    }
}
