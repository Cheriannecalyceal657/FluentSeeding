namespace FluentSeeding;

public abstract class EntitySeederBase
{
    internal abstract IEnumerable<object> SeedInternal();
}
