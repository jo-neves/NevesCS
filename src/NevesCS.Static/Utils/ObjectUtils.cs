using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace NevesCS.Static.Utils
{
    public static class ObjectUtils
    {
        public static bool IsNull<T>(T? @object)
        {
            return @object == null;
        }

        public static bool IsNullOrDefault<T>(T? @object)
        {
            return @object == null || Equals(@object, default(T?));
        }

        public static T ThrowIfNull<T>(T? @object, Type type)
        {
            if (IsNull(@object))
            {
                throw new ArgumentNullException(type.Name);
            }

            return @object!;
        }

        public static T ThrowIfNull<T>(T? @object, string parameterName)
        {
            if (IsNull(@object))
            {
                throw new ArgumentNullException(parameterName);
            }

            return @object!;
        }

        public static T AssertNotNull<T>(T? @object, string parameterName)
        {
            Debug.Assert(!IsNull(@object), parameterName);

            return @object!;
        }

        /// <summary>
        /// Enumerates all parameters in <paramref name="targets"/>.
        ///
        /// </summary>
        public static IEnumerable<T> Enumerate<T>(params T[] targets)
        {
            foreach (var item in targets)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Repeats the same instance (<paramref name="source"/>) times the number defined by <paramref name="count"/>.
        ///
        /// </summary>
        public static IEnumerable<T> Enumerate<T>(T source, int count)
        {
            for (int i = 1; i <= count; ++i)
            {
                yield return source;
            }
        }

        /// <summary>
        /// Enumerates clones of the source instance times the number defined by <paramref name="count"/>.
        ///
        /// </summary>
        public static IEnumerable<T> EnumerateClones<T>(ICloneable source, int count = 0)
            where T : ICloneable
        {
            for (int i = 1; i <= count; ++i)
            {
                yield return (T)source.Clone();
            }
        }

        /// <summary>
        /// Creates a new array with the object.
        ///
        /// </summary>
        public static object[] ToArray(object source)
        {
            return ToArray<object>(source);
        }

        /// <summary>
        /// Creates a new array with the object.
        ///
        /// </summary>
        public static T[] ToArray<T>(object source)
        {
            return [(T)source];
        }

        /// <summary>
        /// Return true if the <paramref name="target"/> has the requested property.
        ///
        /// </summary>
        public static bool HasProperty(object target, string propertyName)
        {
            return target
                .GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                != null;
        }

        /// <summary>
        /// <code>
        /// ObjectUtils.SetPropertyDynamically(
        ///     myClassInstance,
        ///     nameof(MyClass.WithThisProperty),
        ///     someValueToAdd);
        /// </code>
        /// <code>
        /// ObjectUtils.SetPropertyDynamically(
        ///     myClassInstance,
        ///     "ThisProperty.WithThisSubProperty"
        ///     someValueToAdd);
        /// </code>
        /// </summary>
        public static object SetPropertyDynamically(object target, string propertyPath, object value)
        {
            var subPropertyNames = propertyPath.Split('.');

            for (int i = 0; i < subPropertyNames.Length - 1; i++)
            {
                var property = target.GetType().GetProperty(subPropertyNames[i]);
                target = property.GetValue(target) ?? throw new NullReferenceException(subPropertyNames[i]);
            }

            var finalProperty = target
                .GetType()
                .GetProperty(subPropertyNames.Last())
                ?? throw new MissingFieldException(subPropertyNames.Last());

            finalProperty.SetValue(target, value);

            return target;
        }

        /// <summary>
        /// E.g.:
        /// <code>
        /// ObjectUtils.CallMethodOfPropertyDynamically(
        ///     myClassInstance,
        ///     nameof(MyClass.WithThisListProperty),
        ///     nameof(MyClass.WithThisListProperty.AddRange),
        ///     someListOfValuesToAdd);
        /// </code>
        /// </summary>
        public static void CallMethodOfPropertyDynamically(
            object target,
            string propertyName,
            string methodName,
            object value)
        {
            var propertyInfo = target
                .GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                ?? throw new MissingFieldException(propertyName);

            var methodInfo = propertyInfo
                .PropertyType
                .GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance)
                ?? throw new MissingMethodException(methodName);

            var targetProperty = propertyInfo.GetValue(target);

            if (!methodInfo.IsStatic && targetProperty == null)
            {
                throw new NullReferenceException(propertyName);
            }

            methodInfo.Invoke(targetProperty, [value]);
        }
    }
}
