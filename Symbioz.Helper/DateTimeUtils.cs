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
            return (int)timeSpan.TotalSeconds;
        }

        public static double GetEpochInMillisFromDateTime(DateTime input)
        {
            TimeSpan timeSpan = input - new DateTime(1970, 1, 1);
            return timeSpan.TotalMilliseconds;
        }

        public static int ToEpochTime(this DateTime input)
        {
            return DateTimeUtils.GetEpochFromDateTime(input);
        }

        public static double ToEpochTimeInMillis(this DateTime input)
        {
            return DateTimeUtils.GetEpochInMillisFromDateTime(input);
        }

        public static DateTime GetDateTimeFromEpoch(this int input)
        {
            DateTime dateTimeOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeOrigin.AddSeconds(input);
        }
    }
}
