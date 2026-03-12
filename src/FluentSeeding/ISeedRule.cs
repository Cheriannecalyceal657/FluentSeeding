namespace FluentSeeding;

public interface ISeedRule<T>
{
    void Apply(T instance);
}