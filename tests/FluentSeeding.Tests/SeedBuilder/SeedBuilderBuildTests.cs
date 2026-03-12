using FluentAssertions;
using FluentSeeding.Tests.Common;

namespace FluentSeeding.Tests.SeedBuilder;

[TestFixture(TestName = "SeedBuilder.Build")]
[Category("Unit")]
[Category(nameof(SeedBuilder<>))]
public sealed class SeedBuilderBuildTests
{

    [Test]
    public void Build_WhenCalled_ShouldGenerateEntities()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        
        builder.Count(3)
            .RuleFor(u => u.Bar).UseValue("Lorem Ipsum");

        // Act
        var users = builder.Build().ToList();

        // Assert
        users.Should().HaveCount(3);
        users.Should().OnlyContain(u => u.Bar == "Lorem Ipsum");
    }
    
    [Test]
    public void Build_WhenNoFactory_ShouldUseDefaultConstructor()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();

        // Act
        var users = builder.Build().ToList();

        // Assert
        users.Should().OnlyContain(u => u.Foo == 0 && u.Bar == string.Empty);
    }
    
    [Test]
    public void Build_WhenFactoryProvided_ShouldUseFactory()
    {
        // Arrange
        var builder = new SeedBuilder<Model>();
        builder.WithFactory(() => new Model(42, "Factory Value"));

        // Act
        var users = builder.Build().ToList();

        // Assert
        users.Should().OnlyContain(u => u.Foo == 42 && u.Bar == "Factory Value");
    }
    
    private class Model
    {
        public int Foo { get; set; }
        public string Bar { get; set; } = string.Empty;
        
        public Model()
        {
            Foo = 0;
            Bar = string.Empty;
        }
        
        public Model(int foo, string bar)
        {
            Foo = foo;
            Bar = bar;
        }
    }
}