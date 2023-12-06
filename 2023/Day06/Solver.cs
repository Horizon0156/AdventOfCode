namespace AdventOfCode.Y2023.Day06;

[Puzzle("Wait For It", 2023, 6)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var lines = input.SplitLines().ToArray();
        var times = lines[0].Matches<long>(@"\d+").ToArray();
        var distances = lines[1].Matches<long>(@"\d+").ToArray();
        var totalTime = long.Parse(string.Concat(times));
        var totalDistance = long.Parse(string.Concat(distances));
        return new (FindBetterWays(times, distances), FindBetterWays([totalTime], [totalDistance]));
    }

    private static int FindBetterWays(long[] times, long[] distances) => 
        // d = (t - a) * a
        Enumerable.Range(0, times.Length)
                  .Select(i => Enumerable.Range(1, (int) times[i] - 1)
                                         .Select(a => (times[i] - a) * a)
                                         .Where(d => d > distances[i]))
                  .Aggregate(1, (prod, next) => prod * next.Count());
}