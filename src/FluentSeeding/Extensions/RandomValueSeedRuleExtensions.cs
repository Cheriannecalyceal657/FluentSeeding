namespace FluentSeeding.Extensions;

/// <summary>
/// Extension methods that configure a <see cref="SeedRule{T,TProperty}"/> with a randomly-generated
/// value source for the most common .NET primitive and date/time types.
/// Each overload delegates to <see cref="SeedRule{T,TProperty}.UseFactory(Func{TProperty})"/> and
/// returns the parent <see cref="SeedBuilder{T}"/> to continue the fluent chain.
/// </summary>
public static class RandomValueSeedRuleExtensions
{
    /// <summary>
    /// Generates a new <see cref="Guid"/> per entity using <see cref="Guid.NewGuid"/>.
    /// </summary>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, Guid> builder) where T : class
    {
        return builder.UseFactory(Guid.NewGuid);
    }

    /// <summary>
    /// Generates a random <see cref="int"/> per entity, optionally clamped to
    /// [<paramref name="min"/>, <paramref name="max"/>).
    /// When neither bound is provided, the full <see cref="int"/> range including negative values is used.
    /// </summary>
    /// <param name="min">Inclusive lower bound. Defaults to <see cref="int.MinValue"/> when <paramref name="max"/> is set but <paramref name="min"/> is not.</param>
    /// <param name="max">Exclusive upper bound. Defaults to <see cref="int.MaxValue"/> when <paramref name="min"/> is set but <paramref name="max"/> is not.</param>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, int> builder, int? min = null, int? max = null)
        where T : class
    {
        return builder.UseFactory(() => Random.Shared.NextInt32(min, max));
    }

    /// <summary>
    /// Generates a random <see cref="long"/> per entity, optionally clamped to
    /// [<paramref name="min"/>, <paramref name="max"/>).
    /// When neither bound is provided, the full <see cref="long"/> range — including negative values — is used.
    /// </summary>
    /// <param name="min">Inclusive lower bound. Defaults to <see cref="long.MinValue"/> when <paramref name="max"/> is set but <paramref name="min"/> is not.</param>
    /// <param name="max">Exclusive upper bound. Defaults to <see cref="long.MaxValue"/> when <paramref name="min"/> is set but <paramref name="max"/> is not.</param>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, long> builder, long? min = null, long? max = null)
        where T : class
    {
        return builder.UseFactory(() => Random.Shared.NextLong(min, max));
    }

    /// <summary>
    /// Generates a random <see cref="float"/> per entity. Without bounds, returns a value in [0.0, 1.0).
    /// With bounds, scales linearly over [<paramref name="min"/>, <paramref name="max"/>).
    /// </summary>
    /// <param name="min">Inclusive lower bound. Defaults to <see cref="float.MinValue"/> when only <paramref name="max"/> is set.</param>
    /// <param name="max">Exclusive upper bound. Defaults to <see cref="float.MaxValue"/> when only <paramref name="min"/> is set.</param>
    /// <remarks>
    /// Providing only one bound causes the other to default to <see cref="float.MinValue"/> or
    /// <see cref="float.MaxValue"/>. The resulting range is so large it will overflow to
    /// <see cref="float.PositiveInfinity"/> or <see cref="float.NegativeInfinity"/>.
    /// Always specify both bounds together to get meaningful values.
    /// </remarks>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, float> builder, float? min = null, float? max = null)
        where T : class
    {
        return builder.UseFactory(() =>
        {
            float value = (float)Random.Shared.NextDouble();
            if (min.HasValue || max.HasValue)
            {
                float actualMin = min ?? float.MinValue;
                float actualMax = max ?? float.MaxValue;
                value = actualMin + value * (actualMax - actualMin);
            }

            return value;
        });
    }

