using NevesCS.Static.Utils;

using System.Diagnostics.CodeAnalysis;

namespace NevesCS.Static.Constants.DateAndTime
{
    [ExcludeFromCodeCoverage]
    public static class TimeZones
    {
        public static readonly TimeZoneInfo London = TimeZoneUtils.GetTimeZone(TimeZoneIds.LONDON);

        public static readonly TimeZoneInfo Berlin = TimeZoneUtils.GetTimeZone(TimeZoneIds.BERLIN);

        public static readonly TimeZoneInfo Istanbul = TimeZoneUtils.GetTimeZone(TimeZoneIds.ISTANBUL);

        public static readonly TimeZoneInfo AustraliaLordHowe = TimeZoneUtils.GetTimeZone(TimeZoneIds.AUSTRALIA_LORD_HOWE);
    }
}
