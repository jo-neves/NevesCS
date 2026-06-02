using NevesCS.Static.Constants.Values;

using System.Numerics;

namespace NevesCS.Static.Utils
{
    public static class TimeConversionUtils
    {
        public static T MinutesToSeconds<T>(this T value)
            where T : IMultiplyOperators<T, int, T>
        {
            return value * Ints.Sixty;
        }

        public static T SecondsToMinutes<T>(this T value)
            where T : IDivisionOperators<T, int, T>
        {
            return value / Ints.Sixty;
        }

        public static T HoursToSeconds<T>(this T value)
            where T : IMultiplyOperators<T, int, T>
        {
            return value * 3600;
        }

        public static T SecondsToHours<T>(this T value)
            where T : IDivisionOperators<T, int, T>
        {
            return value / 3600;
        }

        public static T DaysToSeconds<T>(this T value)
            where T : IMultiplyOperators<T, int, T>
        {
            return value * 86400;
        }

        public static T SecondsToDays<T>(this T value)
            where T : IDivisionOperators<T, int, T>
        {
            return value / 86400;
        }
    }
}
