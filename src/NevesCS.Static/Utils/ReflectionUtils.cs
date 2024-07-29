using System.Linq.Expressions;
using System.Reflection;

namespace NevesCS.Static.Utils
{
    public static class ReflectionUtils
    {
        public static string? GetPropertyName(Expression expression)
        {
            return GetPropertyInfo(expression)?.Name;
        }

        public static string? GetPropertyName<TTarget>(Expression<Func<TTarget, object?>> propertySelector)
        {
            return GetPropertyInfo(propertySelector)?.Name;
        }

        public static PropertyInfo? GetPropertyInfo(Expression expression)
        {
            return (PropertyInfo?)GetMemberInfo(expression);
        }

        public static PropertyInfo? GetPropertyInfo<TTarget>(Expression<Func<TTarget, object?>> propertySelector)
        {
            return (PropertyInfo?)GetMemberInfo(propertySelector);
        }

        public static MemberInfo? GetMemberInfo(Expression expression)
        {
            return expression switch
            {
                MemberExpression memberExpression => memberExpression.Member,
                MethodCallExpression methodCallExpression => methodCallExpression.Method,

                UnaryExpression unaryExpression => unaryExpression.Operand is MethodCallExpression methodExpression
                                                       ? methodExpression.Method
                                                       : ((MemberExpression)unaryExpression.Operand).Member,

                LambdaExpression lambdaExpression => (lambdaExpression.Body is UnaryExpression unaryLambdaExpression
                                                         ? (MemberExpression)unaryLambdaExpression.Operand
                                                         : (MemberExpression)lambdaExpression.Body)
                                                     .Member,

                _ => null,
            };
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
