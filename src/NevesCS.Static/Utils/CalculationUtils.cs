using NevesCS.Static.Constants.Values;
using System.Numerics;

namespace NevesCS.Static.Utils
{
    public static class CalculationUtils
    {
        /// <summary>
        /// Computes the average and returns 0 in case the source is null or empty (does not throw).
        ///
        /// </summary>
        public static decimal SafeAverage<T>(IEnumerable<T> source, Func<T, decimal> selector)
            where T : INumber<T>
        {
            return source?.Any() != true
                ? 0
                : source.Average(selector);
        }

        public static bool IsEven(int value)
        {
            return value % Ints.Two == Ints.Zero;
        }

        /// <summary>
        /// Returns a percentage of the original number.
        /// E.g.: 150.SubtractPercentage(20) == 30
        ///
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static decimal SubtractPercentage(decimal total, decimal percentage)
        {
            return percentage is < 0 or > 100
                ? throw new ArgumentOutOfRangeException(nameof(percentage))
                : SubtractFractionalPercentage(total, percentage / Ints.OneHundred);
        }

        /// <summary>
        /// Returns a percentage of the original number.
        /// E.g.: 150.SubtractFractionalPercentage(0.20) == 30
        ///
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static decimal SubtractFractionalPercentage(decimal total, decimal fractionalPercentage)
        {
            return fractionalPercentage is < 0 or > 1
                ? throw new ArgumentOutOfRangeException(nameof(fractionalPercentage))
                : total * fractionalPercentage;
        }

        /// <summary>
        /// Returns the percentage representation in 0-100 format.
        /// E.g.: PercentageOfTotal(10, 100) == 10
        ///
        /// </summary>
        public static decimal PercentageOfTotal(decimal part, decimal total)
        {
            return total == 0
                ? throw new ArgumentOutOfRangeException(nameof(total))
                : (part * Ints.OneHundred) / total;
        }

        /// <summary>
        /// Returns the percentage representation in 0-1 format.
        /// E.g.: FractionalPercentageOfTotal(10, 100) == 0.1
        ///
        /// </summary>
        public static decimal FractionalPercentageOfTotal(decimal part, decimal total)
        {
            return total == 0
                ? throw new ArgumentOutOfRangeException(nameof(total))
                : PercentageOfTotal(part, total) / Ints.OneHundred;
        }

        public static int Concat(int left, int right)
        {
            return (left * ((int)Math.Pow(Ints.Ten, DigitCount(right)))) + right;
        }

        public static int DigitCount(int value)
        {
            var counter = Ints.One;
            var widthMeter = Ints.Ten;

            while (widthMeter <= value)
            {
                widthMeter *= Ints.Ten;
                ++counter;
            }

            return counter;
        }

        public static T Add<T>(T left, T right)
            where T : INumber<T>
        {
            return left + right;
        }

        public static T Subtract<T>(T left, T right)
            where T : INumber<T>
        {
            return left - right;
        }

        public static T Multiply<T>(T left, T right)
            where T : INumber<T>
        {
            return left * right;
        }

        public static T Divide<T>(T left, T right)
            where T : INumber<T>
        {
            return left / right;
        }
    }
}
