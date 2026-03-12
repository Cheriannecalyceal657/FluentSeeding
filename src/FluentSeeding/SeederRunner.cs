namespace FluentSeeding;

/// <summary>
/// Orchestrates a collection of <see cref="EntitySeederBase"/> instances, running each in order
/// and persisting their output via an <see cref="IPersistenceLayer"/>.
/// </summary>
/// <remarks>
/// Seeders are executed sequentially in registration order. This matters when seeders have
/// dependencies — a dependent seeder should be registered after the seeder it depends on.
/// </remarks>
public sealed class SeederRunner
{
    private readonly IPersistenceLayer _persistenceLayer;
    private readonly IEnumerable<EntitySeederBase> _seeders;

    /// <param name="persistenceLayer">The store that receives all seeded entities.</param>
    /// <param name="seeders">The ordered collection of seeders to run.</param>
    public SeederRunner(IPersistenceLayer persistenceLayer, IEnumerable<EntitySeederBase> seeders)
    {
        _persistenceLayer = persistenceLayer;
        _seeders = seeders;
    }

    /// <summary>
    /// Runs all seeders synchronously, persisting each in registration order.
    /// </summary>
    public void Run()
    {
        foreach (var seeder in _seeders)
        {
            seeder.PersistTo(_persistenceLayer);
        }
    }

    /// <summary>
    /// Runs all seeders asynchronously, persisting each in registration order.
    /// </summary>
    /// <param name="cancellationToken">Propagated to each seeder's async persist call.</param>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        foreach (var seeder in _seeders)
        {
            await seeder.PersistToAsync(_persistenceLayer, cancellationToken);
        }
    }
}
