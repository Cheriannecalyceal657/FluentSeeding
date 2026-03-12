using System.Linq.Expressions;

namespace FluentSeeding;

internal static class ExpressionExtensions
{
    public static Action<T, TProperty> BuildSetter<T, TProperty>(
        this Expression<Func<T, TProperty>> selector)
    {
        var instance = selector.Parameters[0];              
        var member   = (MemberExpression)selector.Body;      
        var value    = Expression.Parameter(typeof(TProperty), "value");
        var assign   = Expression.Assign(member, value);     
        return Expression.Lambda<Action<T, TProperty>>(assign, instance, value)
            .Compile();
    }
}