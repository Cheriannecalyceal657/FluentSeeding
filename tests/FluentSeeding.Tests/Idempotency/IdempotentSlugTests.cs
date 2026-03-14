using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.Idempotency;

[TestFixture(TestName = "Idempotent.Slug")]
[Category("Unit")]
[Category(nameof(Idempotent))]
public sealed class IdempotentSlugTests
{
    private sealed class ProductCategory { }

    [Test]
    public void Slug_WhenCalledWithSingleWordTypeName_ProducesLowercasedKebabSlug()
    {
        // Arrange / Act
        var slug = Idempotent.Slug<User>(0);

        // Assert
        slug.Should().Be("user-0");
    }

    [Test]
    public void Slug_WhenCalledWithPascalCaseTypeName_ConvertsPascalToKebab()
    {
        // Arrange / Act
        var slug = Idempotent.Slug<ProductCategory>(0);

        // Assert
        slug.Should().Be("product-category-0");
    }

    [Test]
    public void Slug_WhenCalledWithCustomPrefix_UsesCustomPrefixInsteadOfTypeName()
    {
        // Arrange / Act
        var slug = Idempotent.Slug<User>(0, "my-prefix");

        // Assert
        slug.Should().Be("my-prefix-0");
    }

    [Test]
    public void Slug_WhenCalledWithDifferentIndex_ReturnsDifferentValue()
    {
        // Arrange / Act
        var first = Idempotent.Slug<User>(0);
        var second = Idempotent.Slug<User>(1);

        // Assert
        first.Should().NotBe(second);
    }

    [Test]
    public void Slug_WhenCalledTwiceWithSameInputs_ReturnsSameValue()
    {
        // Arrange / Act
        var first = Idempotent.Slug<User>(0);
        var second = Idempotent.Slug<User>(0);

        // Assert
        first.Should().Be(second);
    }

    [Test]
    public void Slug_GenericAndNonGenericOverloads_ReturnSameValue()
    {
        // Arrange / Act
        var generic = Idempotent.Slug<User>(0);
        var nonGeneric = Idempotent.Slug(typeof(User), 0);

        // Assert
        generic.Should().Be(nonGeneric);
    }
}
