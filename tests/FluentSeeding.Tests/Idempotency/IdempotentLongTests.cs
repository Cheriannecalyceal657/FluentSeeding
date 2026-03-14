using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.Idempotency;

[TestFixture(TestName = "Idempotent.Long")]
[Category("Unit")]
[Category(nameof(Idempotent))]
public sealed class IdempotentLongTests
{
    [Test]
    public void Long_WhenCalledTwiceWithSameInputs_ReturnsSameValue()
    {
        // Arrange / Act
        var first = Idempotent.Long<User>(0);
        var second = Idempotent.Long<User>(0);

        // Assert
        first.Should().Be(second);
    }

    [Test]
    public void Long_WhenCalledWithDifferentIndex_ReturnsDifferentValue()
    {
        // Arrange / Act
        var first = Idempotent.Long<User>(0);
        var second = Idempotent.Long<User>(1);

        // Assert
        first.Should().NotBe(second);
    }

    [Test]
    public void Long_WhenCalledWithDifferentSeed_ReturnsDifferentValue()
    {
        // Arrange / Act
        var withDefaultSeed = Idempotent.Long<User>(0);
        var withCustomSeed = Idempotent.Long<User>(0, "custom");

        // Assert
        withDefaultSeed.Should().NotBe(withCustomSeed);
    }

    [Test]
    public void Long_WhenCalledWithNullSeed_BehavesSameAsOmittingSeed()
    {
        // Arrange / Act
        var withNullSeed = Idempotent.Long<User>(0, null);
        var withNoSeed = Idempotent.Long<User>(0);

        // Assert
        withNullSeed.Should().Be(withNoSeed);
    }

    [Test]
    public void Long_GenericAndNonGenericOverloads_ReturnSameValue()
    {
        // Arrange / Act
        var generic = Idempotent.Long<User>(0);
        var nonGeneric = Idempotent.Long(typeof(User), 0);

        // Assert
        generic.Should().Be(nonGeneric);
    }
}
