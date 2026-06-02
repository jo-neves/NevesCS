using NevesCS.Static.Utils;

using System.Linq.Expressions;
using System.Reflection;

namespace NevesCS.Static.Extensions
{
    public static class ExpressionExtensions
    {
        public static string? GetPropertyName(this Expression expression)
        {
            return ReflectionUtils.GetPropertyName(expression);
        }

        public static string? GetPropertyName<TTarget>(this Expression<Func<TTarget, object?>> propertySelector)
        {
            return ReflectionUtils.GetPropertyName(propertySelector);
        }

        public static PropertyInfo? GetPropertyInfo(this Expression expression)
        {
            return ReflectionUtils.GetPropertyInfo(expression);
        }

        public static PropertyInfo? GetPropertyInfo<TTarget>(this Expression<Func<TTarget, object?>> propertySelector)
        {
            return ReflectionUtils.GetPropertyInfo(propertySelector);
        }

        public static MemberInfo? GetMemberInfo(this Expression expression)
        {
            return ReflectionUtils.GetPropertyInfo(expression);
        }
    }
}
