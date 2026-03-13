using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.SeedRule;

[TestFixture(TestName = "SeedRule.When")]
[Category("Unit")]
[Category(nameof(SeedRule<,>))]
public sealed class SeedRuleWhenTests : SeedRuleTest
{
    [Test]
    public void When_ConditionIsTrue_ShouldApplyValue()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.When(_ => true).UseValue("Alice");
        var user = new User();

        // Act
        rule.Apply(user);

        // Assert
        user.Name.Should().Be("Alice");
    }

    [Test]
    public void When_ConditionIsFalse_ShouldNotSetValue()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.When(_ => false).UseValue("Alice");
        var user = new User { Name = "Original" };

        // Act
        rule.Apply(user);

        // Assert
        user.Name.Should().Be("Original");
    }

    [Test]
    public void When_ConditionReceivesEntityInstance_AppliesValueWhenPropertyMatches()
    {
        // Arrange
        var rule = CreateRule(u => u.Email);
        rule.When(u => u.Name == "Admin").UseValue("admin@example.com");
        var user = new User { Name = "Admin" };

        // Act
        rule.Apply(user);

        // Assert
        user.Email.Should().Be("admin@example.com");
    }

    [Test]
    public void When_ConditionReceivesEntityInstance_SkipsValueWhenPropertyDoesNotMatch()
    {
        // Arrange
        var rule = CreateRule(u => u.Email);
        rule.When(u => u.Name == "Admin").UseValue("admin@example.com");
        var user = new User { Name = "Regular", Email = "user@example.com" };

        // Act
        rule.Apply(user);

        // Assert
        user.Email.Should().Be("user@example.com");
    }

    [Test]
    public void When_CalledMultipleTimes_UsesLastCondition()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.When(_ => false).When(_ => true).UseValue("Bob");
        var user = new User();

        // Act
        rule.Apply(user);

        // Assert
        user.Name.Should().Be("Bob");
    }

    [Test]
    public void When_CombinedWithUseFactory_SkipsFactoryWhenConditionIsFalse()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.When(_ => false).UseFactory(() => Guid.NewGuid().ToString());
        var user = new User { Name = "Unchanged" };

        // Act
        rule.Apply(user);

        // Assert
        user.Name.Should().Be("Unchanged");
    }

    [Test]
    public void When_CombinedWithIndexedFactory_SkipsFactoryWhenConditionIsFalse()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.When(_ => false).UseFactory(i => $"User {i}");
        var user = new User { Name = "Unchanged" };

        // Act
        rule.Apply(user, 5);

        // Assert
        user.Name.Should().Be("Unchanged");
    }
}
