using NevesCS.Abstractions.Types;
using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class DateTimeOffsetTimeZoneExtensions
    {
        public static DateTimeOffset ToUtcOffset(this DateTime localDateTime, TimeZoneInfo sourceTimeZone)
        {
            return DateTimeOffsetTimeZoneUtils.ToUtcOffset(localDateTime, sourceTimeZone);
        }

        public static DateTime ToLocalDateTime(this DateTimeOffset localDateTime, TimeZoneInfo sourceTimeZone)
        {
            return DateTimeOffsetTimeZoneUtils.ToLocalDateTime(localDateTime, sourceTimeZone);
        }

        public static bool IsUtc(this DateTimeOffset dateTime)
        {
            return DateTimeOffsetTimeZoneUtils.IsUtc(dateTime);
        }

        public static DayOfWeek GetDayOfWeek(this DateTimeOffset utcDate, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.GetDayOfWeek(utcDate, timeZone);
        }

        /// <summary>
        /// Checks if the given DateTimeOffset is the last day of the month in the specified time zone.
        ///
        /// </summary>
        public static bool IsLastDayOfTheMonth(this DateTimeOffset utcDate, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.IsLastDayOfTheMonth(utcDate, timeZone);
        }

        public static bool IsInBetween(
            this DateTimeOffset utcTargetDate,
            IFiniteDateRange utcFiniteDateRange,
            TimeZoneInfo timeZone,
            bool inclusive)
        {
            return IsInBetween(utcTargetDate, utcFiniteDateRange.Start, utcFiniteDateRange.End, timeZone, inclusive);
        }

        public static bool IsInBetween(
            this DateTimeOffset utcTargetDate,
            DateTimeOffset utcStartDate,
            DateTimeOffset utcEndDate,
            TimeZoneInfo timeZone,
            bool inclusive)
        {
            return IsInBetween(utcTargetDate, utcStartDate, utcEndDate, timeZone, inclusive);
        }

        /// <summary>
        /// Counts the number of days in the specified time zone.
        ///
        /// </summary>
        public static int DaysInMonth(DateTimeOffset utcDate, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.DaysInMonth(utcDate, timeZone);
        }

        /// <summary>
        /// Returns the UTC date for date plus the specified days in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset AddDays(this DateTimeOffset date, double days, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.AddDays(date, days, timeZone);
        }

        /// <summary>
        /// Returns the UTC date for date plus the specified weeks in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset AddWeeks(this DateTimeOffset utcDate, double weeks, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.AddWeeks(utcDate, weeks, timeZone);
        }

        /// <summary>
        /// Returns the UTC date for date plus the specified months in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset AddMonths(this DateTimeOffset date, int months, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.AddMonths(date, months, timeZone);
        }

        /// <summary>
        /// Returns the UTC date for date plus the specified years in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset AddYears(this DateTimeOffset date, int years, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.AddYears(date, years, timeZone);
        }

        /// <summary>
        /// Returns the UTC date corresponding to the start of day for the UTC date in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset ToStartOfDay(this DateTimeOffset date, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.ToStartOfDay(date, timeZone);
        }

        /// <summary>
        /// Returns the UTC date corresponding to the end of day for the UTC date in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset ToEndOfDay(this DateTimeOffset date, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.ToEndOfDay(date, timeZone);
        }

        public static DateTimeOffset ToStartOfWeek(this DateTimeOffset date, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.ToStartOfWeek(date, timeZone);
        }

        public static DateTimeOffset ToNextDayOfWeek(this DateTimeOffset dateTime, DayOfWeek targetDayOfWeek, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.ToNextDayOfWeek(dateTime, targetDayOfWeek, timeZone);
        }

        public static DateTimeOffset ToStartOfMonth(this DateTimeOffset date, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.ToStartOfMonth(date, timeZone);
        }

        /// <summary>
        /// Converts the given date to the last day of the month in the specified time zone.
        ///
        /// </summary>
        public static DateTime ToLastDayOfTheMonth(this DateTime date, TimeZoneInfo timeZone)
        {
            return DateTimeOffsetTimeZoneUtils.ToLastDayOfTheMonth(date, timeZone);
        }
    }
}
