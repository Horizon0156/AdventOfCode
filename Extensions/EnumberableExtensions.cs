namespace AdventOfCode.Extensions;

internal static class EnumerableExtensions 
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var element in source) action(element);
        return source;
    }
    
    public static T Remove<T>(this List<T> list, Func<T, bool> predicate)
    {
        var value = list.Single(predicate);
        list.Remove(value);

        return value;
    }

    public static IEnumerable<(T p1, T p2)> Pairwise<T>(this IEnumerable<T> elements)
    {
        var elementArray = elements.ToArray();
        if (elementArray.Length % 2 != 0)
        {
            throw new ArgumentException("Elements have an odd length.", nameof(elements));
        }
        for (var i = 0; i <= elementArray.Length - 1; i += 2)
        {
            yield return (elementArray[i], elementArray[i + 1]);
        }
    }

    public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> elements, int windowSize)
    {
        for (var i = 0; i <= elements.Count() - windowSize; i++)
        {
            yield return elements.Skip(i).Take(windowSize);
        }
    }

    public static T[,] To2DArray<T>(this IEnumerable<IEnumerable<T>> source)
    {
        var jaggedArray = source.Select(x => x.ToArray()).ToArray();
        var array = new T[jaggedArray.Length, jaggedArray[0].Length];

        for (var y = 0; y < jaggedArray.Length; y++)
        {
            for (var x = 0; x < jaggedArray[y].Length; x++)
            {
                array[y, x] = jaggedArray[y][x];
            }
        }
        return array;
    }
}