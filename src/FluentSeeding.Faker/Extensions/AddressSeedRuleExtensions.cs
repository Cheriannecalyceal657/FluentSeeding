namespace FluentSeeding.Faker.Extensions;

public static class AddressSeedRuleExtensions
{
    /// <summary>
    /// Generates a random road type designator drawn from the builder's locale
    /// (e.g. <c>"Street"</c>, <c>"Avenue"</c>, <c>"Rua"</c>, <c>"Avenida"</c>).
    /// </summary>
    public static SeedBuilder<T> UseStreetSuffix<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFrom(FluentFaker.Locale(rule.Parent.GetLocale()).Address.StreetSuffix);
    }

    /// <summary>
    /// Generates a random street name without a road type designator
    /// (e.g. <c>"Oak"</c>, <c>"das Flores"</c>).
    /// </summary>
    public static SeedBuilder<T> UseStreetName<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFrom(FluentFaker.Locale(rule.Parent.GetLocale()).Address.StreetName);
    }

    /// <summary>
    /// Generates a random building number
    /// (e.g. <c>"742"</c>, <c>"1204"</c>).
    /// </summary>
    public static SeedBuilder<T> UseBuildingNumber<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFactory(() =>
        {
            var format = FluentFaker.Locale(rule.Parent.GetLocale()).Address.BuildingNumberFormat.Pick();
            return format.FillFormat();
        });
    }

    /// <summary>
    /// Generates a random full street line combining a building number, street name, and road type
    /// according to the locale's format
    /// (e.g. <c>"742 Oak Street"</c> or <c>"Rua das Flores, 45"</c>).
    /// </summary>
    public static SeedBuilder<T> UseStreetAddress<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFactory(() => BuildStreetAddress(FluentFaker.Locale(rule.Parent.GetLocale())));
    }

    /// <summary>
    /// Generates a random secondary address designator
    /// (e.g. <c>"Apt. 42"</c>, <c>"Suite 100"</c>, <c>"Apto. 17"</c>).
    /// </summary>
    public static SeedBuilder<T> UseSecondaryAddress<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFactory(() =>
        {
            var format = FluentFaker.Locale(rule.Parent.GetLocale()).Address.SecondaryAddressFormat.Pick();
            return format.FillFormat();
        });
    }

    /// <summary>
    /// Generates a random city name drawn from the builder's locale
    /// (e.g. <c>"Portland"</c>, <c>"São Paulo"</c>).
    /// </summary>
    public static SeedBuilder<T> UseCity<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFrom(FluentFaker.Locale(rule.Parent.GetLocale()).Address.City);
    }

    /// <summary>
    /// Generates a random full state or province name drawn from the builder's locale
    /// (e.g. <c>"Oregon"</c>, <c>"Minas Gerais"</c>).
    /// </summary>
    public static SeedBuilder<T> UseState<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFrom(FluentFaker.Locale(rule.Parent.GetLocale()).Address.State);
    }

    /// <summary>
    /// Generates a random state or province abbreviation drawn from the builder's locale
    /// (e.g. <c>"OR"</c>, <c>"MG"</c>).
    /// </summary>
    public static SeedBuilder<T> UseStateAbbreviation<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFrom(FluentFaker.Locale(rule.Parent.GetLocale()).Address.StateAbbreviation);
    }

    /// <summary>
    /// Generates a random postal or ZIP code in the format defined by the locale
    /// (e.g. <c>"97201"</c>, <c>"97201-4823"</c>, <c>"01310-100"</c>).
    /// </summary>
    public static SeedBuilder<T> UseZipCode<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFactory(() =>
        {
            var format = FluentFaker.Locale(rule.Parent.GetLocale()).Address.ZipCodeFormat.Pick();
            return format.FillFormat();
        });
    }

    /// <summary>
    /// Generates a random country name in the builder's locale language
    /// (e.g. <c>"Germany"</c>, <c>"Alemanha"</c>).
    /// </summary>
    public static SeedBuilder<T> UseCountry<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFrom(FluentFaker.Locale(rule.Parent.GetLocale()).Address.City);
    }

    /// <summary>
    /// Generates a complete address combining a street line, city, state abbreviation, and postal code
    /// (e.g. <c>"742 Oak Street, Portland, OR 97201"</c> or
    /// <c>"Rua das Flores, 45, São Paulo, SP 01310-100"</c>).
    /// </summary>
    public static SeedBuilder<T> UseFullAddress<T>(this SeedRule<T, string> rule) where T : class
    {
        return rule.UseFactory(() =>
        {
            var data = FluentFaker.Locale(rule.Parent.GetLocale());
            var street = BuildStreetAddress(data);
            var city = data.Address.City.Pick();
            var stateAbbr = data.Address.StateAbbreviation.Pick();
            var zip = data.Address.ZipCodeFormat.Pick().FillFormat();
            return $"{street}, {city}, {stateAbbr} {zip}";
        });
    }

    private static string BuildStreetAddress(FluentSeeding.Faker.Locales.LocaleData data)
    {
        var address = data.Address;
        var number = address.BuildingNumberFormat.Pick().FillFormat();
        var name = address.StreetName.Pick();
        var suffix = address.StreetSuffix.Pick();
        var format = address.StreetAddressFormat.Pick();
        return format
            .Replace("{number}", number)
            .Replace("{name}", name)
            .Replace("{suffix}", suffix);
    }
}
