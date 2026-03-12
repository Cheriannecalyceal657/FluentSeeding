using FluentAssertions;

namespace FluentSeeding.Tests.SeedBuilder;

[TestFixture(TestName = "SeedBuilder.RuleFor")]
[Category("Unit")]
[Category(nameof(SeedBuilder<>))]
public sealed class SeedBuilderRuleForTests
{
    private class Model
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    [Test]
    public void RuleFor_WhenCalled_ReturnsConfigurableRule()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();

        // Act
        var rule = builder.RuleFor(m => m.Name);

        // Assert
        rule.Should().NotBeNull();
    }

    [Test]
    public void RuleFor_WhenRuleConfigured_AppliesValueDuringBuild()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.Count(3)
            .RuleFor(m => m.Name).UseValue("John");

        // Act
        var result = builder.Build().ToList();

        // Assert
        result.Should().OnlyContain(m => m.Name == "John");
    }

    [Test]
    public void RuleFor_WhenMultipleRulesConfigured_AllAreApplied()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.Count(2);
        builder.RuleFor(m => m.Name).UseValue("Jane");
        builder.RuleFor(m => m.Age).UseValue(30);

        // Act
        var result = builder.Build().ToList();

        // Assert
        result.Should().OnlyContain(m => m.Name == "Jane" && m.Age == 30);
    }

    [Test]
    public void RuleFor_WhenRuleNotConfigured_BuildThrowsOnApply()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.RuleFor(m => m.Name); // rule added but no UseValue/UseFrom

        // Act
        Action act = () => builder.Build().ToList();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
