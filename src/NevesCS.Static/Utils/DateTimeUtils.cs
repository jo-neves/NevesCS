using NevesCS.Abstractions.Options;
using NevesCS.Abstractions.Types;
using NevesCS.Static.Constants.Values;

namespace NevesCS.Static.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime From(DateTime sourceDate)
        {
            return new DateTime(
                sourceDate.Year,
                sourceDate.Month,
                sourceDate.Day,
                sourceDate.Hour,
                sourceDate.Minute,
                sourceDate.Second,
                sourceDate.Millisecond,
                sourceDate.Microsecond,
                sourceDate.Kind);
        }

        public static DateTimeOffset From(DateTime sourceDate, TimeSpan offset)
        {
            return new DateTimeOffset(
                sourceDate.Year,
                sourceDate.Month,
                sourceDate.Day,
                sourceDate.Hour,
                sourceDate.Minute,
                sourceDate.Second,
                sourceDate.Millisecond,
                sourceDate.Microsecond,
                offset);
        }

        public static DateTimeOffset From(DateTimeOffset sourceDate)
        {
            return new DateTimeOffset(
                sourceDate.Year,
                sourceDate.Month,
                sourceDate.Day,
                sourceDate.Hour,
                sourceDate.Minute,
                sourceDate.Second,
                sourceDate.Millisecond,
                sourceDate.Microsecond,
                sourceDate.Offset);
        }

        public static TimeSpan GetMachineOffset()
        {
            return DateTimeOffset.Now.Offset;
        }

        public static DateOnly GetDateOnly(DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }

        public static DateTimeOffset SetTime(
            DateTimeOffset sourceDateTime,
            int hours,
            int minutes,
            int seconds,
            int milliseconds,
            int microseconds)
        {
            return new DateTimeOffset(
                sourceDateTime.Year,
                sourceDateTime.Month,
                sourceDateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                microseconds,
                sourceDateTime.Offset);
        }

        public static DateTimeOffset SetTicks(DateTimeOffset sourceDateTime, long ticks)
        {
            return ToStartOfDay(sourceDateTime).AddTicks(ticks);
        }

        public static DateTimeOffset ToNextDayOfWeek(DateTimeOffset sourceDateTime, DayOfWeek targetDayOfWeek)
        {
            return From(sourceDateTime).AddDays(targetDayOfWeek - sourceDateTime.DayOfWeek);
        }

        public static DateTimeOffset ToStartOfDay(DateTimeOffset date)
        {
            return From(date.Date, date.Offset);
        }

        public static DateTimeOffset ToEndOfDay(DateTimeOffset date)
        {
            return ToStartOfDay(date).AddDays(1).AddMilliseconds(-1);
        }

        public static DateTimeOffset ToStartOfWeek(DateTimeOffset date)
        {
            var newDateStartOfDay = ToStartOfDay(date);

            switch (newDateStartOfDay.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-2);
                    break;
                case DayOfWeek.Thursday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-3);
                    break;
                case DayOfWeek.Friday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-4);
                    break;
                case DayOfWeek.Saturday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-5);
                    break;
                case DayOfWeek.Sunday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-6);
                    break;
            }

            return newDateStartOfDay;
        }

        public static DateTimeOffset ToNext(
            DateTimeOffset source,
            TimeComponent timeComponent,
            double componentQuantity = Ints.One)
        {
            var newDate = From(source);

            return timeComponent switch
            {
                TimeComponent.Second => newDate
                    .AddMilliseconds(-newDate.Millisecond)
                    .AddMicroseconds(-newDate.Microsecond)
                    .AddSeconds(componentQuantity),

                TimeComponent.Minute => newDate
                    .AddSeconds(-newDate.Second)
                    .AddMilliseconds(-newDate.Millisecond)
                    .AddMicroseconds(-newDate.Microsecond)
                    .AddMinutes(componentQuantity),

                TimeComponent.Hour => newDate
                    .AddMinutes(-newDate.Minute)
                    .AddSeconds(-newDate.Second)
                    .AddMilliseconds(-newDate.Millisecond)
                    .AddMicroseconds(-newDate.Microsecond)
                    .AddHours(componentQuantity),

                TimeComponent.Day => newDate
                    .AddHours(-newDate.Hour)
                    .AddMinutes(-newDate.Minute)
                    .AddSeconds(-newDate.Second)
                    .AddMilliseconds(-newDate.Millisecond)
                    .AddMicroseconds(-newDate.Microsecond)
                    .AddDays(componentQuantity),

                _ => throw new NotImplementedException(),
            };
        }

        public static bool IsInBetween(
            DateTimeOffset targetDate,
            IFiniteDateRange finiteDateRange,
            bool inclusive)
        {
            return IsInBetween(targetDate, finiteDateRange.Start, finiteDateRange.End, inclusive);
        }

        public static bool IsInBetween(
            DateTimeOffset targetDate,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            bool inclusive)
        {
            return inclusive
                ? startDate <= targetDate && targetDate <= endDate
                : startDate < targetDate && targetDate < endDate;
        }
    }
}
