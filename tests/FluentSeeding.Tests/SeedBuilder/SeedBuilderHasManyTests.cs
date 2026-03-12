using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.SeedBuilder;

[TestFixture(TestName = "SeedBuilder.HasMany")]
[Category("Unit")]
[Category(nameof(SeedBuilder<>))]
public sealed class SeedBuilderHasManyTests
{
    [Test]
    public void HasMany_WhenBuilt_PopulatesListProperty()
    {
        // Arrange
        var builder = new SeedBuilder<User>();
        builder
            .RuleFor(u => u.Id).UseFactory(Guid.NewGuid)
            .RuleFor(u => u.Name).UseValue("Alice")
            .RuleFor(u => u.Email).UseValue("alice@example.com")
            .HasMany(u => u.Purchases, b => b
                .Count(3)
                .RuleFor(p => p.Id).UseFactory(Guid.NewGuid)
                .RuleFor(p => p.Quantity).UseValue(2));

        // Act
        var user = builder.Build().Single();

        // Assert
        user.Purchases.Should().HaveCount(3);
        user.Purchases.Should().OnlyContain(p => p.Quantity == 2);
    }

    [Test]
    public void HasMany_EachParentEntityReceivesItsOwnCollection()
    {
        // Arrange
        var builder = new SeedBuilder<User>();
        builder
            .Count(2)
            .RuleFor(u => u.Id).UseFactory(Guid.NewGuid)
            .RuleFor(u => u.Name).UseValue("Bob")
            .RuleFor(u => u.Email).UseValue("bob@example.com")
            .HasMany(u => u.Purchases, b => b
                .Count(2)
                .RuleFor(p => p.Id).UseFactory(Guid.NewGuid)
                .RuleFor(p => p.Quantity).UseValue(1));

        // Act
        var users = builder.Build().ToList();

        // Assert
        users.Should().HaveCount(2);
        users.Should().OnlyContain(u => u.Purchases.Count == 2);
        users[0].Purchases.Should().NotBeSameAs(users[1].Purchases);
    }

    [Test]
    public void HasMany_DefaultCount_ProducesOneItem()
    {
        // Arrange
        var builder = new SeedBuilder<User>();
        builder
            .RuleFor(u => u.Id).UseFactory(Guid.NewGuid)
            .RuleFor(u => u.Name).UseValue("Charlie")
            .RuleFor(u => u.Email).UseValue("charlie@example.com")
            .HasMany(u => u.Purchases, b => b
                .RuleFor(p => p.Id).UseFactory(Guid.NewGuid)
                .RuleFor(p => p.Quantity).UseValue(5));

        // Act
        var user = builder.Build().Single();

        // Assert
        user.Purchases.Should().HaveCount(1);
    }

    [Test]
    public void HasMany_WithNestedSelector_ThrowsArgumentException()
    {
        // Arrange
        var builder = new SeedBuilder<Profile>();

        // Act
        Action act = () => builder.HasMany(p => p.User.Purchases, b => b
            .RuleFor(p => p.Id).UseFactory(Guid.NewGuid)
            .RuleFor(p => p.Quantity).UseValue(1));

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("selector");
    }
}
