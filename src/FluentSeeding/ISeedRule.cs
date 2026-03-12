namespace FluentSeeding;

public interface ISeedRule<in T>
{
    void Apply(T instance, int index = 0);
}