using FluentAssertions;

namespace FluentSeeding.Tests.SeedBuilder;

[TestFixture(TestName = "SeedBuilder.Count")]
[Category("Unit")]
[Category(nameof(SeedBuilder<>))]
public sealed class SeedBuilderCountTests
{
    private class Model
    {
        public string Value { get; set; } = string.Empty;
    }

    [Test]
    public void Count_WithFixedValue_BuildsExactCount()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.Count(5);

        // Act
        var result = builder.Build().ToList();

        // Assert
        result.Should().HaveCount(5);
    }

    [Test]
    public void Count_WithZero_BuildsNoEntities()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.Count(0);

        // Act
        var result = builder.Build().ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Count_WithRange_WhenMinEqualsMax_BuildsExactCount()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.Count(7, 7);

        // Act
        var result = builder.Build().ToList();

        // Assert
        result.Should().HaveCount(7);
    }

    [Test]
    public void Count_WithRange_BuildsCountWithinBounds()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.Count(3, 8);

        // Act
        for (int i = 0; i < 50; i++)
        {
            var count = builder.Build().Count();
            count.Should().BeInRange(3, 8);
        }
    }

    [Test]
    public void Count_WithRange_WhenMinIsZero_AllowsEmptyResult()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.Count(0, 0);

        // Act
        var result = builder.Build().ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Count_Range_WhenMinIsNegative_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();

        // Act
        Action act = () => builder.Count(-1, 5);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("min");
    }

    [Test]
    public void Count_Range_WhenMaxIsLessThanMin_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();

        // Act
        Action act = () => builder.Count(5, 3);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("max");
    }

    [Test]
    public void Count_Fixed_ReturnsBuilderForChaining()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();

        // Act
        var returned = builder.Count(3);

        // Assert
        returned.Should().BeSameAs(builder);
    }

    [Test]
    public void Count_Range_ReturnsBuilderForChaining()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();

        // Act
        var returned = builder.Count(1, 3);

        // Assert
        returned.Should().BeSameAs(builder);
    }
}
