using FluentAssertions;

namespace FluentSeeding.Tests.SeedBuilder;

[TestFixture(TestName = "SeedBuilder.TopologicalSort")]
[Category("Unit")]
[Category(nameof(SeedBuilder<>))]
public sealed class SeedBuilderTopologicalSortTests
{
    [Test]
    public void Build_WhenDependencyRuleRegisteredAfterDependent_ShouldRunDependencyFirst()
    {
        // Arrange - Derived is registered first but depends on Flag
        var builder = new SeedBuilder<Model>();
        builder.RuleFor(m => m.Derived)
            .DependsOn(m => m.Flag)
            .When(m => m.Flag == "set")
            .UseValue("applied");
        builder.RuleFor(m => m.Flag)
            .UseValue("set");

        // Act
        var model = builder.Build().Single();

        // Assert - if Flag ran first, When condition is true and Derived is "applied"
        model.Flag.Should().Be("set");
        model.Derived.Should().Be("applied");
    }

    [Test]
    public void Build_WithChainedDependencies_ShouldRunRulesInTopologicalOrder()
    {
        // Arrange - registration order: SecondDerived -> Derived -> Flag
        // required order: Flag -> Derived -> SecondDerived
        var builder = new SeedBuilder<Model>();
        builder.RuleFor(m => m.SecondDerived)
            .DependsOn(m => m.Derived)
            .When(m => m.Derived == "applied")
            .UseValue("chain complete");
        builder.RuleFor(m => m.Derived)
            .DependsOn(m => m.Flag)
            .When(m => m.Flag == "set")
            .UseValue("applied");
        builder.RuleFor(m => m.Flag)
            .UseValue("set");

        // Act
        var model = builder.Build().Single();

        // Assert
        model.Flag.Should().Be("set");
        model.Derived.Should().Be("applied");
        model.SecondDerived.Should().Be("chain complete");
    }

    [Test]
    public void Build_WithNoDependencies_ShouldBuildAllRulesSuccessfully()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.RuleFor(m => m.Flag).UseValue("a");
        builder.RuleFor(m => m.Derived).UseValue("b");

        // Act
        var model = builder.Build().Single();

        // Assert
        model.Flag.Should().Be("a");
        model.Derived.Should().Be("b");
    }

    [Test]
    public void Build_WithDependencyOnPropertyWithNoRule_ShouldNotThrow()
    {
        // Arrange - Derived depends on SecondDerived, which has no rule
        var builder = new SeedBuilder<Model>();
        builder.RuleFor(m => m.Derived)
            .DependsOn(m => m.SecondDerived)
            .UseValue("applied");

        // Act
        Action act = () => builder.Build();

        // Assert
        act.Should().NotThrow();
    }

    [Test]
    public void Build_WithDependencyOnPropertyWithNoRule_ShouldStillApplyDependentRule()
    {
        // Arrange - Derived depends on SecondDerived, which has no rule 
        var builder = new SeedBuilder<Model>();
        builder.RuleFor(m => m.Derived)
            .DependsOn(m => m.SecondDerived)
            .UseValue("applied");

        // Act
        var model = builder.Build().Single();

        // Assert
        model.Derived.Should().Be("applied");
    }

    [Test]
    public void Build_WithCircularDependency_ShouldThrowInvalidOperationException()
    {
        // Arrange - Flag depends on Derived, Derived depends on Flag
        var builder = new SeedBuilder<Model>();
        builder.RuleFor(m => m.Flag)
            .DependsOn(m => m.Derived)
            .UseValue("flag");
        builder.RuleFor(m => m.Derived)
            .DependsOn(m => m.Flag)
            .UseValue("derived");

        // Act
        Action act = () => builder.Build();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*circular*");
    }

    [Test]
    public void Build_WithMultipleRulesForSameProperty_ShouldRespectDependencies()
    {
        // Arrange - two rules for Flag, Derived depends on both
        var executionOrder = new List<string>();
        var builder = new SeedBuilder<Model>();
        builder.RuleFor(m => m.Derived)
            .DependsOn(m => m.Flag)
            .UseFactory(() =>
            {
                executionOrder.Add("Derived");
                return "derived";
            });
        builder.RuleFor(m => m.Flag)
            .UseFactory(() =>
            {
                executionOrder.Add("Flag1");
                return "first";
            });
        builder.RuleFor(m => m.Flag)
            .UseFactory(() =>
            {
                executionOrder.Add("Flag2");
                return "second";
            });

        // Act
        builder.Build().Single();

        // Assert - both Flag rules must precede the Derived rule
        executionOrder.IndexOf("Derived").Should().BeGreaterThan(executionOrder.IndexOf("Flag1"));
        executionOrder.IndexOf("Derived").Should().BeGreaterThan(executionOrder.IndexOf("Flag2"));
    }

    private class Model
    {
        public string? Flag { get; set; }
        public string? Derived { get; set; }
        public string? SecondDerived { get; set; }
    }
}
