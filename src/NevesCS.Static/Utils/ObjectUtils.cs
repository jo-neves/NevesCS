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
    }
}
