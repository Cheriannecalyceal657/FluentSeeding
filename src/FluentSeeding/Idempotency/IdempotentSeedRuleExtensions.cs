namespace FluentSeeding;

public static class IdempotentSeedRuleExtensions
{
    public static SeedBuilder<T> UseIdempotentGuid<T>(this SeedRule<T, Guid> rule, string? seed = null)
        where T : class
    {
        rule.IndexedValueFactory = i => Idempotent.Guid<T>(i, seed);
        return rule.Parent;
    }
    
    public static SeedBuilder<T> UseIdempotentInt<T>(this SeedRule<T, int> rule, string? seed = null)
        where T : class
    {
        rule.IndexedValueFactory = i => Idempotent.Int<T>(i, seed);
        return rule.Parent;
    }
    
    public static SeedBuilder<T> UseIdempotentLong<T>(this SeedRule<T, long> rule, string? seed = null)
        where T : class
    {
        rule.IndexedValueFactory = i => Idempotent.Long<T>(i, seed);
        return rule.Parent;
    }
    
    public static SeedBuilder<T> UseIdempotentSlug<T>(this SeedRule<T, string> rule, string? seed = null)
        where T : class
    {
        rule.IndexedValueFactory = i => Idempotent.Slug<T>(i, seed);
        return rule.Parent;
    }
    
}
