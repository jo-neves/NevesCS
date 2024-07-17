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

        public static T? SetIfNotNull<T>(this T? target, Action<T> setter)
        {
            return FunctionalUtils.SetIfNotNull(target, setter);
        }

        public static T? OrIfNull<T>(this T? target, Func<T> factoryFunction)
        {
            return FunctionalUtils.OrIfNull(target, factoryFunction);
        }
    }
}
