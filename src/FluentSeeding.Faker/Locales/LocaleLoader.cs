using System.Collections.Concurrent;
using System.Text.Json;

namespace FluentSeeding.Faker.Locales;

internal static class LocaleLoader
{
    private static readonly ConcurrentDictionary<string, LocaleData> _cache = new();
    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    internal static LocaleData Load(string locale)
        => _cache.GetOrAdd(locale, ResolveLocale);

    private static LocaleData ResolveLocale(string locale)
    {
        var data = TryLoad(locale);
        if (data is not null)
            return data;

        var dashIndex = locale.IndexOf('-');
        if (dashIndex > 0)
        {
            data = TryLoad(locale[..dashIndex]);
            if (data is not null)
                return data;
        }

        return TryLoad("en")
            ?? throw new InvalidOperationException("Failed to load the default locale 'en'. The embedded resource may be missing.");
    }

    private static LocaleData? TryLoad(string locale)
    {
        var assembly = typeof(LocaleLoader).Assembly;
        var resourceName = $"FluentSeeding.Faker.Locales.Data.{locale}.json";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream is null)
            return null;
        return JsonSerializer.Deserialize<LocaleData>(stream, _options);
    }
}
