using NevesCS.Static.Constants.Values;

namespace NevesCS.Static.Utils
{
    public static class DateTimeOffsetTimeZoneUtils
    {
        public static DateTimeOffset ToUtcOffset(DateTime localDateTime, TimeZoneInfo sourceTimeZone)
        {
            return new DateTimeOffset(
                TimeZoneInfo.ConvertTimeToUtc(localDateTime, sourceTimeZone),
                TimeSpan.Zero);
        }

        public static DateTime ToLocalDateTime(DateTimeOffset localDateTime, TimeZoneInfo sourceTimeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(localDateTime.UtcDateTime, sourceTimeZone);
        }

        public static bool IsUtc(this DateTimeOffset dateTime)
        {
            return dateTime.Offset == TimeSpan.Zero;
        }

        /// <summary>
        /// Checks if the given DateTimeOffset is the last day of the month in the specified time zone.
        ///
        /// </summary>
        public static bool IsLastDayOfTheMonth(DateTimeOffset utcDate, TimeZoneInfo timeZone)
        {
            var localDate = ToLocalDateTime(utcDate, timeZone);
            return localDate.Day == DateTime.DaysInMonth(localDate.Year, localDate.Month);
        }

        public static bool IsInBetween(
            DateTimeOffset utcTargetDate,
            IFiniteDateRange utcFiniteDateRange,
            TimeZoneInfo timeZone,
            bool inclusive)
        {
            return IsInBetween(utcTargetDate, utcFiniteDateRange.Start, utcFiniteDateRange.End, timeZone, inclusive);
        }

        public static bool IsInBetween(
            DateTimeOffset utcTargetDate,
            DateTimeOffset utcStartDate,
            DateTimeOffset utcEndDate,
            TimeZoneInfo timeZone,
            bool inclusive)
        {
            var localTargetDate = ToLocalDateTime(utcTargetDate, timeZone);
            var localStartDate = ToLocalDateTime(utcStartDate, timeZone);
            var localEndDate = ToLocalDateTime(utcEndDate, timeZone);

            return DateTimeUtils.IsInBetween(localTargetDate, localStartDate, localEndDate, timeZone, inclusive);
        }

        /// <summary>
        /// Counts the number of days in the specified time zone.
        ///
        /// </summary>
        public static int DaysInMonth(DateTimeOffset utcDate, TimeZoneInfo timeZone)
        {
            var localDate = ToLocalDateTime(utcDate, timeZone);
            return DateTime.DaysInMonth(localDate.Year, localDate.Month);
        }

        /// <summary>
        /// Returns the UTC date for date plus the specified days in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset AddDays(DateTimeOffset date, double days, TimeZoneInfo timeZone)
        {
            var localDate = ToLocalDateTime(date, timeZone).AddDays(days);

            return ToUtcOffset(localDate, timeZone);
        }

        /// <summary>
        /// Returns the UTC date for date plus the specified weeks in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset AddWeeks(DateTimeOffset utcDate, double weeks, TimeZoneInfo timeZone)
        {
            return AddDays(utcDate, weeks * 7, timeZone);
        }

        /// <summary>
        /// Returns the UTC date for date plus the specified months in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset AddMonths(DateTimeOffset date, int months, TimeZoneInfo timeZone)
        {
            var localDate = ToLocalDateTime(date, timeZone).AddMonths(months);

            return ToUtcOffset(localDate, timeZone);
        }

        /// <summary>
        /// Returns the UTC date for date plus the specified years in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset AddYears(DateTimeOffset date, int years, TimeZoneInfo timeZone)
        {
            var localDate = ToLocalDateTime(date, timeZone).AddYears(years);

            return ToUtcOffset(localDate, timeZone);
        }

        /// <summary>
        /// Returns the UTC date corresponding to the start of day for the UTC date in accordance with local timezone. <br/>
        /// This is a DST aware method.
        ///
        /// </summary>
        public static DateTimeOffset ToStartOfDay(DateTimeOffset dateTime, TimeZoneInfo timeZone)
        {
            var localDate = ToLocalDateTime(dateTime, timeZone);

            return ToUtcOffset(
                localDate
                    .AddHours(-localDate.Hour)
                    .AddMinutes(-localDate.Minute)
                    .AddSeconds(-localDate.Second)
                    .AddMilliseconds(-localDate.Millisecond),
                timeZone);
        }

        public static DateTimeOffset ToStartOfWeek(DateTimeOffset date, TimeZoneInfo timeZone)
        {
            var newDate = ToStartOfDay(ToLocalDateTime(date, timeZone) , timeZone);

            var daysToAdd = 0;

            switch (newDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    daysToAdd = -Ints.One;
                    break;
                case DayOfWeek.Wednesday:
                    daysToAdd = -Ints.Two;
                    break;
                case DayOfWeek.Thursday:
                    daysToAdd = -Ints.Three;
                    break;
                case DayOfWeek.Friday:
                    daysToAdd = -Ints.Four;
                    break;
                case DayOfWeek.Saturday:
                    daysToAdd = -Ints.Five;
                    break;
                case DayOfWeek.Sunday:
                    daysToAdd = -Ints.Six;
                    break;
            }

            newDate = AddDays(newDate, daysToAdd, timeZone);

            return newDate.ToUniversalTime();
        }

        public static DateTimeOffset ToNextDayOfWeek(DateTimeOffset dateTime, DayOfWeek targetDayOfWeek, TimeZoneInfo timeZone)
        {
            var localDate = ToLocalDateTime(dateTime, timeZone);
            var currentDayOfWeek = localDate.DayOfWeek;

            localDate = AddDays(localDate, targetDayOfWeek - currentDayOfWeek, timeZone).DateTime;

            return ToUtcOffset(localDate, timeZone);
        }

        public static DateTimeOffset ToStartOfMonth(DateTimeOffset date, TimeZoneInfo timeZone)
        {
            return AddDays(ToStartOfDay(date, timeZone), -(ToLocalDateTime(date, timeZone).Day - 1), timeZone);
        }

        /// <summary>
        /// Converts the given date to the last day of the month in the specified time zone.
        ///
        /// </summary>
        public static DateTime ToLastDayOfTheMonth(this DateTime date, TimeZoneInfo timeZone)
        {
            var localDate = date;

            if (date.Kind == DateTimeKind.Utc)
            {
                localDate = ToLocalDateTime(date, timeZone);

                return new DateTime(
                    localDate.Year,
                    localDate.Month,
                    DateTime.DaysInMonth(localDate.Year, localDate.Month),
                    localDate.Hour,
                    localDate.Minute,
                    localDate.Second,
                    date.Kind)
                    .ToUniversalTime();
            }

            return new DateTime(
                localDate.Year,
                localDate.Month,
                DateTime.DaysInMonth(localDate.Year, localDate.Month),
                localDate.Hour,
                localDate.Minute,
                localDate.Second,
                date.Kind);
        }
    }
}
