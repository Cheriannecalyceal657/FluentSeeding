namespace FluentSeeding;

public static class IdempotentSeedRuleExtensions
{
    public static SeedRule<T, Guid> UseIdempotentGuid<T>(this SeedRule<T, Guid> rule, string? seed = null)
        where T : class
    {
        rule.IndexedValueFactory = i => Idempotent.Guid<T>(i, seed);
        return rule;
    }
    
    public static SeedRule<T, int> UseIdempotentInt<T>(this SeedRule<T, int> rule, string? seed = null)
        where T : class
    {
        rule.IndexedValueFactory = i => Idempotent.Int<T>(i, seed);
        return rule;
    }
    
    public static SeedRule<T, long> UseIdempotentLong<T>(this SeedRule<T, long> rule, string? seed = null)
        where T : class
    {
        rule.IndexedValueFactory = i => Idempotent.Long<T>(i, seed);
        return rule;
    }
    
    public static SeedRule<T, string> UseIdempotentSlug<T>(this SeedRule<T, string> rule, string? seed = null)
        where T : class
    {
        rule.IndexedValueFactory = i => Idempotent.Slug<T>(i, seed);
        return rule;
    }
    
}
