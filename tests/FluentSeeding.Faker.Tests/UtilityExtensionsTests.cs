using FluentAssertions;
using FluentSeeding.Faker.Extensions;
using FluentSeeding.Faker.Locales;

namespace FluentSeeding.Faker.Tests;

[TestFixture(TestName = "UtilityExtensions")]
[Category("Unit")]
public sealed class UtilityExtensionsTests
{
    private static readonly GenderedArrays _sample = new(
        Male: ["James", "John", "Robert"],
        Female: ["Mary", "Patricia", "Jennifer"]
    );

    [Test]
    public void GetForGender_WithMale_ReturnsMaleArray()
    {
        // Act
        var result = _sample.GetForGender(Gender.Male);

        // Assert
        result.Should().BeEquivalentTo(_sample.Male);
    }

    [Test]
    public void GetForGender_WithFemale_ReturnsFemaleArray()
    {
        // Act
        var result = _sample.GetForGender(Gender.Female);

        // Assert
        result.Should().BeEquivalentTo(_sample.Female);
    }

    [Test]
    public void GetForGender_WithAny_ReturnsBothArraysConcatenated()
    {
        // Act
        var result = _sample.GetForGender(Gender.Any);

        // Assert
        result.Should().BeEquivalentTo(_sample.Male.Concat(_sample.Female));
    }

    [Test]
    public void Pick_WithNonEmptyCollection_ReturnsElementFromCollection()
    {
        // Arrange
        string[] items = ["a", "b", "c"];

        // Act
        var result = items.Pick();

        // Assert
        items.Should().Contain(result);
    }

    [Test]
    public void Pick_WithSingleElement_AlwaysReturnsThatElement()
    {
        // Arrange
        string[] items = ["only"];

        // Act & Assert
        items.Pick().Should().Be("only");
    }

    [Test]
    public void Pick_WithEmptyCollection_ThrowsInvalidOperationException()
    {
        // Act
        Action act = () => Array.Empty<string>().Pick();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void PickMany_WithNonEmptyCollection_ReturnsExactRequestedCount()
    {
        // Arrange
        string[] items = ["a", "b", "c"];

        // Act
        var result = items.PickMany(5).ToList();

        // Assert
        result.Should().HaveCount(5);
    }

    [Test]
    public void PickMany_WithNonEmptyCollection_OnlyReturnsElementsFromSource()
    {
        // Arrange
        string[] items = ["a", "b", "c"];

        // Act
        var result = items.PickMany(10).ToList();

        // Assert
        result.Should().OnlyContain(item => items.Contains(item));
    }

    [Test]
    public void PickMany_WithZeroCount_ReturnsEmptySequence()
    {
        // Arrange
        string[] items = ["a", "b", "c"];

        // Act & Assert
        items.PickMany(0).Should().BeEmpty();
    }

    [Test]
    public void PickMany_WithEmptyCollection_ThrowsInvalidOperationException()
    {
        // Act
        Action act = () => Array.Empty<string>().PickMany(3).ToList();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
