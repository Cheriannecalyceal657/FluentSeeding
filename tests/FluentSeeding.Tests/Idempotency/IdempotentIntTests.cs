using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.Idempotency;

[TestFixture(TestName = "Idempotent.Int")]
[Category("Unit")]
[Category(nameof(Idempotent))]
public sealed class IdempotentIntTests
{
    [Test]
    public void Int_WhenCalledTwiceWithSameInputs_ReturnsSameValue()
    {
        // Arrange / Act
        var first = Idempotent.Int<User>(0);
        var second = Idempotent.Int<User>(0);

        // Assert
        first.Should().Be(second);
    }

    [Test]
    public void Int_WhenCalledWithDifferentIndex_ReturnsDifferentValue()
    {
        // Arrange / Act
        var first = Idempotent.Int<User>(0);
        var second = Idempotent.Int<User>(1);

        // Assert
        first.Should().NotBe(second);
    }

    [Test]
    public void Int_WhenCalledWithDifferentSeed_ReturnsDifferentValue()
    {
        // Arrange / Act
        var withDefaultSeed = Idempotent.Int<User>(0);
        var withCustomSeed = Idempotent.Int<User>(0, "custom");

        // Assert
        withDefaultSeed.Should().NotBe(withCustomSeed);
    }

    [Test]
    public void Int_WhenCalledWithNullSeed_BehavesSameAsOmittingSeed()
    {
        // Arrange / Act
        var withNullSeed = Idempotent.Int<User>(0, null);
        var withNoSeed = Idempotent.Int<User>(0);

        // Assert
        withNullSeed.Should().Be(withNoSeed);
    }

    [Test]
    public void Int_GenericAndNonGenericOverloads_ReturnSameValue()
    {
        // Arrange / Act
        var generic = Idempotent.Int<User>(0);
        var nonGeneric = Idempotent.Int(typeof(User), 0);

        // Assert
        generic.Should().Be(nonGeneric);
    }
}
