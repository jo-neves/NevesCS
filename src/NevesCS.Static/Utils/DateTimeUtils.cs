using NevesCS.Abstractions.Options;
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
            return From(
                SetTime(sourceDateTime.DateTime, hours, minutes, seconds, milliseconds, microseconds),
                sourceDateTime.Offset);
        }

        public static DateTime SetTime(
            DateTime sourceDateTime,
            int hours,
            int minutes,
            int seconds,
            int milliseconds,
            int microseconds)
        {
            return new DateTime(
                sourceDateTime.Year,
                sourceDateTime.Month,
                sourceDateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                microseconds,
                sourceDateTime.Kind);
        }
        public static DateTimeOffset SetTicks(DateTimeOffset sourceDateTime, long ticks)
        {
            return From(SetTicks(sourceDateTime.DateTime, ticks), sourceDateTime.Offset);
        }

        public static DateTime SetTicks(DateTime sourceDateTime, long ticks)
        {
            return ToStartOfDay(sourceDateTime).AddTicks(ticks);
        }

        public static DateTimeOffset ToNextDayOfWeek(DateTimeOffset sourceDateTime, DayOfWeek targetDayOfWeek)
        {
            return From(ToNextDayOfWeek(sourceDateTime.DateTime, targetDayOfWeek), sourceDateTime.Offset);
        }

        public static DateTime ToNextDayOfWeek(DateTime sourceDateTime, DayOfWeek targetDayOfWeek)
        {
            return sourceDateTime.AddDays(targetDayOfWeek - sourceDateTime.DayOfWeek);
        }

        public static DateTimeOffset ToStartOfDay(DateTimeOffset date)
        {
            return From(ToStartOfDay(date.DateTime), date.Offset);
        }

        public static DateTime ToStartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 00, 00, 00, 00, 00, date.Kind);
        }

        public static DateTimeOffset ToEndOfDay(DateTimeOffset date)
        {
            return From(ToEndOfDay(date.DateTime), date.Offset);
        }

        public static DateTime ToEndOfDay(DateTime date)
        {
            return ToStartOfDay(date).AddDays(Ints.One).AddTicks(-Ints.One);
        }

        public static DateTimeOffset ToStartOfWeek(DateTimeOffset date)
        {
            return From(ToStartOfWeek(date.DateTime), date.Offset);
        }

        public static DateTime ToStartOfWeek(DateTime date)
        {
            var newDate = ToStartOfDay(From(date));

            switch (newDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    newDate = newDate.AddDays(-Ints.One);
                    break;
                case DayOfWeek.Wednesday:
                    newDate = newDate.AddDays(-Ints.Two);
                    break;
                case DayOfWeek.Thursday:
                    newDate = newDate.AddDays(-Ints.Three);
                    break;
                case DayOfWeek.Friday:
                    newDate = newDate.AddDays(-Ints.Four);
                    break;
                case DayOfWeek.Saturday:
                    newDate = newDate.AddDays(-Ints.Five);
                    break;
                case DayOfWeek.Sunday:
                    newDate = newDate.AddDays(-Ints.Six);
                    break;
            }

            return newDate;
        }

        public static DateTimeOffset ToNext(
            DateTimeOffset source,
            TimeComponent timeComponent,
            double componentQuantity = Ints.One)
        {
            return From(ToNext(source.DateTime, timeComponent, componentQuantity), source.Offset);
        }

        public static DateTime ToNext(
            DateTime source,
            TimeComponent timeComponent,
            double componentQuantity = Ints.One)
        {
            return timeComponent switch
            {
                TimeComponent.Second => source
                    .AddMilliseconds(-source.Millisecond)
                    .AddMicroseconds(-source.Microsecond)
                    .AddSeconds(componentQuantity),
                TimeComponent.Minute => source
                    .AddSeconds(-source.Second)
                    .AddMilliseconds(-source.Millisecond)
                    .AddMicroseconds(-source.Microsecond)
                    .AddMinutes(componentQuantity),
                TimeComponent.Hour => source
                    .AddMinutes(-source.Minute)
                    .AddSeconds(-source.Second)
                    .AddMilliseconds(-source.Millisecond)
                    .AddMicroseconds(-source.Microsecond)
                    .AddHours(componentQuantity),
                TimeComponent.Day => source
                    .AddHours(-source.Hour)
                    .AddMinutes(-source.Minute)
                    .AddSeconds(-source.Second)
                    .AddMilliseconds(-source.Millisecond)
                    .AddMicroseconds(-source.Microsecond)
                    .AddDays(componentQuantity),

                _ => throw new NotImplementedException(),
            };
        }
    }
}