    /// <summary>
    /// Generates a random <see cref="double"/> per entity. Without bounds, returns a value in [0.0, 1.0).
    /// With bounds, scales linearly over [<paramref name="min"/>, <paramref name="max"/>).
    /// </summary>
    /// <param name="min">Inclusive lower bound. Defaults to <see cref="double.MinValue"/> when only <paramref name="max"/> is set.</param>
    /// <param name="max">Exclusive upper bound. Defaults to <see cref="double.MaxValue"/> when only <paramref name="min"/> is set.</param>
    /// <remarks>
    /// Providing only one bound causes the other to default to <see cref="double.MinValue"/> or
    /// <see cref="double.MaxValue"/>. The resulting range is so large it will overflow to
    /// <see cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.
    /// Always specify both bounds together to get meaningful values.
    /// </remarks>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, double> builder, double? min = null, double? max = null)
        where T : class
    {
        return builder.UseFactory(() =>
        {
            double value = Random.Shared.NextDouble();
            if (min.HasValue || max.HasValue)
            {
                double actualMin = min ?? double.MinValue;
                double actualMax = max ?? double.MaxValue;
                value = actualMin + value * (actualMax - actualMin);
            }

            return value;
        });
    }

    /// <summary>
    /// Generates a random <see cref="decimal"/> per entity. Without bounds, produces a value
    /// distributed across a wide range with scale 28. With bounds, scales linearly from
    /// <paramref name="min"/> to <paramref name="max"/>.
    /// </summary>
    /// <param name="min">
    /// Inclusive lower bound. Defaults to <c>0m</c>not <see cref="decimal.MinValue"/> when
    /// <paramref name="max"/> is set but <paramref name="min"/> is not.
    /// </param>
    /// <param name="max">Upper bound. Defaults to <see cref="decimal.MaxValue"/> when <paramref name="min"/> is set but <paramref name="max"/> is not.</param>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, decimal> builder, decimal? min = null,
        decimal? max = null) where T : class
    {
        return builder.UseFactory(() => Random.Shared.NextDecimal(min, max));
    }

    /// <summary>
    /// Generates a random <see cref="bool"/> per entity with equal probability of
    /// <see langword="true"/> and <see langword="false"/>.
    /// </summary>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, bool> builder) where T : class
    {
        return builder.UseFactory(() => Random.Shared.Next(2) == 0);
    }

    /// <summary>
    /// Generates a random alphanumeric string of <paramref name="length"/> characters per entity.
    /// </summary>
    /// <param name="length">Number of characters in the generated string. Defaults to 10.</param>
    /// <remarks>
    /// The character pool is <c>A-Z</c>, <c>a-z</c>, and <c>0-9</c>.
    /// No special characters, whitespace, or Unicode code points are included.
    /// </remarks>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, string> builder, int length = 10) where T : class
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return builder.UseFactory(() =>
            new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Shared.Next(s.Length)]).ToArray()));
    }

    /// <summary>
    /// Picks a uniformly random member of <typeparamref name="TEnum"/> per entity.
    /// </summary>
    public static SeedBuilder<T> UseRandom<T, TEnum>(this SeedRule<T, TEnum> builder) where T : class
        where TEnum : struct, Enum
    {
        var values = Enum.GetValues<TEnum>();
        return builder.UseFactory(() => values[Random.Shared.Next(values.Length)]);
    }

    /// <summary>
    /// Generates a random <see cref="DateTime"/> per entity within
    /// [<paramref name="from"/>, <paramref name="to"/>).
    /// Defaults to the full <see cref="DateTime.MinValue"/>-<see cref="DateTime.MaxValue"/> range.
    /// </summary>
    /// <param name="from">Inclusive lower bound. Defaults to <see cref="DateTime.MinValue"/>.</param>
    /// <param name="to">Exclusive upper bound. Defaults to <see cref="DateTime.MaxValue"/>.</param>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, DateTime> builder, DateTime? from = null, DateTime? to = null) where T : class
    {
        return builder.UseFactory(() =>
        {
            var actualFrom = from ?? DateTime.MinValue;
            var actualTo = to ?? DateTime.MaxValue;
            var range = (actualTo - actualFrom).Ticks;
            var randomTicks = (long)(Random.Shared.NextDouble() * range);
            return actualFrom.AddTicks(randomTicks);
        });
    }

    /// <summary>
    /// Generates a random <see cref="DateTimeOffset"/> per entity within
    /// [<paramref name="from"/>, <paramref name="to"/>).
    /// Defaults to the full <see cref="DateTimeOffset.MinValue"/>-<see cref="DateTimeOffset.MaxValue"/> range.
    /// </summary>
    /// <param name="from">Inclusive lower bound. Defaults to <see cref="DateTimeOffset.MinValue"/>.</param>
    /// <param name="to">Exclusive upper bound. Defaults to <see cref="DateTimeOffset.MaxValue"/>.</param>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, DateTimeOffset> builder, DateTimeOffset? from = null, DateTimeOffset? to = null) where T : class
    {
        return builder.UseFactory(() =>
        {
            var actualFrom = from ?? DateTimeOffset.MinValue;
            var actualTo = to ?? DateTimeOffset.MaxValue;
            var range = (actualTo - actualFrom).Ticks;
            var randomTicks = (long)(Random.Shared.NextDouble() * range);
            return actualFrom.AddTicks(randomTicks);
        });
    }

    /// <summary>
    /// Generates a random <see cref="TimeSpan"/> per entity within
    /// [<paramref name="from"/>, <paramref name="to"/>).
    /// Defaults to the full <see cref="TimeSpan.MinValue"/>-<see cref="TimeSpan.MaxValue"/> range.
    /// </summary>
    /// <param name="from">Inclusive lower bound. Defaults to <see cref="TimeSpan.MinValue"/>.</param>
    /// <param name="to">Exclusive upper bound. Defaults to <see cref="TimeSpan.MaxValue"/>.</param>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, TimeSpan> builder, TimeSpan? from = null, TimeSpan? to = null) where T : class
    {
        return builder.UseFactory(() =>
        {
            var actualFrom = from ?? TimeSpan.MinValue;
            var actualTo = to ?? TimeSpan.MaxValue;
            var range = (actualTo - actualFrom).Ticks;
            var randomTicks = (long)(Random.Shared.NextDouble() * range);
            return actualFrom.Add(TimeSpan.FromTicks(randomTicks));
        });
    }

    /// <summary>
    /// Generates a random <see cref="DateOnly"/> per entity within
    /// [<paramref name="from"/>, <paramref name="to"/>).
    /// Defaults to the full <see cref="DateOnly.MinValue"/>-<see cref="DateOnly.MaxValue"/> range.
    /// </summary>
    /// <param name="from">Inclusive lower bound. Defaults to <see cref="DateOnly.MinValue"/>.</param>
    /// <param name="to">Exclusive upper bound. Defaults to <see cref="DateOnly.MaxValue"/>.</param>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, DateOnly> builder, DateOnly? from = null, DateOnly? to = null) where T : class
    {
        return builder.UseFactory(() =>
        {
            var actualFrom = from ?? DateOnly.MinValue;
            var actualTo = to ?? DateOnly.MaxValue;
            var range = (actualTo.ToDateTime(TimeOnly.MinValue) - actualFrom.ToDateTime(TimeOnly.MinValue)).Ticks;
            var randomTicks = (long)(Random.Shared.NextDouble() * range);
            return DateOnly.FromDateTime(actualFrom.ToDateTime(TimeOnly.MinValue).AddTicks(randomTicks));
        });
    }

    /// <summary>
    /// Generates a random <see cref="TimeOnly"/> per entity within
    /// [<paramref name="from"/>, <paramref name="to"/>).
    /// Defaults to the full <see cref="TimeOnly.MinValue"/>-<see cref="TimeOnly.MaxValue"/> range.
    /// </summary>
    /// <param name="from">Inclusive lower bound. Defaults to <see cref="TimeOnly.MinValue"/>.</param>
    /// <param name="to">Exclusive upper bound. Defaults to <see cref="TimeOnly.MaxValue"/>.</param>
    public static SeedBuilder<T> UseRandom<T>(this SeedRule<T, TimeOnly> builder, TimeOnly? from = null, TimeOnly? to = null) where T : class
    {
        return builder.UseFactory(() =>
        {
            var actualFrom = from ?? TimeOnly.MinValue;
            var actualTo = to ?? TimeOnly.MaxValue;
            var range = (actualTo.ToTimeSpan() - actualFrom.ToTimeSpan()).Ticks;
            var randomTicks = (long)(Random.Shared.NextDouble() * range);
            return TimeOnly.FromTimeSpan(actualFrom.ToTimeSpan().Add(TimeSpan.FromTicks(randomTicks)));
        });
    }
}