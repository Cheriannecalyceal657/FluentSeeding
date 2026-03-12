using System.Linq.Expressions;

namespace FluentSeeding;

/// <summary>
/// Encapsulates how a single property of <typeparamref name="T"/> is populated during seeding.
/// Obtained via <see cref="SeedBuilder{T}.RuleFor{TProperty}"/>; chain one of
/// <see cref="UseValue"/>, <see cref="UseFactory(Func{TProperty})"/>, or <see cref="UseFrom(TProperty[])"/>
/// to configure the value source, which returns the parent <see cref="SeedBuilder{T}"/>.
/// </summary>
/// <typeparam name="T">The entity type that owns the property.</typeparam>
/// <typeparam name="TProperty">The type of the property being seeded.</typeparam>
public sealed class SeedRule<T, TProperty> : ISeedRule<T>
where T : class
{
    private readonly Expression<Func<T, TProperty>> _selector;
    private readonly Action<T, TProperty> _setter;
    private readonly SeedBuilder<T> _parent;

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

    internal SeedRule(Expression<Func<T, TProperty>> selector, SeedBuilder<T> parent)
    {
        _selector = selector;
        _parent = parent;
        _setter = selector.BuildSetter();
    }

    /// <summary>
    /// Assigns the same constant <paramref name="value"/> to this property on every seeded entity.
    /// </summary>
    public SeedBuilder<T> UseValue(TProperty value)
    {
        _valueFactory = () => value;
        return _parent;
    }

    /// <summary>
    /// Invokes <paramref name="value"/> once per entity to produce the property value.
    /// Use this when each entity should receive a freshly generated value (e.g. <c>Guid.NewGuid</c>).
    /// </summary>
    /// <remarks>Setting this clears any previously configured indexed factory.</remarks>
    public SeedBuilder<T> UseFactory(Func<TProperty> value)
    {
        _valueFactory = value;
        return _parent;
    }

    /// <summary>
    /// Invokes <paramref name="value"/> once per entity, passing the entity's zero-based index in the seed batch.
    /// Useful for generating sequential or index-derived values (e.g. <c>i => $"User {i}"</c>).
    /// </summary>
    /// <remarks>Setting this clears any previously configured non-indexed factory.</remarks>
    public SeedBuilder<T> UseFactory(Func<int, TProperty> value)
    {
        _indexedValueFactory = value;
        return _parent;
    }

    /// <summary>
    /// Picks a random value from <paramref name="values"/> for each entity.
    /// </summary>
    /// <param name="values">The pool of values to pick from. Must contain at least one element.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="values"/> is null or empty.</exception>
    public SeedBuilder<T> UseFrom(params TProperty[] values)
    {
        if (values == null || values.Length == 0)
            throw new ArgumentException("Values collection cannot be null or empty.", nameof(values));
        var random = new Random();
        _valueFactory = () => values[random.Next(values.Length)];
        return _parent;
    }

    /// <summary>
    /// Picks a random value from <paramref name="values"/> for each entity.
    /// </summary>
    /// <param name="values">The pool of values to pick from. Must contain at least one element.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="values"/> is null or empty.</exception>
    public SeedBuilder<T> UseFrom(IEnumerable<TProperty> values)
    {
        if (values == null || !values.Any())
            throw new ArgumentException("Values collection cannot be null or empty.", nameof(values));
        var random = new Random();
        _valueFactory = () => values.ElementAt(random.Next(values.Count()));
        return _parent;
    }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Thrown when neither a value nor a factory has been configured for this rule.
    /// Ensure <see cref="UseValue"/>, <see cref="UseFactory(Func{TProperty})"/>, or <see cref="UseFrom(TProperty[])"/> was called.
    /// </exception>
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
