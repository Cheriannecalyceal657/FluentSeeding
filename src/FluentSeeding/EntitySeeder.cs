namespace FluentSeeding;

/// <summary>
/// Base class for a typed entity seeder. Subclasses implement <see cref="Configure"/> to define
/// how entities of type <typeparamref name="T"/> are built and populated.
/// </summary>
/// <typeparam name="T">The entity type to seed. Must be a reference type.</typeparam>
/// <remarks>
/// <see cref="Data"/> is lazily evaluated and cached — the seed runs only once per seeder instance.
/// Other seeders can reference this seeder's <see cref="Data"/> to build cross-entity relationships.
/// </remarks>
public abstract class EntitySeeder<T> : EntitySeederBase
    where T : class
{
    private List<T>? _seedData;

    /// <summary>
    /// The seeded entities, generated on first access and cached for the lifetime of this instance.
    /// Use this property when other seeders need to reference seeded data to build relationships.
    /// </summary>
    public IReadOnlyList<T> Data => _seedData ??= Seed().ToList();

    /// <summary>
    /// Override to configure the <see cref="SeedBuilder{T}"/> — set the entity count,
    /// register property rules, and optionally supply a factory.
    /// </summary>
    protected abstract void Configure(SeedBuilder<T> builder);

    /// <summary>
    /// Generates entities according to the configuration in <see cref="Configure"/>.
    /// Does not persist entities or cache the result; use <see cref="Data"/> for caching.
    /// </summary>
    public IEnumerable<T> Seed()
    {
        var builder = new SeedBuilder<T>();
        Configure(builder);
        return builder.Build();
    }

    internal sealed override IEnumerable<object> SeedInternal()
    {
        return Seed();
    }

    internal override void PersistTo(IPersistenceLayer persistenceLayer)
    {
        persistenceLayer.Persist(Data);
    }

    internal override Task PersistToAsync(IPersistenceLayer persistenceLayer, CancellationToken cancellationToken)
    {
        return persistenceLayer.PersistAsync(Data, cancellationToken);
    }
}
