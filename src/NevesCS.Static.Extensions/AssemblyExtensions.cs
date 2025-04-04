using NevesCS.Static.Utils;

using System.Reflection;

namespace NevesCS.Static.Extensions
{
    public static class AssemblyExtensions
    {
        public static Type GetTypeByName(this Assembly assembly, string typeName)
        {
            return ReflectionUtils.GetTypeByNameFromAssembly(assembly, typeName);
        }
    }
}
