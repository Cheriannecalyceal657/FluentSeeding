namespace FluentSeeding;

public sealed class SeederRunner
{
    private readonly IPersistenceLayer _persistenceLayer;
    private readonly IEnumerable<EntitySeederBase> _seeders;
    
    public SeederRunner(IPersistenceLayer persistenceLayer, IEnumerable<EntitySeederBase> seeders)
    {
        _persistenceLayer = persistenceLayer;
        _seeders = seeders;
    }
    
    public void Run()
    {
        foreach (var seeder in _seeders)
        {
            var entities = seeder.SeedInternal();
            _persistenceLayer.Persist(entities);
        }
    }
    
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        foreach (var seeder in _seeders)
        {
            var entities = seeder.SeedInternal();
            await _persistenceLayer.PersistAsync(entities, cancellationToken);
        }
    }
}
