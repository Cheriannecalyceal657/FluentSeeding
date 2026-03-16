namespace FluentSeeding.Extensions;

internal static class RandomExtensions
{
    public static int NextInt32(this Random rng, int? min = null, int? max = null)
    {
        if (min.HasValue || max.HasValue)
            return rng.Next(min ?? int.MinValue, max ?? int.MaxValue);

        int firstBits = rng.Next(0, 1 << 4) << 28;
        int lastBits = rng.Next(0, 1 << 28);
        return firstBits | lastBits;
    }

    public static long NextLong(this Random random, long? min = null, long? max = null)
    {
        if (min.HasValue || max.HasValue)
        {
            long actualMin = min ?? long.MinValue;
            long actualMax = max ?? long.MaxValue;
            ulong range = (ulong)(actualMax - actualMin);
            var buffer = new byte[8];
            random.NextBytes(buffer);
            ulong value = BitConverter.ToUInt64(buffer, 0) % range;
            return (long)value + actualMin;
        }

        var fullBuffer = new byte[8];
        random.NextBytes(fullBuffer);
        return BitConverter.ToInt64(fullBuffer, 0);
    }

    public static decimal NextDecimal(this Random rng, decimal? min = null, decimal? max = null)
    {
        if (min.HasValue || max.HasValue)
        {
            decimal actualMin = min ?? 0m;
            decimal actualMax = max ?? decimal.MaxValue;
            return actualMin + (decimal)rng.NextDouble() * (actualMax - actualMin);
        }

        return new decimal(rng.NextInt32(),
            rng.NextInt32(),
            rng.Next(0x204FCE5E),
            false,
            28);
    }
}