namespace AdventOfCode.Y2022.Day14;

[Puzzle("Regolith Reservoir", 2022, 14)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var lineSegments = input.SplitLines()
                                .Select(l => l.Split(" -> ")
                                              .Select(Point.Parse));
        var occupiedPoints = lineSegments.SelectMany(s => s.SlidingWindow(2)
                                                           .SelectMany(l => l.First().Interpolate(l.Last())))
                                         .ToHashSet();

        return new(
            CountSand(new HashSet<Point>(occupiedPoints), hasFloor: false),
            CountSand(occupiedPoints, hasFloor: true));
    }

    private static int CountSand(HashSet<Point> occupiedPoints, bool hasFloor)
    {
        var sand = new Point(500, 0);
        var abyss = occupiedPoints.Max(p => p.Y) + 1;
        var floor = occupiedPoints.Max(p => p.Y) + 2;
        var sandCount = 0;

        while (hasFloor ? !occupiedPoints.Contains(new Point(500, 0)) : sand.Y < abyss)
        {
            var previousPosition = sand;
            sand = sand with { Y = sand.Y + 1 };
            if (!occupiedPoints.Contains(sand) && (!hasFloor || sand.Y < floor)) continue;
            sand = sand with { X = sand.X - 1 };
            if (!occupiedPoints.Contains(sand) && (!hasFloor || sand.Y < floor)) continue;
            sand = sand with { X = sand.X + 2 };
            if (!occupiedPoints.Contains(sand) && (!hasFloor || sand.Y < floor)) continue;
            occupiedPoints.Add(previousPosition);
            sandCount++;
            sand = new Point(500, 0);
        }
        return sandCount;
    }
}