using System.Linq.Expressions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.SeedRule;

public abstract class SeedRuleTest
{
    protected static SeedRule<User, TProperty> CreateRule<TProperty>(Expression<Func<User, TProperty>> selector)
    {
        return new SeedRule<User, TProperty>(selector);
    }
}
