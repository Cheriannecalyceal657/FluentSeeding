namespace FluentSeeding;

/// <summary>
/// Abstracts the storage mechanism that receives seeded entities.
/// Implement this interface to target a specific store (e.g. EF Core, in-memory, JSON file).
/// </summary>
public interface IPersistenceLayer
{
    /// <summary>
    /// Persists a batch of seeded entities to the underlying store.
    /// </summary>
    /// <param name="entities">The entities to store. Must not be <see langword="null"/>.</param>
    void Persist<T>(IEnumerable<T> entities) where T : class;

    /// <summary>
    /// Asynchronously persists a batch of seeded entities to the underlying store.
    /// </summary>
    /// <param name="entities">The entities to store. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">Propagated to the underlying async store operation.</param>
    Task PersistAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class;
}