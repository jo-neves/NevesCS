using System.Diagnostics;

namespace NevesCS.Static.Utils
{
    public static class StringUtils
    {
        public static string CloneIntoNew(string source)
        {
            return new string(source);
        }

        public static string ThrowIfNullOrEmpty(string? @string, string parameterName)
        {
            if (string.IsNullOrEmpty(@string))
            {
                throw new ArgumentNullException(parameterName);
            }

            return @string!;
        }

        public static string AssertIfNullOrEmpty(string? @string, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(@string), parameterName);

            return @string!;
        }

        public static string ThrowIfNullOrWhiteSpace(string? @string, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(@string))
            {
                throw new ArgumentNullException(parameterName);
            }

            return @string!;
        }

        public static string AssertIfNullOrWhiteSpace(string? @string, string parameterName)
        {
            Debug.Assert(string.IsNullOrWhiteSpace(@string), parameterName);

            return @string!;
        }

        public static bool EqualsIgnoreCase(string? source, string target)
        {
            return source?.Equals(target, StringComparison.OrdinalIgnoreCase) == true;
        }

        public static Guid HashIntoGuid(string source)
        {
            return GuidUtils.HashStringIntoGuid(source);
        }
    }
}
