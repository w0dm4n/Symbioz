﻿using System;
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

        public static int ToEpochTime(this DateTime input)
        {
            return DateTimeUtils.GetEpochFromDateTime(input);
        }

        public static DateTime GetDateTimeFromEpoch(this int input)
        {
            DateTime dateTimeOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeOrigin.AddSeconds(input);
        }
    }
}
