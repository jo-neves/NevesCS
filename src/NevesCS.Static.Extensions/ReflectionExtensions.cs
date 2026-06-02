using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Return true if the <paramref name="target"/> has the requested property.
        ///
        /// </summary>
        public static bool HasProperty(this object target, string propertyName)
        {
            return ReflectionUtils.HasProperty(target, propertyName);
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
            return ReflectionUtils.SetPropertyDynamically(target, propertyPath, value);
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
            ReflectionUtils.CallMethodOfPropertyDynamically(target, propertyName, methodName, value);
        }
    }
}
