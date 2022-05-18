using System;

namespace XamarinSMS.Utils
{
    public static class TimeHelper
    {
        public static long GetTimeStamp(DateTime time)
        {
            var startTime = new DateTime(1970, 1, 1).ToLocalTime(); // 当地时区
            return (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
        }
        public static long ToTimeStamp(this DateTime time) => GetTimeStamp(time);

        public static DateTime GetDateTime(long timestamp)
        {
            var startTime = new DateTime(1970, 1, 1).ToLocalTime(); // 当地时区
            return startTime.AddMilliseconds(timestamp);
        }
        public static DateTime ToDateTime(this long timestamp) => GetDateTime(timestamp);
    }
}