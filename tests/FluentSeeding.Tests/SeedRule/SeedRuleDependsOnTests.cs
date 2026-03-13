using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.SeedRule;

[TestFixture(TestName = "SeedRule.DependsOn")]
[Category("Unit")]
[Category(nameof(SeedRule<,>))]
public sealed class SeedRuleDependsOnTests : SeedRuleTest
{
    [Test]
    public void DependsOn_WithDirectSelector_AddsDependencyPropertyName()
    {
        // Arrange
        var rule = CreateRule(u => u.Email);

        // Act
        rule.DependsOn(u => u.Name).UseValue("test@example.com");

        // Assert
        rule.Dependencies.Should().Contain("Name");
    }

    [Test]
    public void DependsOn_CalledMultipleTimes_AccumulatesAllDependencies()
    {
        // Arrange
        var rule = CreateRule(u => u.Email);

        // Act
        rule.DependsOn(u => u.Name).DependsOn(u => u.Id).UseValue("test@example.com");

        // Assert
        rule.Dependencies.Should().BeEquivalentTo(new[] { "Name", "Id" });
    }

    [Test]
    public void DependsOn_WithSamePropertyTwice_DoesNotDuplicateDependency()
    {
        // Arrange
        var rule = CreateRule(u => u.Email);

        // Act
        rule.DependsOn(u => u.Name).DependsOn(u => u.Name).UseValue("test@example.com");

        // Assert
        rule.Dependencies.Should().ContainSingle(d => d == "Name");
    }

    [Test]
    public void DependsOn_WithNestedPropertyAccess_ThrowsArgumentException()
    {
        // Arrange
        var rule = CreateRule(u => u.Email);

        // Act
        Action act = () => rule.DependsOn(u => u.Name.Length).UseValue("test@example.com");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("dependency");
    }

    [Test]
    public void DependsOn_ReturnsSameRuleInstance_AllowsChaining()
    {
        // Arrange
        var rule = CreateRule(u => u.Email);

        // Act
        var result = rule.DependsOn(u => u.Name);

        // Assert
        result.Should().BeSameAs(rule);
    }

    [Test]
    public void DependsOn_WhenNoDependenciesDeclared_DependenciesCollectionIsEmpty()
    {
        // Arrange & Act
        var rule = CreateRule(u => u.Name);
        rule.UseValue("Alice");

        // Assert
        rule.Dependencies.Should().BeEmpty();
    }
}
