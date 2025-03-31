using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class FunctionalExtensions
    {
        public static TOut Into<TIn, TOut>(this TIn source, Func<TIn, TOut> convertFunction)
        {
            return FunctionalUtils.Into(source, convertFunction);
        }

        public static T Set<T>(this T target, Action<T> setter)
        {
            return FunctionalUtils.Set(target, setter);
        }

        /// <summary>
        /// Executes <paramref name="setter"/>, if <paramref name="target"/> is not <typeparamref name="null"/>.
        ///
        /// </summary>
        public static T? SetIfNotNull<T>(this T? target, Action<T> setter)
        {
            return FunctionalUtils.SetIfNotNull(target, setter);
        }

        /// <summary>
        /// Handles the null by not throwing it, and executes <paramref name="factoryFunction"/>.
        ///
        /// </summary>
        public static T? OrIfNull<T>(this T? target, Func<T> factoryFunction)
        {
            return FunctionalUtils.OrIfNull(target, factoryFunction);
        }
    }
}
