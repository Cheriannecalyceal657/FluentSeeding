using System.Linq.Expressions;

namespace FluentSeeding;

public sealed class SeedRule<T, TProperty> : ISeedRule<T>
where T : class
{
    private readonly Expression<Func<T, TProperty>> _selector;
    private readonly Action<T, TProperty> _setter;

    private Func<TProperty>? _valueFactory
    {
        get;
        set
        {
            if (field == value) return;
            _indexedValueFactory = null;
            field = value;
        }
    }

    private Func<int, TProperty>? _indexedValueFactory
    {
        get;
        set
        {
            if (field == value) return;
            _valueFactory = null;
            field = value;
        }
    }

    internal SeedRule(Expression<Func<T, TProperty>> selector)
    {
        _selector = selector;
        _setter = selector.BuildSetter();
    }

    public SeedRule<T, TProperty> UseValue(TProperty value)
    {
        _valueFactory = () => value;
        return this;
    }

    public SeedRule<T, TProperty> UseFactory(Func<TProperty> value)
    {
        _valueFactory = value;
        return this;
    }
    
    public SeedRule<T, TProperty> UseFactory(Func<int, TProperty> value)
    {
        _indexedValueFactory = value;
        return this;
    }

    public SeedRule<T, TProperty> UseFrom(params TProperty[] values)
    {
        var random = new Random();
        _valueFactory = () => values[random.Next(values.Length)];
        return this;
    }

    public void Apply(T instance, int index = 0)
    {
        if (_valueFactory is null && _indexedValueFactory is null)
            throw new InvalidOperationException($"No value or factory configured for '{_selector}.");
        
        if (_valueFactory != null)
        {
            _setter(instance, _valueFactory());
        }
        else if (_indexedValueFactory != null)
        {
            _setter(instance, _indexedValueFactory(index));
        }
    }
}
