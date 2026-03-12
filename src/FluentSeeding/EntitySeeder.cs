namespace FluentSeeding;

public abstract class EntitySeeder<T> : EntitySeederBase
    where T : class
{
    protected abstract void Configure(SeedBuilder<T> builder);

    public IEnumerable<T> Seed()
    {
        var builder = new SeedBuilder<T>();
        Configure(builder);
        return builder.Build();
    }
    
    internal override IEnumerable<object> SeedInternal()
    {
        return Seed();
    }
}
