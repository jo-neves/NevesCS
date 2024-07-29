using FluentAssertions;

using NevesCS.Static.Constants.DateAndTime;
using NevesCS.Static.Extensions;

namespace NevesCS.Tests.Static
{
    public class DateTimeOffsetTimeZoneUtilsTests
    {
        private const int Summer1hDst = 1;

        private static readonly TimeSpan DbTimeSpan = TimeSpan.Zero;

        public static IEnumerable<object[]> Dst1hTimeZonesMemberData =>
            [
                [TimeZones.London],
                [TimeZones.Berlin],
            ];

        public static IEnumerable<object[]> NonDstTimeZonesMemberData =>
            [
                new object[] { TimeZones.Istanbul },
            ];

        [Theory]
        [InlineData(DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Friday)]
        [InlineData(DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Sunday)]
        public void ToNextDayOfWeek_AddsCorrectNumberOfDays(DayOfWeek dayOfWeek)
        {
            var currentDate = DateTimeOffset.UtcNow.ToStartOfDay(TimeZoneInfo.Utc).ToStartOfWeek(TimeZoneInfo.Utc);
            currentDate.DayOfWeek.Should().Be(DayOfWeek.Monday);

            currentDate.ToNextDayOfWeek(dayOfWeek, TimeZoneInfo.Utc).DayOfWeek.Should().Be(dayOfWeek);
        }

        [Fact]
        public void AddYears_Passes_ForLondonTz()
        {
            var localTimeZone = TimeZones.London;

            // Summer.
            new DateTimeOffset(2024, 06, 29, 23, 00, 00, DbTimeSpan)
                .AddYears(1, localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2025, 06, 29, 23, 00, 00, DbTimeSpan));

            // Winter.
            new DateTimeOffset(2024, 01, 12, 00, 00, 00, DbTimeSpan)
                .AddYears(1, localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2025, 01, 12, 00, 00, 00, DbTimeSpan));
        }

        [Theory]
        [MemberData(nameof(Dst1hTimeZonesMemberData))]
        public void AddMonths_Passes_ForDstTimeZones(TimeZoneInfo localTimeZone)
        {
            // Summer.
            new DateTimeOffset(2024, 06, 29, 23, 00, 00, DbTimeSpan)
                .AddMonths(1, localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 07, 29, 23, 00, 00, DbTimeSpan));

            // Winter.
            new DateTimeOffset(2024, 01, 12, 00, 00, 00, DbTimeSpan)
                .AddMonths(1, localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 02, 12, 00, 00, 00, DbTimeSpan));

            // Winter to Summer.
            new DateTimeOffset(2024, 03, 12, 00, 00, 00, DbTimeSpan)
                .AddMonths(2, localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 05, 11, 23, 00, 00, DbTimeSpan));

            // This is to show the wrong date.
            new DateTimeOffset(2024, 03, 12, 00, 00, 00, DbTimeSpan)
                .AddMonths(3)
                .Should()
                .NotBe(new DateTimeOffset(2024, 05, 12, 00, 00, 00, DbTimeSpan));
        }

        [Theory]
        [MemberData(nameof(NonDstTimeZonesMemberData))]
        public void AddMonths_Passes_ForNonDstTimeZones(TimeZoneInfo localTimeZone)
        {
            // Summer.
            new DateTimeOffset(2024, 06, 30, 00, 00, 00, DbTimeSpan)
                .AddMonths(1, localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 07, 30, 00, 00, 00, DbTimeSpan));

            // Winter.
            new DateTimeOffset(2024, 01, 12, 00, 00, 00, DbTimeSpan)
                .AddMonths(1, localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 02, 12, 00, 00, 00, DbTimeSpan));

            var timeZoneOffset = localTimeZone.BaseUtcOffset.Hours;
            var midnight = 24 - timeZoneOffset;

            // Winter to Summer.
            new DateTimeOffset(2024, 03, 11, midnight, 00, 00, DbTimeSpan)
                .AddMonths(2, localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 05, 11, midnight, 00, 00, DbTimeSpan));
        }

        [Theory]
        [MemberData(nameof(Dst1hTimeZonesMemberData))]
        public void ToStartOfMonth_Passes_ForDstTimezones(TimeZoneInfo localTimeZone)
        {
            // For London the base timezone offset is 0, but for Germany it's 1h.
            var winterTimeZoneOffset = localTimeZone.BaseUtcOffset.Hours;
            var summerMidnight = 24 - winterTimeZoneOffset - Summer1hDst;

            // Summer.
            new DateTimeOffset(2024, 06, 29, summerMidnight, 00, 00, DbTimeSpan)
                .ToStartOfMonth(localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 05, 31, summerMidnight, 00, 00, DbTimeSpan));

            // Summer.
            new DateTimeOffset(2024, 06, 12, summerMidnight, 00, 00, DbTimeSpan)
                .ToStartOfMonth(localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 05, 31, summerMidnight, 00, 00, DbTimeSpan));

            if (winterTimeZoneOffset != 0)
            {
                // Let's only test London's winter.
                return;
            }

            // Winter.
            new DateTimeOffset(2024, 01, 29, 12, 15, 56, DbTimeSpan)
                .ToStartOfMonth(localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 01, 01, 00, 00, 00, DbTimeSpan));

            // Winter.
            new DateTimeOffset(2024, 01, 12, 00, 00, 00, DbTimeSpan)
                .ToStartOfMonth(localTimeZone)
                .Should()
                .Be(new DateTimeOffset(2024, 01, 01, 00, 00, 00, DbTimeSpan));
        }
    }
}
