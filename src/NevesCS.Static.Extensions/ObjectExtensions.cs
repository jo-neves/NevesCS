using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull<T>(this T? @object)
        {
            return ObjectUtils.IsNull(@object);
        }

        public static bool IsNullOrDefault<T>(this T? @object)
        {
            return ObjectUtils.IsNullOrDefault(@object);
        }

        public static T ThrowIfNull<T>(this T? @object, Type type)
        {
            return ObjectUtils.ThrowIfNull(@object, type);
        }

        public static T ThrowIfNull<T>(this T? @object, string parameterName)
        {
            return ObjectUtils.ThrowIfNull(@object, parameterName);
        }

        public static T AssertNotNull<T>(this T? @object, string parameterName)
        {
            return ObjectUtils.AssertNotNull(@object, parameterName);
        }

        public static bool IsIn<TIn>(this TIn? @object, IEnumerable<TIn> target)
        {
            return target.Contains(@object);
        }

        /// <summary>
        /// Enumerates the same instance reference (<paramref name="source"/>) times the number defined by <paramref name="repeatTimes"/>.
        ///
        /// </summary>
        public static IEnumerable<T> Enumerate<T>(this T source, int count)
        {
            return ObjectUtils.Enumerate(source, count);
        }

        /// <summary>
        /// Enumerates clones of the source instance times the number defined by <paramref name="count"/>.
        ///
        /// </summary>
        public static IEnumerable<T> EnumerateClones<T>(this ICloneable source, int count)
            where T : ICloneable
        {
            return ObjectUtils.EnumerateClones<T>(source, count);
        }

        /// <summary>
        /// Creates a new array with the object.
        ///
        /// </summary>
        public static object[] ToArray(this object source)
        {
            return ObjectUtils.ToArray(source);
        }

        /// <summary>
        /// Creates a new array with the object.
        ///
        /// </summary>
        public static T[] ToArray<T>(this object source)
        {
            return ObjectUtils.ToArray<T>(source);
        }

        /// <summary>
        /// Return true if the <paramref name="target"/> has the requested property.
        ///
        /// </summary>
        public static bool HasProperty(this object target, string propertyName)
        {
            return ObjectUtils.HasProperty(target, propertyName);
        }

        /// <summary>
        /// <code>
        /// myClassInstance.SetPropertyDynamically(
        ///     nameof(MyClass.WithThisProperty),
        ///     someValueToAdd);
        /// </code>
        /// <code>
        /// myClassInstance.SetPropertyDynamically(
        ///     "ThisProperty.WithThisSubProperty"
        ///     someValueToAdd);
        /// </code>
        /// </summary>
        public static object SetPropertyDynamically(this object target, string propertyPath, object value)
        {
            return ObjectUtils.SetPropertyDynamically(target, propertyPath, value);
        }

        /// <summary>
        /// E.g.:
        /// <code>
        /// myClassInstance.CallMethodOfPropertyDynamically(
        ///     nameof(MyClass.WithThisListProperty),
        ///     nameof(MyClass.WithThisListProperty.AddRange),
        ///     someListOfValuesToAdd);
        /// </code>
        /// </summary>
        public static void CallMethodOfPropertyDynamically(
            this object target,
            string propertyName,
            string methodName,
            object value)
        {
            ObjectUtils.CallMethodOfPropertyDynamically(target, propertyName, methodName, value);
        }
    }
}
