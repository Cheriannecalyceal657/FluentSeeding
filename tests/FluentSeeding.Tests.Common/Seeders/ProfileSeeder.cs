namespace FluentSeeding.Tests.Common.Seeders;

public sealed class ProfileSeeder : EntitySeeder<Profile>
{
    protected override void Configure(SeedBuilder<Profile> builder)
    {
        builder.Count(5);
        builder.HasOne(p => p.User, u => u.RuleFor(user => user.Name).UseFactory(i => $"User {i}"));
    }
}
