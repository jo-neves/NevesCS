using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            IEnumerableUtils.ForEach(enumeration, action);
        }

        /// <summary>
        /// Computes the average, or returns 0 in case the source is null or empty (does not throw).
        ///
        /// </summary>
        public static decimal SafeAverage<T>(this IEnumerable<T> source, Func<T, decimal> selector)
        {
            return CalculationUtils.SafeAverage(source, selector);
        }

        /// <summary>
        /// Computes the sum, or returns 0 in case the source is null or empty (does not throw).
        ///
        /// </summary>
        public static decimal SafeSum<T>(this IEnumerable<T> source, Func<T, decimal> selector)
        {
            return CalculationUtils.SafeSum(source, selector);
        }

        public static bool None<T>(this IEnumerable<T> enumeration)
        {
            return IEnumerableUtils.None(enumeration);
        }

        public static bool None<T>(this IEnumerable<T> enumeration, Func<T, bool> predicate)
        {
            return IEnumerableUtils.None(enumeration, predicate);
        }

        public static bool AreAnyNull<T>(IEnumerable<T> values)
        {
            return IEnumerableUtils.AreAnyNull(values);
        }

        public static T? TryGetElementAtOr<T>(this IEnumerable<T> enumeration, Index index, T? defaultValue = default)
        {
            return IEnumerableUtils.TryGetElementAtOr(enumeration, index, defaultValue);
        }

        public static IEnumerable<TValue> OrEmpty<TValue>(this IEnumerable<TValue>? enumeration)
        {
            return IEnumerableUtils.OrEmpty(enumeration);
        }

        public static bool ContainsObjectValue<TObject, TValue>(this IEnumerable<TValue> enumeration, TObject testObject, Func<TObject, TValue> selector)
        {
            return IEnumerableUtils.ContainsObjectValue(enumeration, testObject, selector);
        }
    }
}
