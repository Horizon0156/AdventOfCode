#!/usr/bin/env dotnet-script
#nullable enable

public static IEnumerable<T> WherePairwise<T>(
        this IEnumerable<T> elements,
        Func<T, T, bool> filter)
{
    var elementArray = elements.ToArray();

    for (var i = 1; i < elementArray.Length; i++)
    {
        if (filter(elementArray[i - 1], elementArray[i]))
        yield return elementArray[i];
    }
}

public static IEnumerable<TR> SlidingWindow<T, TR>(
    this IEnumerable<T> elements,
    int windowSize,
    Func<IEnumerable<T>, TR> aggregator)
{
    var elementArray = elements.ToArray();

    for (var i = 0; i <= elementArray.Length - windowSize; i++)
    {
        yield return aggregator(elements.Skip(i).Take(windowSize));
    }
}

var data = await File.ReadAllLinesAsync("Data/Day1.txt");
var measurements = data.Select(int.Parse);

var part1 = measurements
                .WherePairwise((previous, current) => previous < current)
                .Count();

var part2 = measurements
                .SlidingWindow(3, batch => batch.Sum())
                .WherePairwise((previous, current) => previous < current)
                .Count();

Console.WriteLine("Day 1: Sonar Sweep");
Console.WriteLine($"{part1}, {part2}");
