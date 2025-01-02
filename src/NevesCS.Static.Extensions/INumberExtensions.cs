using System.Numerics;

using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class INumberExtensions
    {
        public static T Add<T>(this T left, T right)
            where T : INumber<T>
        {
            return CalculationUtils.Add(left, right);
        }

        public static T Subtract<T>(this T left, T right)
            where T : INumber<T>
        {
            return CalculationUtils.Subtract(left, right);
        }

        public static T Multiply<T>(this T left, T right)
            where T : INumber<T>
        {
            return CalculationUtils.Multiply(left, right);
        }

        public static T Divide<T>(this T left, T right)
            where T : INumber<T>
        {
            return CalculationUtils.Divide(left, right);
        }
    }
}
