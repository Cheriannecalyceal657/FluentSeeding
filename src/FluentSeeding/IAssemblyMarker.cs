using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FluentSeeding.Tests")]
namespace FluentSeeding;

/// <summary>
/// Marker interface for referencing this assembly during DI registration or assembly scanning.
/// </summary>
public interface IAssemblyMarker;
