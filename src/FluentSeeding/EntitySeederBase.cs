namespace FluentSeeding;

/// <summary>
/// Non-generic base class for all entity seeders.
/// Allows <see cref="SeederRunner"/> to orchestrate heterogeneous seeders without knowing their entity types.
/// Derive from <see cref="EntitySeeder{T}"/> rather than this class directly.
/// </summary>
public abstract class EntitySeederBase
{
    internal abstract IEnumerable<object> SeedInternal();
    internal abstract void PersistTo(IPersistenceLayer persistenceLayer);
    internal abstract Task PersistToAsync(IPersistenceLayer persistenceLayer, CancellationToken cancellationToken);
}
