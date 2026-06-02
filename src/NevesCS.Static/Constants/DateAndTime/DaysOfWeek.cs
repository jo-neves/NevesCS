using NevesCS.Static.Utils;

using System.Collections.Immutable;

namespace NevesCS.Static.Constants.DateAndTime
{
    public static class DaysOfWeek
    {
        public static readonly ImmutableArray<DayOfWeek> AllDaysOfWeek =
            DayOfWeekUtils.GetAllDaysOfWeek()
                .Select(x => (DayOfWeek)x[0])
                .ToImmutableArray();

        public static readonly ImmutableArray<DayOfWeek> WeekendDaysOfWeek =
            [
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            ];
    }
}
