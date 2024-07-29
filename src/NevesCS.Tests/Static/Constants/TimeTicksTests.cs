using FluentAssertions;

using NevesCS.Static.Constants.DateTime;

namespace NevesCS.Tests.Static.Constants
{
    public class TimeTicksTests
    {
        [Fact]
        public void Constants_AreCorrect()
        {
#pragma warning disable S6562 // Always set the "DateTimeKind" when creating new "DateTime" instances

            new DateTime(2024, 02, 26).AddTicks(TimeTicks.OneMillisecond).Should().Be(new DateTime(2024, 02, 26, 00, 00, 00, 01));
            new DateTime(2024, 02, 26).AddTicks(TimeTicks.OneSecond).Should().Be(new DateTime(2024, 02, 26, 00, 00, 01));
            new DateTime(2024, 02, 26).AddTicks(TimeTicks.OneMinute).Should().Be(new DateTime(2024, 02, 26, 00, 01, 00));
            new DateTime(2024, 02, 26).AddTicks(TimeTicks.OneHour).Should().Be(new DateTime(2024, 02, 26, 01, 00, 00));

#pragma warning restore S6562 // Always set the "DateTimeKind" when creating new "DateTime" instances
        }
    }
}
