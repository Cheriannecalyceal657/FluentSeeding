using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.SeedRule;

[TestFixture(TestName = "SeedRule.Apply")]
[Category("Unit")]
[Category(nameof(SeedRule<,>))]
public sealed class SeedRuleApplyTests : SeedRuleTest
{
    [Test]
    public void Apply_WhenConfigured_ShouldApplyValue()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        rule.UseValue("Test Name");
        var user = new User();
        
        // Act
        rule.Apply(user);
        
        // Assert
        user.Name.Should().Be("Test Name");
    }

    [Test]
    public void Apply_WhenNotConfigured_ShouldThrow()
    {
        // Arrange
        var rule = CreateRule(u => u.Name);
        var user = new User();

        // Act
        Action act = () => rule.Apply(user);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
