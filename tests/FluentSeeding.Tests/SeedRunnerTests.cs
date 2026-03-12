using FluentAssertions;
using FluentSeeding.Tests.Common;
using FluentSeeding.Tests.Common.Seeders;
using NSubstitute;

namespace FluentSeeding.Tests;

[TestFixture(TestName = "SeederRunner")]
[Category("Unit")]
[Category(nameof(SeederRunner))]
public sealed class SeedRunnerTests
{
    private IPersistenceLayer _persistenceLayer = null!;

    [SetUp]
    public void SetUp()
    {
        _persistenceLayer = Substitute.For<IPersistenceLayer>();
    }

    [Test]
    public void Run_WithNoSeeders_ShouldNotCallPersist()
    {
        // Arrange
        var sut = new SeederRunner(_persistenceLayer, []);

        // Act
        sut.Run();

        // Assert
        _persistenceLayer.DidNotReceive().Persist(Arg.Any<IEnumerable<object>>());
    }

    [Test]
    public void Run_WithSingleSeeder_ShouldCallPersistOnce()
    {
        // Arrange
        EntitySeederBase[] seeders = [new ProductSeeder()];
        var sut = new SeederRunner(_persistenceLayer, seeders);

        // Act
        sut.Run();

        // Assert
        _persistenceLayer.Received(1).Persist(Arg.Any<IEnumerable<object>>());
    }

    [Test]
    public void Run_WithMultipleSeeders_ShouldCallPersistForEachSeeder()
    {
        // Arrange
        EntitySeederBase[] seeders = [new UserSeeder(), new ProductSeeder()];
        var sut = new SeederRunner(_persistenceLayer, seeders);

        // Act
        sut.Run();

        // Assert
        _persistenceLayer.Received(2).Persist(Arg.Any<IEnumerable<object>>());
    }

    [Test]
    public void Run_WithProductSeeder_ShouldPersistCorrectEntityCount()
    {
        // Arrange
        var inMemory = new InMemoryPersistenceLayer();
        EntitySeederBase[] seeders = [new ProductSeeder()];
        var sut = new SeederRunner(inMemory, seeders);

        // Act
        sut.Run();

        // Assert
        inMemory.GetEntities<object>().Should().HaveCount(5);
    }

    [Test]
    public void Run_WithProductSeeder_ShouldPersistEntitiesWithCorrectValues()
    {
        // Arrange
        var inMemory = new InMemoryPersistenceLayer();
        EntitySeederBase[] seeders = [new ProductSeeder()];
        var sut = new SeederRunner(inMemory, seeders);

        // Act
        sut.Run();

        // Assert
        var products = inMemory.GetEntities<object>().Cast<Product>().ToList();
        products.Should().AllSatisfy(p =>
        {
            p.Name.Should().Be("Sample Product");
            p.Price.Should().Be(9.99m);
        });
    }

    [Test]
    public void Run_WithMultipleSeeders_ShouldPersistEntitiesFromAllSeeders()
    {
        // Arrange
        var inMemory = new InMemoryPersistenceLayer();
        EntitySeederBase[] seeders = [new UserSeeder(), new ProductSeeder()];
        var sut = new SeederRunner(inMemory, seeders);

        // Act
        sut.Run();

        // Assert
        inMemory.GetEntities<object>().Should().HaveCount(15); // 10 users + 5 products
    }

    [Test]
    public async Task RunAsync_WithNoSeeders_ShouldNotCallPersistAsync()
    {
        // Arrange
        var sut = new SeederRunner(_persistenceLayer, []);

        // Act
        await sut.RunAsync();

        // Assert
        await _persistenceLayer.DidNotReceive().PersistAsync(Arg.Any<IEnumerable<object>>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task RunAsync_WithSingleSeeder_ShouldCallPersistAsyncOnce()
    {
        // Arrange
        EntitySeederBase[] seeders = [new ProductSeeder()];
        var sut = new SeederRunner(_persistenceLayer, seeders);

        // Act
        await sut.RunAsync();

        // Assert
        await _persistenceLayer.Received(1).PersistAsync(Arg.Any<IEnumerable<object>>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task RunAsync_WithMultipleSeeders_ShouldCallPersistAsyncForEachSeeder()
    {
        // Arrange
        EntitySeederBase[] seeders = [new UserSeeder(), new ProductSeeder()];
        var sut = new SeederRunner(_persistenceLayer, seeders);

        // Act
        await sut.RunAsync();

        // Assert
        await _persistenceLayer.Received(2).PersistAsync(Arg.Any<IEnumerable<object>>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task RunAsync_ShouldPassCancellationTokenToPersistAsync()
    {
        // Arrange
        EntitySeederBase[] seeders = [new ProductSeeder()];
        var sut = new SeederRunner(_persistenceLayer, seeders);
        var cts = new CancellationTokenSource();

        // Act
        await sut.RunAsync(cts.Token);

        // Assert
        await _persistenceLayer.Received(1).PersistAsync(Arg.Any<IEnumerable<object>>(), cts.Token);
    }

    [Test]
    public async Task RunAsync_WithProductSeeder_ShouldPersistCorrectEntityCount()
    {
        // Arrange
        var inMemory = new InMemoryPersistenceLayer();
        EntitySeederBase[] seeders = [new ProductSeeder()];
        var sut = new SeederRunner(inMemory, seeders);

        // Act
        await sut.RunAsync();

        // Assert
        inMemory.GetEntities<object>().Should().HaveCount(5);
    }

    [Test]
    public async Task RunAsync_WithMultipleSeeders_ShouldPersistEntitiesFromAllSeeders()
    {
        // Arrange
        var inMemory = new InMemoryPersistenceLayer();
        EntitySeederBase[] seeders = [new UserSeeder(), new ProductSeeder()];
        var sut = new SeederRunner(inMemory, seeders);

        // Act
        await sut.RunAsync();

        // Assert
        inMemory.GetEntities<object>().Should().HaveCount(15); // 10 users + 5 products
    }
}
