using NevesCS.Static.Constants.DateAndTime;

namespace NevesCS.Static.Utils
{
    public static class DateTimeFormatUtils
    {
        public static string ToIso8601FormatString(DateTimeOffset date)
        {
            return date.ToString(DateStringFormat.ISO_8601);
        }

        public static string ToDetailedFormatString(DateTimeOffset date)
        {
            return date.ToString(DateStringFormat.DETAILED_DATE_TIME);
        }

        public static string ToSqlQueryFormatString(DateTimeOffset date)
        {
            return date.ToString(DateStringFormat.SQL_QUERY);
        }
    }
}
