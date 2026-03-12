using FluentAssertions;

namespace FluentSeeding.Tests.EntitySeeder;

[TestFixture(TestName = "EntitySeeder.Seed")]
[Category("Unit")]
[Category(nameof(EntitySeeder<>))]
public sealed class EntitySeederSeedTests
{
    private class Item
    {
        public string Label { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    private class ItemSeeder : EntitySeeder<Item>
    {
        protected override void Configure(SeedBuilder<Item> builder)
        {
            builder.Count(3);
            builder.RuleFor(i => i.Label).UseValue("Test");
            builder.RuleFor(i => i.Value).UseValue(42);
        }
    }

    private class VariableCountSeeder : EntitySeeder<Item>
    {
        protected override void Configure(SeedBuilder<Item> builder)
        {
            builder.Count(2, 5);
            builder.RuleFor(i => i.Label).UseValue("Ranged");
            builder.RuleFor(i => i.Value).UseValue(1);
        }
    }

    private class SingleItemSeeder : EntitySeeder<Item>
    {
        protected override void Configure(SeedBuilder<Item> builder)
        {
            builder.RuleFor(i => i.Label).UseValue("One");
            builder.RuleFor(i => i.Value).UseValue(99);
        }
    }

    [Test]
    public void Seed_WhenConfiguredWithFixedCount_ReturnsCorrectNumberOfEntities()
    {
        // Arrange
        var seeder = new ItemSeeder();

        // Act
        var result = seeder.Seed().ToList();

        // Assert
        result.Should().HaveCount(3);
    }

    [Test]
    public void Seed_WhenConfiguredWithRules_ReturnsEntitiesWithAppliedValues()
    {
        // Arrange
        var seeder = new ItemSeeder();

        // Act
        var result = seeder.Seed().ToList();

        // Assert
        result.Should().OnlyContain(i => i.Label == "Test" && i.Value == 42);
    }

    [Test]
    public void Seed_WhenCalledMultipleTimes_ReturnsNewInstancesEachTime()
    {
        // Arrange
        var seeder = new ItemSeeder();

        // Act
        var first = seeder.Seed().ToList();
        var second = seeder.Seed().ToList();

        // Assert
        first.Should().NotBeSameAs(second);
        first.Zip(second).Should().OnlyContain(pair => !ReferenceEquals(pair.First, pair.Second));
    }

    [Test]
    public void Seed_WhenCountIsDefault_ReturnsOneEntity()
    {
        // Arrange
        var seeder = new SingleItemSeeder();

        // Act
        var result = seeder.Seed().ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].Label.Should().Be("One");
        result[0].Value.Should().Be(99);
    }

    [Test]
    public void Seed_WhenConfiguredWithRange_ReturnsCountWithinRange()
    {
        // Arrange
        var seeder = new VariableCountSeeder();

        // Act & Assert — repeat to increase confidence the random stays in range
        for (int i = 0; i < 30; i++)
        {
            var count = seeder.Seed().Count();
            count.Should().BeInRange(2, 5);
        }
    }

    [Test]
    public void SeedInternal_WhenCalled_ReturnsSameResultsAsSeed()
    {
        // Arrange
        var seeder = new ItemSeeder();

        // Act
        var fromSeed = seeder.Seed().ToList();
        var fromInternal = seeder.SeedInternal().ToList();

        // Assert — both should produce entities of the same type, count, and values
        fromInternal.Should().HaveCount(fromSeed.Count);
        fromInternal.Should().AllBeOfType<Item>();
        fromInternal.Cast<Item>().Should().OnlyContain(i => i.Label == "Test" && i.Value == 42);
    }

    [Test]
    public void SeedInternal_ReturnedObjects_AreOfCorrectType()
    {
        // Arrange
        var seeder = new ItemSeeder();

        // Act
        var result = seeder.SeedInternal().ToList();

        // Assert
        result.Should().AllBeOfType<Item>();
    }
}
