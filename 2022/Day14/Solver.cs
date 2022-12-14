namespace AdventOfCode.Y2022.Day14;

[Problem("Regolith Reservoir", 2022, 14)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var lineSegments = input.SplitLines()
                                .Select(l => l.Split(" -> ")
                                              .Select(Point.Parse));
        var occupiedPoints = lineSegments.SelectMany(s => s.SlidingWindow(2)
                                                           .SelectMany(l => l.First().InterpolateLine(l.Last())))
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

    record Point(int X, int Y)
    {
        public static Point Parse(string value)
        {
            var parts = value.Split(",");
            return new (int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public IEnumerable<Point> InterpolateLine(Point end)
        {
            var xStep = this.X <= end.X ? 1 : -1;
            var yStep = this.Y <= end.Y ? 1 : -1;

            for(var x = this.X; xStep > 0 ? x <= end.X : x >= end.X; x += xStep)
                for(var y = this.Y; yStep > 0 ? y <= end.Y : y >= end.Y; y += yStep)
                    yield return new (x, y);
        }
    }
}