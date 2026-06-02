using NevesCS.Static.Constants.Values;
using System.Numerics;

namespace NevesCS.Static.Utils
{
    public static class DimensionConversionUtils
    {
        public static T MillimetersToCentimeters<T>(this T value)
            where T : IDivisionOperators<T, int, T>
        {
            return value / Ints.Ten;
        }

        public static T CentimetersToMillimeters<T>(this T value)
            where T : IMultiplyOperators<T, int, T>
        {
            return value * Ints.Ten;
        }
    }
}
