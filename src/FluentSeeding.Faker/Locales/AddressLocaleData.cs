using System.Text.Json.Serialization;

namespace FluentSeeding.Faker.Locales;

internal sealed class AddressLocaleData
{
    /// <summary>
    /// Road type designators (e.g. "Street", "Avenue", "Rua", "Avenida").
    /// Position in a full address is determined by <see cref="StreetAddressFormat"/>.
    /// </summary>
    [JsonPropertyName("street_suffix")]
    public string[] StreetSuffix { get; init; } = null!;

    /// <summary>Name portion of a street, without the road type (e.g. "Oak", "das Flores").</summary>
    [JsonPropertyName("street_name")]
    public string[] StreetName { get; init; } = null!;

    /// <summary>City names for this locale.</summary>
    [JsonPropertyName("city")]
    public string[] City { get; init; } = null!;

    /// <summary>Full state / province / region names.</summary>
    [JsonPropertyName("state")]
    public string[] State { get; init; } = null!;

    /// <summary>Abbreviated state / province codes, aligned with <see cref="State"/> by index.</summary>
    [JsonPropertyName("state_abbreviation")]
    public string[] StateAbbreviation { get; init; } = null!;

    /// <summary>
    /// Format templates for a street line. Tokens: <c>{number}</c>, <c>{name}</c>, <c>{suffix}</c>.
    /// Example: <c>"{number} {name} {suffix}"</c> or <c>"{suffix} {name}, {number}"</c>.
    /// </summary>
    [JsonPropertyName("street_address_format")]
    public string[] StreetAddressFormat { get; init; } = null!;

    /// <summary>
    /// Building number format templates. <c>#</c> is replaced with a random digit.
    /// Example: <c>"###"</c>, <c>"####"</c>.
    /// </summary>
    [JsonPropertyName("building_number_format")]
    public string[] BuildingNumberFormat { get; init; } = null!;

    /// <summary>
    /// Secondary address format templates (apartment, suite, floor). <c>#</c> → random digit.
    /// Example: <c>"Apt. ###"</c>, <c>"Suite ###"</c>.
    /// </summary>
    [JsonPropertyName("secondary_address_format")]
    public string[] SecondaryAddressFormat { get; init; } = null!;

    /// <summary>
    /// Postal / ZIP code format templates. <c>#</c> → random digit.
    /// Example: <c>"#####"</c>, <c>"#####-####"</c>, <c>"#####-###"</c>.
    /// </summary>
    [JsonPropertyName("zip_code_format")]
    public string[] ZipCodeFormat { get; init; } = null!;

    /// <summary>Country names in this locale's language (e.g. English or Brazilian Portuguese).</summary>
    [JsonPropertyName("countries")]
    public string[] Countries { get; init; } = null!;
}
