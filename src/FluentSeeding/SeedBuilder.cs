using System.Linq.Expressions;

namespace FluentSeeding;

public sealed class SeedBuilder<T> where T : class
{
    private readonly List<ISeedRule<T>> _rules = new();
    private Func<T>? _factory;
    private int _countMin = 1;
    private int _countMax = 1;

    public SeedBuilder<T> Count(int count)
    {
        _countMin = count;
        _countMax = count;
        return this;
    }
    
    public SeedBuilder<T> Count(int min, int max)
    {
        if (min < 0) throw new ArgumentOutOfRangeException(nameof(min), "Minimum count cannot be negative.");
        if (max < min) throw new ArgumentOutOfRangeException(nameof(max), "Maximum count cannot be less than minimum count.");

        _countMin = min;
        _countMax = max;
        return this;
    }

    public SeedRule<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> selector)
    {
        var rule = new SeedRule<T, TProperty>(selector, this);
        _rules.Add(rule);
        return rule;
    }
    
    public SeedBuilder<T> WithFactory(Func<T> factory)
    {
        _factory = factory;
        return this;
    }
    
    public IEnumerable<T> Build()
    {
        var random = new Random();
        var count = random.Next(_countMin, _countMax + 1);
        var entities = new List<T>();

        for (int i = 0; i < count; i++)
        {
            var entity = _factory != null ? _factory() : Activator.CreateInstance<T>()!;
            foreach (var rule in _rules)
            {
                rule.Apply(entity, i);
            }
            entities.Add(entity);
        }

        return entities;
    }
}
