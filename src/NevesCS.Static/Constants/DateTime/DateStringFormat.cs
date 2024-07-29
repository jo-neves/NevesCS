<<<<<<<< HEAD:src/NevesCS.Static/Constants/DateAndTime/DateStringFormat.cs
namespace NevesCS.Static.Constants.DateAndTime
========
namespace NevesCS.Static.Constants.DateTime
>>>>>>>> edc3f9c (!reorg: small file reorg):src/NevesCS.Static/Constants/DateTime/DateStringFormat.cs
{
    public static class DateStringFormat
    {
        public const string TIMEZONE_INFO = "K";

        public const string ISO_8601 = $"yyyy'-'MM'-'dd'T'HH':'mm':'ss{TIMEZONE_INFO}";
        public const string DETAILED_DATE_TIME = $"yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffff{TIMEZONE_INFO}";

        public const string SQL_QUERY = "yyyy-MM-dd HH:mm:ss";
    }
}
