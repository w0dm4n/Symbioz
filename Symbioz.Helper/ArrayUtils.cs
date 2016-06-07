using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shader.Helper
{
    public static class ArrayUtils
    {
        public static bool InArray(int[] array, int type)
        {
            foreach (var value in array)
                if (value == type)
                    return (true);
            return (false);
        }
    }
}
