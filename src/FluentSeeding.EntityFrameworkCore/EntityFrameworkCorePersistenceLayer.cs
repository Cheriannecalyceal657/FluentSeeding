using Microsoft.EntityFrameworkCore;

namespace FluentSeeding.EntityFrameworkCore;

internal sealed class EntityFrameworkCorePersistenceLayer : IPersistenceLayer
{
    private readonly DbContext _dbContext;

    public EntityFrameworkCorePersistenceLayer(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Persist<T>(IEnumerable<T> entities) where T : class
    {
        _dbContext.Set<T>().AddRange(entities);
    }

    public Task PersistAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
    {
        _dbContext.Set<T>().AddRange(entities);
        return Task.CompletedTask;
    }

    public void Flush()
    {
        _dbContext.SaveChanges();
    }

    public Task FlushAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
