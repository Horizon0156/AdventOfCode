#!/usr/bin/env dotnet-script

// Thanks to https://www.reddit.com/user/Neojume
// Analyzing the code shows 14 repeating blocks of 18 statements
// where only two variables might change. Those compared to previous while keeping
// constraints valid will provide the final numbers.
static long FindModelNumber(bool findMaximum)
{
    var lines = File.ReadAllLines("Data/Day24.txt").ToArray();
    var pairs = Enumerable.Range(0, 14)
                          .Select(i => (int.Parse(lines[i * 18 + 5][6..]),
                                        int.Parse(lines[i * 18 + 15][6..])));
    var stack = new Stack<(int, int)>();
    var links = new Dictionary<int, (int j, int delta)>();

    foreach (var (i, pair) in pairs.Select((p, i) => (i, p)))
    {
        if (pair.Item1 > 0)
        {
            stack.Push((i, pair.Item2));
        }
        else
        {
            var (j, bj) = stack.Pop();
            links[i] = (j, bj + pair.Item1);
        }
    }
    var assignments = new int[14];

    foreach (var (i, v) in links)
    {
        assignments[i] = findMaximum ? Math.Min(9, 9 + v.delta) : Math.Max(1, 1 + v.delta);
        assignments[v.j] = findMaximum ? Math.Min(9, 9 - v.delta) : Math.Max(1, 1 - v.delta);
    }

    return long.Parse(string.Join(string.Empty, assignments));
}

Console.WriteLine("Day 24: Arithmetic Logic Unit");
Console.WriteLine($"{FindModelNumber(findMaximum: true)}, {FindModelNumber(findMaximum: false)}");
