namespace AdventOfCode.Extensions;

internal static class ArrayExtensions
{
    public static int IndexOf<T>(this T[] array, T value) => Array.IndexOf<T>(array, value);

    public static int FindIndex<T>(this T[] array, Predicate<T> match) => Array.FindIndex<T>(array, match);

    public static void Move<T>(this T[] array, int fromIndex, int toIndex)
    {
        var movedElement = array[fromIndex];
        var length = fromIndex - toIndex;
        if (length > 0) Array.Copy(array, toIndex, array, toIndex + 1, length);
        if (length < 0) Array.Copy(array, fromIndex + 1, array, fromIndex, -length);
        array[toIndex] = movedElement;
    }
}