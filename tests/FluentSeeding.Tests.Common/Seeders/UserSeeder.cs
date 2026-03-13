namespace FluentSeeding.Tests.Common.Seeders;

public sealed class UserSeeder : EntitySeeder<User>
{
    protected override void Configure(SeedBuilder<User> builder)
    {
        builder.Count(10);
        
        builder.RuleFor(u => u.Id).UseFactory(Guid.NewGuid);
        builder.RuleFor(u => u.Name).UseValue("Test User");
        builder.RuleFor(u => u.Email).UseFactory(i => DataUtils.GenerateEmail("Test User " + i));
    }
}
