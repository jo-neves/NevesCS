using NevesCS.Static.Utils;

using System.Diagnostics.CodeAnalysis;

<<<<<<<< HEAD:src/NevesCS.Static/Constants/DateTime/TimeZones.cs
<<<<<<<< HEAD:src/NevesCS.Static/Constants/DateAndTime/TimeZones.cs
namespace NevesCS.Static.Constants.DateAndTime
========
namespace NevesCS.Static.Constants.DateTime
>>>>>>>> edc3f9c (!reorg: small file reorg):src/NevesCS.Static/Constants/DateTime/TimeZones.cs
========
namespace NevesCS.Static.Constants.DateAndTime
>>>>>>>> 17778aa (build: fix):src/NevesCS.Static/Constants/DateAndTime/TimeZones.cs
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
