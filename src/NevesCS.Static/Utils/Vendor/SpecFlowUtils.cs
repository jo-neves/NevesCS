using System.Reflection;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NevesCS.Static.Utils.Vendor
{
    public static class SpecFlowUtils
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
        public static object? CreateInstanceByTypeName<KnownType>(Table table, string typeName)
        {
            var modelType = ReflectionUtils.GetTypeFrom<KnownType>(typeName);

            return CallFirstCreateInstanceMethodInfo(GetFirstCreateInstanceMethodInfo(table, modelType)!, table);
        }

        private static MethodInfo? GetFirstCreateInstanceMethodInfo(Table? _, Type genericType)
        {
            var tableCreateInstanceMethod = typeof(TableHelperExtensionMethods)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(method => method.Name == nameof(TableHelperExtensionMethods.CreateInstance)
                                 && method.GetGenericArguments().Length == 1)
                !
                .First();

            return tableCreateInstanceMethod.MakeGenericMethod(genericType);
        }

        private static object? CallFirstCreateInstanceMethodInfo(MethodInfo tableCreateInstanceMethod, Table table)
        {
            return CallFirstCreateInstanceMethodInfo<object?>(tableCreateInstanceMethod, table);
        }

        private static T? CallFirstCreateInstanceMethodInfo<T>(MethodInfo tableCreateInstanceMethod, Table table)
        {
            return (T?)tableCreateInstanceMethod.Invoke(null, [table]);
        }
    }
}
