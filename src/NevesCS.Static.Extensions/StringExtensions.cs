using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class StringExtensions
    {
        public static string CloneIntoNew(this string source)
        {
            return new string(source);
        }

        public static string ThrowIfNullOrEmpty(this string? @object, string parameterName)
        {
            return StringUtils.ThrowIfNullOrEmpty(@object, parameterName);
        }

        public static string AssertIfNullOrEmpty(this string? @object, string parameterName)
        {
            return StringUtils.AssertIfNullOrEmpty(@object, parameterName);
        }

        public static string ThrowIfNullOrWhiteSpace(this string? @object, string parameterName)
        {
            return StringUtils.ThrowIfNullOrWhiteSpace(@object, parameterName);
        }

        public static string AssertIfNullOrWhiteSpace(this string? @object, string parameterName)
        {
            return StringUtils.AssertIfNullOrWhiteSpace(@object, parameterName);
        }

        /// <summary>
        /// Returns <paramref name="target"/>, or <paramref name="defaultValue"/> if <typeparamref name="null"/> or empty.
        ///
        /// </summary>
        public static string OrIfNullOrEmpty(this string? target, string defaultValue)
        {
            return StringUtils.OrIfNullOrEmpty(target, defaultValue);
        }

        /// <summary>
        /// Returns <paramref name="target"/>, or <paramref name="defaultValue"/> if <typeparamref name="null"/> or white space.
        ///
        /// </summary>
        public static string OrIfNullOrWhiteSpace(this string? target, string defaultValue)
        {
            return StringUtils.OrIfNullOrWhiteSpace(target, defaultValue);
        }

        public static bool EqualsIgnoreCase(this string? source, string target)
        {
            return StringUtils.EqualsIgnoreCase(source, target);
        }

        public static Guid HashStringIntoGuid(this string source)
        {
            return GuidUtils.HashStringIntoGuid(source);
        }

        public static async Task<Guid> HashStringIntoGuidAsync(this string source)
        {
            return await GuidUtils.HashStringIntoGuidAsync(source);
        }
    }
}
