using System.Text.Json.Serialization;

namespace FluentSeeding.Faker.Locales;

public sealed class PersonLocaleData
{
    [JsonPropertyName("first_name")] 
    public GenderedArrays FirstName { get; init; } = null!;

    [JsonPropertyName("last_name")] 
    public string[] LastName { get; init; } = null!;

    [JsonPropertyName("occupation")] 
    public string[] Occupations { get; init; } = null!;
}