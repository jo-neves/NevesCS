using NevesCS.Static.Utils;

using System.Reflection;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NevesCS.Static.Extensions.Vendor
{
    public static class SpecFlowExtensions
    {
        /// <summary>
        /// Dynamically creates a type instance from <paramref name="table"/> (<see cref="Table"/>) by <paramref name="typeName"/>,
        /// supporting external assemblies.
        ///
        /// </summary>
        /// <typeparam name="KnownType">A known type embedded in the Assembly where the <paramref name="typeName"/> belongs to.</typeparam>
        /// <param name="table"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static object? CreateInstanceByTypeName<KnownType>(this Table table, string typeName)
        {
            var modelType = ReflectionUtils.GetTypeFrom<KnownType>(typeName);

            return table.GetFirstCreateInstanceMethodInfo(modelType)!.CallFirstCreateInstanceMethodInfo(table);
        }

        private static MethodInfo? GetFirstCreateInstanceMethodInfo(this Table? _, Type genericType)
        {
            var tableCreateInstanceMethod = typeof(TableHelperExtensionMethods)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(method => method.Name == nameof(TableHelperExtensionMethods.CreateInstance)
                                 && method.GetGenericArguments().Length == 1)
                !
                .First();

            return tableCreateInstanceMethod.MakeGenericMethod(genericType);
        }

        private static object? CallFirstCreateInstanceMethodInfo(this MethodInfo tableCreateInstanceMethod, Table table)
        {
            return tableCreateInstanceMethod.CallFirstCreateInstanceMethodInfo<object?>(table);
        }

        private static T? CallFirstCreateInstanceMethodInfo<T>(this MethodInfo tableCreateInstanceMethod, Table table)
        {
            return (T?)tableCreateInstanceMethod.Invoke(null, new[] { table });
        }
    }
}
