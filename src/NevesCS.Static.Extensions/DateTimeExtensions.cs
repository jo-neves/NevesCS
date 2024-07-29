using NevesCS.Abstractions.Options;
using NevesCS.Static.Constants.Values;
using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime SetTime(
            this DateTime sourceDateTime,
            int hours,
            int minutes,
            int seconds,
            int milliseconds,
            int microseconds)
        {
            return DateTimeUtils.SetTime(sourceDateTime, hours, minutes, seconds, milliseconds, microseconds);
        }

        public static DateTimeOffset SetTime(
            this DateTimeOffset sourceDateTime,
            int hours,
            int minutes,
            int seconds,
            int milliseconds,
            int microseconds)
        {
            return DateTimeUtils.SetTime(sourceDateTime, hours, minutes, seconds, milliseconds, microseconds);
        }

        public static DateTime SetTicks(this DateTime sourceDateTime, long ticks)
        {
            return DateTimeUtils.SetTicks(sourceDateTime, ticks);
        }

        public static DateTimeOffset SetTicks(this DateTimeOffset sourceDateTime, long ticks)
        {
            return DateTimeUtils.SetTicks(sourceDateTime, ticks);
        }

        public static DateTime ToNextDayOfWeek(this DateTime sourceDateTime, DayOfWeek targetDayOfWeek)
        {
            return DateTimeUtils.ToNextDayOfWeek(sourceDateTime, targetDayOfWeek);
        }

        public static DateTimeOffset ToNextDayOfWeek(this DateTimeOffset sourceDateTime, DayOfWeek targetDayOfWeek)
        {
            return DateTimeUtils.ToNextDayOfWeek(sourceDateTime, targetDayOfWeek);
        }

        public static DateTime ToStartOfDay(this DateTime date)
        {
            return DateTimeUtils.ToStartOfDay(date);
        }

        public static DateTimeOffset ToStartOfDay(this DateTimeOffset date)
        {
            return DateTimeUtils.ToStartOfDay(date);
        }

        public static DateTime ToEndOfDay(this DateTime date)
        {
            return DateTimeUtils.ToEndOfDay(date);
        }

        public static DateTimeOffset ToEndOfDay(this DateTimeOffset date)
        {
            return DateTimeUtils.ToEndOfDay(date);
        }

        public static DateTime ToStartOfWeek(this DateTime date)
        {
            return DateTimeUtils.ToStartOfWeek(date);
        }

        public static DateTimeOffset ToStartOfWeek(this DateTimeOffset date)
        {
            return DateTimeUtils.ToStartOfWeek(date);
        }

        public static DateTimeOffset ToNext(
            this DateTimeOffset source,
            TimeComponent timeComponent,
            double componentQuantity = Ints.One)
        {
            return DateTimeUtils.ToNext(source, timeComponent, componentQuantity);
        }

        public static DateTime ToNext(
            this DateTime source,
            TimeComponent timeComponent,
            double componentQuantity = Ints.One)
        {
            return DateTimeUtils.ToNext(source, timeComponent, componentQuantity);
        }
    }
}
