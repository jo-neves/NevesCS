using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class DecimalExtensions
    {
        public static long EncodeAsLong(this decimal source)
        {
            return NumericalConversionUtils.EncodeDecimalAsLong(source);
        }

        /// <summary>
        /// Returns a percentage of the original number.
        /// E.g.: 150.SubtractPercentage(20) == 30
        ///
        /// </summary>
        public static decimal SubtractPercentage(this decimal total, decimal percentage)
        {
            return CalculationUtils.SubtractPercentage(total, percentage);
        }

        /// <summary>
        /// Returns a percentage of the original number.
        /// E.g.: 150.SubtractFractionalPercentage(0.20) == 30
        ///
        /// </summary>
        public static decimal SubtractFractionalPercentage(this decimal total, decimal fractionalPercentage)
        {
            return CalculationUtils.SubtractFractionalPercentage(total, fractionalPercentage);
        }

        /// <summary>
        /// Returns the percentage representation in 0-100 format.
        /// E.g.: 10.PercentageOfTotal(100) == 10
        ///
        /// </summary>
        public static decimal PercentageOfTotal(this decimal part, decimal percentage)
        {
            return CalculationUtils.PercentageOfTotal(part, percentage);
        }

        /// <summary>
        /// Returns the percentage representation in 0-1 format.
        /// E.g.: 10.FractionalPercentageOfTotal(100) == 0.10
        ///
        /// </summary>
        public static decimal FractionalPercentageOfTotal(this decimal part, decimal percentage)
        {
            return CalculationUtils.FractionalPercentageOfTotal(part, percentage);
        }
    }
}
