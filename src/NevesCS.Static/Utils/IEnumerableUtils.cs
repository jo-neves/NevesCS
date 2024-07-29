namespace NevesCS.Static.Utils
{
    public static class IEnumerableUtils
    {
        /// <summary>
        /// It materializes/consumes the enumerable and performs a foreach in it.
        ///
        /// </summary>
        public static void ForEach<T>(IEnumerable<T> enumeration, Action<T> action)
        {
            Array.ForEach(enumeration.ToArray(), action);
        }

        public static bool None<T>(IEnumerable<T> enumeration)
        {
            return !enumeration.Any();
        }

        public static bool None<T>(IEnumerable<T> enumeration, Func<T, bool> predicate)
        {
            return !enumeration.Any(predicate);
        }

        public static T? TryGetElementAtOr<T>(IEnumerable<T> enumeration, Index index, T? defaultValue = default)
        {
            try
            {
                return enumeration.ElementAt(index);
            }
            catch (IndexOutOfRangeException)
            {
                return defaultValue;
            }
        }

        public static IEnumerable<TValue> OrEmpty<TValue>(IEnumerable<TValue>? enumeration)
        {
            return ObjectUtils.IsNull(enumeration)
                ? []
                : enumeration!;
        }

        public static bool ContainsObjectValue<TObject, TValue>(IEnumerable<TValue> enumeration, TObject testObject, Func<TObject, TValue> selector)
        {
            return enumeration.Contains(selector(testObject));
        }
    }
}
