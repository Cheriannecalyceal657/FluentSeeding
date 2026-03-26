namespace FluentSeeding.Faker.Extensions;

public static class BrazilianSeedRuleExtensions
{
    public static SeedBuilder<T> UseCpf<T>(this SeedRule<T, string> rule, bool formatted = false) where T : class
    {
        return rule.UseFactory(() => GenerateCpf(formatted));
    }

    private static string GenerateCpf(bool formatted = false)
    {
        var n = Random.Shared.Next(0, 1_000_000_000);

        var d0 = n / 100_000_000 % 10;
        var d1 = n / 10_000_000 % 10;
        var d2 = n / 1_000_000 % 10;
        var d3 = n / 100_000 % 10;
        var d4 = n / 10_000 % 10;
        var d5 = n / 1_000 % 10;
        var d6 = n / 100 % 10;
        var d7 = n / 10 % 10;
        var d8 = n % 10;

        var sum1 = d0 * 10 + d1 * 9 + d2 * 8 + d3 * 7 + d4 * 6 + d5 * 5 + d6 * 4 + d7 * 3 + d8 * 2;
        var r1 = sum1 % 11;
        var c1 = r1 < 2 ? 0 : 11 - r1;

        var sum2 = d0 * 11 + d1 * 10 + d2 * 9 + d3 * 8 + d4 * 7 + d5 * 6 + d6 * 5 + d7 * 4 + d8 * 3 + c1 * 2;
        var r2 = sum2 % 11;
        var c2 = r2 < 2 ? 0 : 11 - r2;

        if (formatted)
            return $"{d0}{d1}{d2}.{d3}{d4}{d5}.{d6}{d7}{d8}-{c1}{c2}";
        return $"{d0}{d1}{d2}{d3}{d4}{d5}{d6}{d7}{d8}{c1}{c2}";
    }
}