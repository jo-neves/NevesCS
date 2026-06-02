using NevesCS.Static.Extensions;
using NevesCS.Static.Utils;

using FluentAssertions;

namespace NevesCS.Tests.Static
{
    public class CalculationUtilsTests
    {
        [Fact]
        public void SafeAverage_DoesNotThrow_IfSourceIsNullOrEmpty()
        {
            var input = Array.Empty<int>();
            var actionNoThrow = () => CalculationUtils.SafeAverage(input, x => x);
            var actionToThrow = () => input.Average();

            actionNoThrow.Should().NotThrow();
            actionToThrow.Should().Throw<Exception>();

            var a = new[] { 10, 2, 38, 23, 38, 23, 21 };
            const decimal aExpectedResult = 22.142857142857142857142857143M;
            CalculationUtils.SafeAverage(a, x => x).Should().Be(aExpectedResult);
            a.SafeAverage(x => x).Should().Be(aExpectedResult);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(4, true)]
        [InlineData(53, false)]
        [InlineData(54, true)]
        [InlineData(100, true)]
        public void IsEven_Passes(int input, bool expected)
        {
            CalculationUtils.IsEven(input).Should().Be(expected);
            input.IsEven().Should().Be(expected);
        }

        [Theory]
        [InlineData(150, 20, 0.20, 30)]
        [InlineData(100, 10, 0.10, 10)]
        [InlineData(100, 98.5658, 0.985658, 98.5658)]
        [InlineData(1110, 50, 0.50, 555)]
        public void SubtractPercentage_Passes(decimal total, decimal percentage, decimal fractionalPercentage, decimal expected)
        {
            total.SubtractPercentage(percentage).Should().Be(expected);
            total.SubtractFractionalPercentage(fractionalPercentage).Should().Be(expected);
        }

        [Theory]
        [InlineData(50, 100, 50, 0.5)]
        [InlineData(98.5658, 100, 98.5658, 0.985658)]
        [InlineData(555, 1110, 50, 0.5)]
        [InlineData(279.72, 1110, 25.2, 0.252)]
        public void PercentageOfTotal_Passes(decimal part, decimal total, decimal expected, decimal expectedFractional)
        {
            CalculationUtils.PercentageOfTotal(part, total).Should().Be(expected);
            part.PercentageOfTotal(total).Should().Be(expected);

            CalculationUtils.FractionalPercentageOfTotal(part, total).Should().Be(expectedFractional);
            part.FractionalPercentageOfTotal(total).Should().Be(expectedFractional);
        }

        [Theory]
        [InlineData(1039, 7056, 10397056)]
        [InlineData(23, 100, 23100)]
        [InlineData(200, 23, 20023)]
        public void Concat_ConcatenatesTwoNumbers(int left, int right, int expected)
        {
            CalculationUtils.Concat(left, right).Should().Be(expected);
            left.Concat(right).Should().Be(expected);
        }

        [Theory]
        [InlineData(10397056, 8)]
        [InlineData(1039, 4)]
        [InlineData(200, 3)]
        [InlineData(23, 2)]
        [InlineData(9, 1)]
        [InlineData(0, 1)]
        public void DigitCount_CountsNumberOfDigits(int input, int expected)
        {
            CalculationUtils.DigitCount(input).Should().Be(expected);
            input.DigitCount().Should().Be(expected);
        }
    }
}
