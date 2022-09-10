using System;

namespace WebTools.Context
{
    public static class DateTimeExtensions
    {
        public static string ToYMD(this DateTime theDate)
        {
            return theDate.ToString("yyyyMMdd");
        }

        public static string ToYMD(this DateTime? theDate)
        {
            return theDate.HasValue ? theDate.Value.ToYMD() : string.Empty;
        }
    }
}
