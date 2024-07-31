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
            return From(date)
                .AddHours(-date.Hour)
                .AddMinutes(-date.Minute)
                .AddSeconds(-date.Second)
                .AddMilliseconds(-date.Millisecond)
                .AddMicroseconds(-date.Microsecond);
        }

        public static DateTimeOffset ToEndOfDay(DateTimeOffset date)
        {
            return ToStartOfDay(date).AddDays(Ints.One).AddTicks(-Ints.One);
        }

        public static DateTimeOffset ToStartOfWeek(DateTimeOffset date)
        {
            var newDateStartOfDay = ToStartOfDay(date);

            switch (newDateStartOfDay.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-Ints.One);
                    break;
                case DayOfWeek.Wednesday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-Ints.Two);
                    break;
                case DayOfWeek.Thursday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-Ints.Three);
                    break;
                case DayOfWeek.Friday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-Ints.Four);
                    break;
                case DayOfWeek.Saturday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-Ints.Five);
                    break;
                case DayOfWeek.Sunday:
                    newDateStartOfDay = newDateStartOfDay.AddDays(-Ints.Six);
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
