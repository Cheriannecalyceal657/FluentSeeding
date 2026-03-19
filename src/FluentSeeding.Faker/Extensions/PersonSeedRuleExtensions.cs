using FluentSeeding.Faker.Locales;

namespace FluentSeeding.Faker.Extensions;

public static class PersonSeedRuleExtensions
{
    public static SeedBuilder<T> UseFirstName<T>(this SeedRule<T, string> rule, Gender gender = Gender.Any, string? locale = null) where T : class
    {
        return rule.UseFrom(FluentFaker.Locale(locale).Person.FirstName.GetForGender(gender));
    }
    
    public static SeedBuilder<T> UseLastName<T>(this SeedRule<T, string> rule, string? locale = null) where T : class
    {
        return rule.UseFrom(FluentFaker.Locale(locale).Person.LastName);
    }

    public static SeedBuilder<T> UseFullName<T>(this SeedRule<T, string> rule, Gender gender = Gender.Any,
        string? locale = null) where T : class
    {
        return rule.UseFactory(() =>
        {
            var data = FluentFaker.Locale(locale);
            var firstName = data.Person.FirstName.GetForGender(gender).Pick();
            var lastNames = data.Person.LastName.PickMany(Random.Shared.Next(1, 3)).ToArray();
            return $"{firstName} {string.Join(" ", lastNames)}";
        });
    }
    
}
