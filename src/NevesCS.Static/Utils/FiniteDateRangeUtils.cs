using NevesCS.Abstractions.Types;
using NevesCS.NonStatic.Models.ValueTypes;
using NevesCS.Static.Constants.Values;

namespace NevesCS.Static.Utils
{
    public static class FiniteDateRangeUtils
    {
        public static FiniteDateRangeValue ToFiniteDateRangeValue(IFiniteDateRange range)
        {
            return new FiniteDateRangeValue(range.Start, range.End);
        }

        public static IEnumerable<FiniteDateRangeValue> SplitByDays(INonFiniteDateRange range, int daysToSplitBy)
        {
            return SplitByDays(NonFiniteDateRangeUtils.ToFiniteDateRangeValue(range), daysToSplitBy);
        }

        public static IEnumerable<FiniteDateRangeValue> SplitByDays(IFiniteDateRange range, int daysToSplitBy)
        {
            if (daysToSplitBy == Ints.One)
            {
                foreach (var dailyRange in SplitIntoDailyRanges(range))
                {
                    yield return dailyRange;
                }

                yield break;
            }

            var rangeEnd = range.End == DateTimeOffset.MaxValue ? range.End.AddDays(-daysToSplitBy) : range.End;

            var days = (rangeEnd - range.Start).TotalDays;
            var count = days / daysToSplitBy;

            for (double index = Ints.Zero; index < count; ++index)
            {
                var sliceStart = range.Start.AddDays(index * daysToSplitBy);

                if (sliceStart < rangeEnd)
                {
                    var sliceEnd = sliceStart.AddDays(daysToSplitBy);

                    yield return new FiniteDateRangeValue(
                        sliceStart,
                        sliceEnd > rangeEnd
                            ? rangeEnd
                            : sliceEnd.AddTicks(-Ints.One));
                }
                else
                {
                    yield break;
                }
            }
        }

        public static IEnumerable<FiniteDateRangeValue> SplitIntoDailyRanges(IFiniteDateRange range)
        {
            var currentDuration = TimeSpan.Zero;
            var currentStart = range.Start;

            var finalDuration = (range.End == DateTimeOffset.MaxValue ? range.End.AddDays(-Ints.One) : range.End) - range.Start;

            while (currentDuration < finalDuration)
            {
                var timeLeftInTheDay = TimeSpan.FromDays(Ints.One) - currentStart.TimeOfDay;
                currentDuration = currentDuration.Add(timeLeftInTheDay);
                var todaysEnd = currentDuration <= finalDuration
                    ? currentStart.Add(timeLeftInTheDay).AddTicks(-Ints.One)
                    : range.End;

                yield return new FiniteDateRangeValue(currentStart, todaysEnd);
                currentStart = currentStart.Add(timeLeftInTheDay);
            }
        }
    }
}
