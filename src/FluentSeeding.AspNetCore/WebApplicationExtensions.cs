using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FluentSeeding.AspNetCore;

/// <summary>
/// Extension methods for running FluentSeeding on an <see cref="IHost"/>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Creates a DI scope, resolves <see cref="SeederRunner"/>, and runs all registered seeders.
    /// Call this after <c>builder.Build()</c> and before <c>app.Run()</c>.
    /// </summary>
    /// <param name="host">The built host or web application.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    public static async Task RunSeedersAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<SeederRunner>();
        await runner.RunAsync(cancellationToken);
    }
}
