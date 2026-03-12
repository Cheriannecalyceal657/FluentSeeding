using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.SeedRule;

[TestFixture(TestName = "SeedRule.UseFrom")]
[Category("Unit")]
[Category(nameof(SeedRule<,>))]
public sealed class SeedRuleUseFromTests : SeedRuleTest
{
    [Test]
    public void UseFrom_WithSingleValue_AlwaysSetsExactValue()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseFrom("Only");

        // Act & Assert — run several times to be confident it always picks the one value
        for (int i = 0; i < 20; i++)
        {
            var user = new User();
            rule.Apply(user);
            user.Name.Should().Be("Only");
        }
    }

    [Test]
    public void UseFrom_WithMultipleValues_SetsValueFromProvidedSet()
    {
        // Arrange
        var values = new[] { "Alice", "Bob", "Charlie" };
        var rule = CreateRule(u => u.Name);
        rule.UseFrom(values);

        // Act
        var results = Enumerable.Range(0, 50)
            .Select(_ =>
            {
                var user = new User();
                rule.Apply(user);
                return user.Name;
            })
            .ToList();

        // Assert
        results.Should().OnlyContain(name => values.Contains(name));
    }

    [Test]
    public void UseFrom_WhenCalledMultipleTimes_UsesLastCall()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseFrom("First");
        rule.UseFrom("Second");
        var user = new User();

        // Act
        rule.Apply(user);

        // Assert
        user.Name.Should().Be("Second");
    }
}
