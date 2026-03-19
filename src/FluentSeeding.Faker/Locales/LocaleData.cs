using System.Text.Json.Serialization;

namespace FluentSeeding.Faker.Locales;

internal sealed record LocaleData
{
    [JsonPropertyName("person")]
    public PersonLocaleData Person { get; init; } = null!;
}
