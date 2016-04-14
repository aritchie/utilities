//using System;


//namespace Acr.Utilities
//{
//    public static class DateTimeExtensions
//    {

//        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
//        public static long ToUnixTimestamp(this DateTime dateTime)
//        {
//            var utc = TimeZoneInfo.ConvertTimeToUtc(dateTime);
//            return Convert.ToInt64((utc - Epoch).TotalSeconds);
//        }


//        public static DateTime FromUnixTimestamp(this long unixTimestamp)
//        {
//            return Epoch
//                .AddSeconds(unixTimestamp)
//                .ToLocalTime();
//        }
//    }
//}
