using NevesCS.Static.Utils;

using FluentAssertions;

namespace NevesCS.Tests.Static
{
    public class CurrencyConversionUtilsTests
    {
        [Theory]
        [InlineData(3567.47, 356747)]
        [InlineData(12.30, 1230)]
        [InlineData(0.05, 5)]
        public void DecimalToCents_Passes(decimal input, long expected)
        {
            CurrencyConversionUtils.DecimalToCents(input).Should().Be(expected);
        }

        [Theory]
        [InlineData(356747, 3567.47)]
        [InlineData(1230, 12.30)]
        [InlineData(5, 0.05)]
        public void CentsToDecimal_Passes(long input, decimal expected)
        {
            CurrencyConversionUtils.CentsToDecimal(input).Should().Be(expected);
        }
    }
}
