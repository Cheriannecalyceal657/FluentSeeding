using FluentSeeding.Faker.Locales;

namespace FluentSeeding.Faker.Extensions;

internal static class UtilityExtensions
{
    public static string PickFromAny(params ReadOnlySpan<string[]> pools)
    {
        int total = 0;
        foreach (var pool in pools) total += pool.Length;
        int idx = Random.Shared.Next(total);                                                                                                                                                  
        foreach (var pool in pools)
        {                                                                                                                                                                                     
            if (idx < pool.Length) return pool[idx];          
            idx -= pool.Length;
        }

        return pools[0][0]; 
    }
    public static string[] GetForGender(this GenderedArrays arrays, Gender gender)
    {
        return gender switch
        {
            Gender.Male => arrays.Male,
            Gender.Female => arrays.Female,
            _ => arrays.Male.Concat(arrays.Female).ToArray()
        };
    }
    
    public static T Pick<T>(this IEnumerable<T> source)
    {
        var list = source as IList<T> ?? source.ToList();
        if (!list.Any())
            throw new InvalidOperationException("Sequence contains no elements");
        var index = Random.Shared.Next(list.Count);
        return list[index];
    }
    
    public static IEnumerable<T> PickMany<T>(this IEnumerable<T> source, int count)
    {
        var list = source as IList<T> ?? source.ToList();
        if (!list.Any())
            throw new InvalidOperationException("Sequence contains no elements");
        for (int i = 0; i < count; i++)
        {
            var index = Random.Shared.Next(list.Count);
            yield return list[index];
        }
    }

    /// <summary>
    /// Replaces each <c>#</c> character in <paramref name="format"/> with a random decimal digit (0–9).
    /// All other characters are passed through unchanged.
    /// </summary>
    public static string FillFormat(this string format)
    {
        Span<char> buf = format.Length <= 64
            ? stackalloc char[format.Length]
            : new char[format.Length];

        for (var i = 0; i < format.Length; i++)
            buf[i] = format[i] == '#' ? (char)('0' + Random.Shared.Next(10)) : format[i];

        return new string(buf);
    }
}
