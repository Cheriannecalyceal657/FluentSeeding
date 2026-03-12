using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.SeedRule;

[TestFixture(TestName = "SeedRule.UseFactory")]
[Category("Unit")]
[Category(nameof(SeedRule<,>))]
public sealed class SeedRuleUseFactoryTests : SeedRuleTest
{
    [Test]
    public void UseFactory_WhenCalled_ReturnsSameRuleForChaining()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);

        // Act
        var returned = rule.UseFactory(() => "Test");

        // Assert
        returned.Should().BeSameAs(rule);
    }

    [Test]
    public void UseFactory_WhenApplied_SetsPropertyToFactoryResult()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseFactory(() => "Generated");
        var user = new User();

        // Act
        rule.Apply(user);

        // Assert
        user.Name.Should().Be("Generated");
    }

    [Test]
    public void UseFactory_WhenAppliedMultipleTimes_InvokesFactoryEachTime()
    {
        // Arrange
        var callCount = 0;
        var rule = CreateRule(u => u.Name);
        rule.UseFactory(() =>
        {
            callCount++;
            return $"Call{callCount}";
        });

        var user1 = new User();
        var user2 = new User();

        // Act
        rule.Apply(user1);
        rule.Apply(user2);

        // Assert
        callCount.Should().Be(2);
        user1.Name.Should().Be("Call1");
        user2.Name.Should().Be("Call2");
    }

    [Test]
    public void UseFactory_WhenCalledMultipleTimes_UsesLastFactory()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseFactory(() => "First");
        rule.UseFactory(() => "Second");
        var user = new User();

        // Act
        rule.Apply(user);

        // Assert
        user.Name.Should().Be("Second");
    }

    [Test]
    public void UseFactory_WithValueTypeProperty_SetsPropertyCorrectly()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var rule = CreateRule(u => u.Id);
        rule.UseFactory(() => expectedId);
        var user = new User();

        // Act
        rule.Apply(user);

        // Assert
        user.Id.Should().Be(expectedId);
    }

    [Test]
    public void UseFactory_WithIndex_WhenCalled_ReturnsSameRuleForChaining()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);

        // Act
        var returned = rule.UseFactory(i => $"User{i}");

        // Assert
        returned.Should().BeSameAs(rule);
    }

    [Test]
    public void UseFactory_WithIndex_WhenApplied_PassesIndexToFactory()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseFactory(i => $"User{i}");
        var user = new User();

        // Act
        rule.Apply(user, 5);

        // Assert
        user.Name.Should().Be("User5");
    }

    [Test]
    public void UseFactory_WithIndex_WhenAppliedMultipleTimes_PassesCorrectIndexEachTime()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseFactory(i => $"User{i}");

        var user0 = new User();
        var user1 = new User();
        var user2 = new User();

        // Act
        rule.Apply(user0, 0);
        rule.Apply(user1, 1);
        rule.Apply(user2, 2);

        // Assert
        user0.Name.Should().Be("User0");
        user1.Name.Should().Be("User1");
        user2.Name.Should().Be("User2");
    }

    [Test]
    public void UseFactory_WithIndex_WhenCalledMultipleTimes_UsesLastFactory()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseFactory(i => $"First{i}");
        rule.UseFactory(i => $"Second{i}");
        var user = new User();

        // Act
        rule.Apply(user, 3);

        // Assert
        user.Name.Should().Be("Second3");
    }

    [Test]
    public void UseFactory_WithIndex_AfterUseFactory_OverridesPreviousFactory()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseFactory(() => "NoIndex");
        rule.UseFactory(i => $"Index{i}");
        var user = new User();

        // Act
        rule.Apply(user, 7);

        // Assert
        user.Name.Should().Be("Index7");
    }

    [Test]
    public void UseFactory_AfterUseFactoryWithIndex_OverridesPreviousFactory()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseFactory(i => $"Index{i}");
        rule.UseFactory(() => "NoIndex");
        var user = new User();

        // Act
        rule.Apply(user, 7);

        // Assert
        user.Name.Should().Be("NoIndex");
    }
}
