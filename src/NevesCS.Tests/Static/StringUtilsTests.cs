using FluentAssertions;

using NevesCS.Static.Extensions;
using NevesCS.Static.Utils;

namespace NevesCS.Tests.Static
{
    public class StringUtilsTests
    {
        [Theory]
        [InlineData(null, "ABC", false)]
        [InlineData(null, null, false)]
        [InlineData("", "ABC", false)]
        [InlineData("ABC", "", false)]
        [InlineData("abc", "ADc", false)]
        [InlineData("adc", "ABc", false)]
        [InlineData("ABC", "abc", true)]
        [InlineData("abc", "ABC", true)]
        [InlineData("abc", "ABc", true)]
        public void EqualsIgnoreCase_Should_Pass(string? source, string? target, bool expected)
        {
            source.EqualsIgnoreCase(target!).Should().Be(expected);
            StringUtils.EqualsIgnoreCase(source, target!).Should().Be(expected);
        }

        [Theory]
        [InlineData("ABC", false)]
        [InlineData(null, true)]
        [InlineData("", true)]
        public void ThrowIfNullOrEmpty_Should_Pass(string? target, bool shouldThrow)
        {
            var act1 = () => target.ThrowIfNullOrEmpty(nameof(target));
            var act2 = () => target.AssertIfNullOrEmpty(nameof(target));

            if (shouldThrow)
            {
                act1.Should().Throw();
                act2.Should().Throw();
            }
            else
            {
                act1.Should().NotThrow();
                act2.Should().NotThrow();
            }
        }
    }
}
