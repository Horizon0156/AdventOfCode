namespace AdventOfCode.Extensions;

internal static class EnumerableExtensions 
{
    public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> elements, int windowSize)
    {
        for (var i = 0; i <= elements.Count() - windowSize; i++)
        {
            yield return elements.Skip(i).Take(windowSize);
        }
    }

    public static T Remove<T>(this List<T> list, Func<T, bool> predicate)
    {
        var value = list.Single(predicate);
        list.Remove(value);

        return value;
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