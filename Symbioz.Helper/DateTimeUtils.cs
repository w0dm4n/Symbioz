using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shader.Helper
{
    public static class DateTimeUtils
    {
        public static int GetEpochFromDateTime(DateTime input)
        {
            TimeSpan timeSpan = input - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)timeSpan.TotalSeconds;
            return secondsSinceEpoch;
        }
    }
}
