namespace NevesCS.Static.Utils
{
    public static class FunctionalUtils
    {
        public static TOut Into<TIn, TOut>(TIn source, Func<TIn, TOut> convertFunction)
        {
            return convertFunction(source);
        }

        public static T Set<T>(T target, Action<T> setter)
        {
            setter(target);

            return target;
        }

        public static T? SetIfNotNull<T>(T? target, Action<T> setter)
        {
            if (ObjectUtils.IsNull(target))
            {
                return target;
            }

            return Set(target!, setter);
        }

        public static T? OrIfNull<T>(T? target, Func<T> factoryFunction)
        {
            if (!ObjectUtils.IsNull(target))
            {
                return target;
            }

            return factoryFunction();
        }
    }
}
