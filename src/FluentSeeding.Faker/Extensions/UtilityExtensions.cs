using FluentSeeding.Faker.Locales;

namespace FluentSeeding.Faker.Extensions;

internal static class UtilityExtensions
{
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
}
