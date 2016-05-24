using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class Point : ID2OInternalClass
    {
        public int x;
        public int y;
        public Point(int x,int y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
