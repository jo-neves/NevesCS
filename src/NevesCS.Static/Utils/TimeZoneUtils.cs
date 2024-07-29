using NevesCS.Static.Constants.DateTime;

using System.Runtime.InteropServices;

namespace NevesCS.Static.Utils
{
    public static class TimeZoneUtils
    {
        public static TimeZoneInfo GetTimeZone(SystemTimeZoneIdsRecord systemTimeZoneSystemIds)
        {
            return GetTimeZone(systemTimeZoneSystemIds.Unix, systemTimeZoneSystemIds.Windows);
        }

        public static TimeZoneInfo GetTimeZone(string unixTimezoneName, string windowsTimezoneName)
        {
            return GetTimeZone(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsTimezoneName : unixTimezoneName);
        }

        public static TimeZoneInfo GetTimeZone(string timezoneId)
        {
            TimeZoneInfo tz;

            try
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                throw new TimeZoneNotFoundException(
                    "Could not find timezone for " +
                    $"{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? nameof(OSPlatform.Windows) : "Unix")} " +
                    $"system. `{timezoneId}`");
            }

            return tz;
        }

        public static DateTimeOffset ConvertToLondonTimeZone(DateTimeOffset source)
        {
            return ConvertToTimeZone(source, TimeZones.London);
        }

        public static DateTimeOffset ConvertToTimeZone(DateTimeOffset source, TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTime(source, timeZoneInfo);
        }
    }
}
