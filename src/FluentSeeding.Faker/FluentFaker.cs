using System.Runtime.CompilerServices;
using FluentSeeding.Faker.Locales;

[assembly: InternalsVisibleTo("FluentSeeding.Faker.Tests")]

namespace FluentSeeding.Faker;

/// <summary>
/// Static entry point for locale-aware fake data generation.
/// Set <see cref="DefaultLocale"/> once at startup, or pass a locale per call.
/// </summary>
internal static class FluentFaker
{
    /// <summary>
    /// Gets or sets the default locale used when no locale is specified in a generator call.
    /// Defaults to <c>"en"</c>.
    /// </summary>
    /// <remarks>
    /// Unknown locales fall back to their language root, then to <c>"en"</c>.
    /// </remarks>
    public static string DefaultLocale { get; set; } = "en";

    public static LocaleData Locale(string? locale) => LocaleLoader.Load(locale ?? DefaultLocale);
}
