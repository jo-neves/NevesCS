using System.Diagnostics.CodeAnalysis;

namespace NevesCS.Static.Constants.Values
{
    [ExcludeFromCodeCoverage]
    public static class Strings
    {
        public const string Space = " ";

        public const string Underscore = "_";

        public const string Minus = "-";

        public const string Equal = "=";

        public static string EmptyGuid { get; } = Guid.Empty.ToString();
    }
}
