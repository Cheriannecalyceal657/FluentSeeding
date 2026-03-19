using FluentAssertions;
using FluentSeeding.Faker.Extensions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Faker.Tests;

[TestFixture(TestName = "PersonSeedRuleExtensions")]
[Category("Unit")]
[Category(nameof(PersonSeedRuleExtensions))]
public sealed class PersonSeedRuleExtensionsTests
{
    private static SeedRule<User, string> CreateStringRule()
        => new SeedBuilder<User>().RuleFor(u => u.Name);

    private static string ApplyOnce(SeedRule<User, string> rule)
    {
        var user = new User();
        rule.Apply(user);
        return user.Name;
    }

    [Test]
    public void UseFirstName_WithDefaultSettings_SetsNonEmptyName()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseFirstName();

        // Act & Assert
        ApplyOnce(rule).Should().NotBeNullOrEmpty();
    }

    [Test]
    public void UseFirstName_WithGenderMale_SetsNonEmptyName()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseFirstName(Gender.Male);

        // Act & Assert
        ApplyOnce(rule).Should().NotBeNullOrEmpty();
    }

    [Test]
    public void UseFirstName_WithGenderFemale_SetsNonEmptyName()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseFirstName(Gender.Female);

        // Act & Assert
        ApplyOnce(rule).Should().NotBeNullOrEmpty();
    }

    [Test]
    public void UseFirstName_WithGenderAny_SetsNonEmptyName()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseFirstName(Gender.Any);

        // Act & Assert
        ApplyOnce(rule).Should().NotBeNullOrEmpty();
    }

    [Test]
    public void UseFirstName_WithExplicitLocale_SetsNonEmptyName()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseFirstName(locale: "en");

        // Act & Assert
        ApplyOnce(rule).Should().NotBeNullOrEmpty();
    }

    [Test]
    public void UseLastName_WithDefaultSettings_SetsNonEmptyName()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseLastName();

        // Act & Assert
        ApplyOnce(rule).Should().NotBeNullOrEmpty();
    }

    [Test]
    public void UseLastName_WithExplicitLocale_SetsNonEmptyName()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseLastName("en");

        // Act & Assert
        ApplyOnce(rule).Should().NotBeNullOrEmpty();
    }

    [Test]
    public void UseFullName_WithDefaultSettings_SetsNameContainingSpace()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseFullName();

        // Act & Assert
        ApplyOnce(rule).Should().Contain(" ");
    }

    [Test]
    public void UseFullName_WithGenderMale_SetsNameContainingSpace()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseFullName(Gender.Male);

        // Act & Assert
        ApplyOnce(rule).Should().Contain(" ");
    }

    [Test]
    public void UseFullName_WithGenderFemale_SetsNameContainingSpace()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseFullName(Gender.Female);

        // Act & Assert
        ApplyOnce(rule).Should().Contain(" ");
    }

    [Test]
    public void UseFullName_WithExplicitLocale_SetsNameContainingSpace()
    {
        // Arrange
        var rule = CreateStringRule();
        rule.UseFullName(locale: "en");

        // Act & Assert
        ApplyOnce(rule).Should().Contain(" ");
    }
}
