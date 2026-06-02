using NevesCS.Static.Utils.Vendor;

using TechTalk.SpecFlow;

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
            return SpecFlowUtils.CreateInstanceByTypeName<KnownType>(table, typeName);
        }
    }
}
