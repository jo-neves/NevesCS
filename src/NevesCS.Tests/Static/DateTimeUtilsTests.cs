using FluentAssertions;

using NevesCS.Abstractions.Options;
using NevesCS.Static.Constants.DateAndTime;
using NevesCS.Static.Extensions;
using NevesCS.Static.Utils;

namespace NevesCS.Tests.Static
{
    public class DateTimeUtilsTests
    {
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
            var currentDate = DateTimeOffset.UtcNow.ToStartOfDay().ToStartOfWeek();
            currentDate.DayOfWeek.Should().Be(DayOfWeek.Monday);

            currentDate.ToNextDayOfWeek(dayOfWeek).DayOfWeek.Should().Be(dayOfWeek);
            DateTimeUtils.ToNextDayOfWeek(currentDate, dayOfWeek).DayOfWeek.Should().Be(dayOfWeek);
        }

        [Fact]
        public void ToStartOfDay_ReturnsCorrect()
        {
            var date1 = DateTimeOffset.UtcNow.ToStartOfDay();
            date1.TimeOfDay.Should().Be(new TimeSpan(0, 0, 0));

            var date2 = DateTimeOffset.Now.ToStartOfDay();
            date2.TimeOfDay.Should().Be(new TimeSpan(0, 0, 0));

            var date3 = new DateTimeOffset(2024, 02, 21, 18, 18, 15, TimeSpan.Zero).ToStartOfDay();
            date3.Should().Be(new DateTimeOffset(2024, 02, 21, 00, 00, 00, TimeSpan.Zero));
            date3.TimeOfDay.Should().Be(new TimeSpan(0, 0, 0));

            var date4 = new DateTimeOffset(2024, 02, 21, 18, 18, 15, TimeSpan.FromHours(1)).ToStartOfDay();
            date4.Should().Be(new DateTimeOffset(2024, 02, 21, 00, 00, 00, TimeSpan.FromHours(1)));
            date4.TimeOfDay.Should().Be(new TimeSpan(0, 0, 0));

            var date5 = new DateTime(2024, 02, 21, 18, 18, 15, DateTimeKind.Local).ToStartOfDay();
            date5.Should().Be(new DateTime(2024, 02, 21, 00, 00, 00, DateTimeKind.Local));
            date5.TimeOfDay.Should().Be(new TimeSpan(00, 00, 00));

            var date6 = new DateTime(2024, 02, 21, 18, 18, 15, DateTimeKind.Utc).ToStartOfDay();
            date6.Should().Be(new DateTime(2024, 02, 21, 00, 00, 00, DateTimeKind.Utc));
            date6.TimeOfDay.Should().Be(new TimeSpan(00, 00, 00));
        }

        [Fact]
        public void ToStartOfWeek_ReturnsCorrect()
        {
            var date1 = new DateTimeOffset(2024, 02, 21, 18, 18, 15, TimeSpan.Zero).ToStartOfWeek();
            date1.Should().Be(new DateTimeOffset(2024, 02, 19, 00, 00, 00, TimeSpan.Zero));
            date1.TimeOfDay.Should().Be(new TimeSpan(00, 00, 00));

            var date2 = new DateTimeOffset(2024, 02, 21, 18, 18, 15, TimeSpan.FromHours(1)).ToStartOfWeek();
            date2.Should().Be(new DateTimeOffset(2024, 02, 19, 00, 00, 00, TimeSpan.FromHours(1)));
            date2.TimeOfDay.Should().Be(new TimeSpan(00, 00, 00));

            var date3 = new DateTime(2024, 02, 21, 18, 18, 15, DateTimeKind.Local).ToStartOfWeek();
            date3.Should().Be(new DateTime(2024, 02, 19, 00, 00, 00, DateTimeKind.Local));
            date3.TimeOfDay.Should().Be(new TimeSpan(00, 00, 00));

            var date4 = new DateTime(2024, 02, 21, 18, 18, 15, DateTimeKind.Utc).ToStartOfWeek();
            date4.Should().Be(new DateTime(2024, 02, 19, 00, 00, 00, DateTimeKind.Utc));
            date4.TimeOfDay.Should().Be(new TimeSpan(00, 00, 00));
        }

        [Fact]
        public void SetTicks_ReturnsCorrect()
        {
            var date1 = new DateTimeOffset(2024, 02, 21, 18, 18, 15, TimeSpan.Zero);
            date1 = date1.SetTicks(TimeTicks.OneHour);
            date1.Should().Be(new DateTimeOffset(2024, 02, 21, 01, 00, 00, TimeSpan.Zero));
        }

        [Fact]
        public void SetTime_Passes()
        {
            new DateTimeOffset(2024, 02, 21, 18, 18, 15, TimeSpan.Zero).SetTime(02, 02, 02, 02, 02)
                .Should()
                .Be(new DateTimeOffset(2024, 02, 21, 02, 02, 02, 02, 02, TimeSpan.Zero));
        }

        [Fact]
        public void ToNext_Passes()
        {
            var testDate = new DateTimeOffset(2024, 02, 21, 18, 18, 18, TimeSpan.Zero);

            testDate.ToNext(TimeComponent.Second).Should().Be(new DateTimeOffset(2024, 02, 21, 18, 18, 19, 00, 00, TimeSpan.Zero));
            testDate.ToNext(TimeComponent.Second, 3).Should().Be(new DateTimeOffset(2024, 02, 21, 18, 18, 21, 00, 00, TimeSpan.Zero));

            testDate.ToNext(TimeComponent.Minute).Should().Be(new DateTimeOffset(2024, 02, 21, 18, 19, 00, 00, 00, TimeSpan.Zero));
            testDate.ToNext(TimeComponent.Minute, 3).Should().Be(new DateTimeOffset(2024, 02, 21, 18, 21, 00, 00, 00, TimeSpan.Zero));

            testDate.ToNext(TimeComponent.Hour).Should().Be(new DateTimeOffset(2024, 02, 21, 19, 00, 00, 00, 00, TimeSpan.Zero));
            testDate.ToNext(TimeComponent.Hour, 3).Should().Be(new DateTimeOffset(2024, 02, 21, 21, 00, 00, 00, 00, TimeSpan.Zero));

            testDate.ToNext(TimeComponent.Day).Should().Be(new DateTimeOffset(2024, 02, 22, 00, 00, 00, 00, 00, TimeSpan.Zero));
            testDate.ToNext(TimeComponent.Day, 3).Should().Be(new DateTimeOffset(2024, 02, 24, 00, 00, 00, 00, 00, TimeSpan.Zero));

            new DateTimeOffset(2024, 02, 21, 18, 18, 00, 00, 00, TimeSpan.Zero)
                .ToNext(TimeComponent.Second)
                .Should()
                .Be(new DateTimeOffset(2024, 02, 21, 18, 18, 01, 00, 00, TimeSpan.Zero));
        }
    }
}
