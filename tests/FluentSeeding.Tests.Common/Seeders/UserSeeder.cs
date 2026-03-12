namespace FluentSeeding.Tests.Common.Seeders;

public sealed class UserSeeder : EntitySeeder<User>
{
    protected override void Configure(SeedBuilder<User> builder)
    {
        builder.Count(10);
    }
}
