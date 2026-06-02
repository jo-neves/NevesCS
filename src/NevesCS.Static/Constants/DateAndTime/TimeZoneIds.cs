using System.Diagnostics.CodeAnalysis;

namespace NevesCS.Static.Constants.DateAndTime
{
    /// <summary>
    /// Windows: <see href="https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones"/> <br/>
    /// Unix: <see href="https://en.wikipedia.org/wiki/List_of_tz_database_time_zones"/>
    /// Mapping: <see href="https://gist.github.com/alejzeis/ad5827eb14b5c22109ba652a1a267af5"/>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TimeZoneIds
    {
        public const string LONDON_UNIX = "Europe/London";
        public const string LONDON_WINDOWS = "GMT Standard Time";
        public static readonly SystemTimeZoneIdsRecord LONDON = new(LONDON_UNIX, LONDON_WINDOWS);

        public const string BERLIN_UNIX = "Europe/Berlin";
        public const string BERLIN_WINDOWS = "W. Europe Standard Time";
        public static readonly SystemTimeZoneIdsRecord BERLIN = new(BERLIN_UNIX, BERLIN_WINDOWS);

        public const string ISTANBUL_UNIX = "Europe/Istanbul";
        public const string ISTANBUL_WINDOWS = "Turkey Standard Time";
        public static readonly SystemTimeZoneIdsRecord ISTANBUL = new(ISTANBUL_UNIX, ISTANBUL_WINDOWS);

        public const string AUSTRALIA_LORD_HOWE_UNIX = "Australia/Lord_Howe";
        public const string AUSTRALIA_LORD_HOWE_WINDOWS = "Lord Howe Standard Time";
        public static readonly SystemTimeZoneIdsRecord AUSTRALIA_LORD_HOWE = new(AUSTRALIA_LORD_HOWE_UNIX, AUSTRALIA_LORD_HOWE_WINDOWS);
    }

    [ExcludeFromCodeCoverage]
    public readonly record struct SystemTimeZoneIdsRecord
    {
        [SetsRequiredMembers]
        public SystemTimeZoneIdsRecord(string unixTimeZoneId, string windowsTimeZoneId)
        {
            Unix = unixTimeZoneId;
            Windows = windowsTimeZoneId;
        }

        public required readonly string Unix { get; init; }

        public required readonly string Windows { get; init; }
    }
}
