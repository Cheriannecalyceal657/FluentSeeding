namespace FluentSeeding;

public interface IPersistenceLayer
{
    void Persist<T>(IEnumerable<T> entities);
    Task PersistAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}