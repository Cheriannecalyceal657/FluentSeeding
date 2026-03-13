namespace FluentSeeding;

/// <summary>
/// Represents a rule that populates a property on an instance of <typeparamref name="T"/> during seeding.
/// </summary>
/// <typeparam name="T">The entity type this rule targets.</typeparam>
public interface ISeedRule<in T>
{
    /// <summary>
    /// The name of the property this rule targets. Used for dependency resolution.
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    /// The names of properties this rule must run after. Used by the topological sorter.
    /// </summary>
    IReadOnlyCollection<string> Dependencies { get; }

    /// <summary>
    /// Applies the rule to the given entity instance, setting the target property.
    /// </summary>
    /// <param name="instance">The entity instance to modify.</param>
    /// <param name="index">The zero-based index of this instance in the current seed batch. Used by index-aware factories.</param>
    void Apply(T instance, int index = 0);
}