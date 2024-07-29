using NevesCS.Static.Utils;

using System.Collections.Immutable;

<<<<<<<< HEAD:src/NevesCS.Static/Constants/DateAndTime/DaysOfWeek.cs
namespace NevesCS.Static.Constants.DateAndTime
========
namespace NevesCS.Static.Constants.DateTime
>>>>>>>> edc3f9c (!reorg: small file reorg):src/NevesCS.Static/Constants/DateTime/DaysOfWeek.cs
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
