using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.SeedRule;

[TestFixture(TestName = "SeedRule.UseValue")]
[Category("Unit")]
[Category(nameof(SeedRule<,>))]
public sealed class SeedRuleUseValueTests : SeedRuleTest
{
    [Test]
    public void UseValue_WhenCalled_ReturnsSameRuleForChaining()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);

        // Act
        var returned = rule.UseValue("Test");

        // Assert
        returned.Should().BeSameAs(rule);
    }

    [Test]
    public void UseValue_WhenCalledMultipleTimes_UsesLastValue()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseValue("First");
        rule.UseValue("Second");
        var user = new User();

        // Act
        rule.Apply(user);

        // Assert
        user.Name.Should().Be("Second");
    }

    [Test]
    public void UseValue_WithNullValue_SetsPropertyToNull()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseValue(null!);
        var user = new User { Name = "Existing" };

        // Act
        rule.Apply(user);

        // Assert
        user.Name.Should().BeNull();
    }

    [Test]
    public void UseValue_WithValueType_SetsPropertyCorrectly()
    {
        // Arrange
        var rule = CreateRule(u => u.Id);
        var id = Guid.NewGuid();
        rule.UseValue(id);
        var user = new User();

        // Act
        rule.Apply(user);

        // Assert
        user.Id.Should().Be(id);
    }
}
