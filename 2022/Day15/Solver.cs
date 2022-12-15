using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day15;

[Problem("Beacon Exclusion Zone", 2022, 15)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var sensors = input.SplitLines()
                           .Select(Sensor.Parse)
                           .OrderBy(s => s.Position.X)
                           .ToArray();

        return new(
            CountCoveredPointsOnLine(sensors, 2000000),
            GetTuningFrequency(sensors, 4000000));
    }

    static int CountCoveredPointsOnLine(Sensor[] sensors, int y)
    {
        var westX = sensors.Min(s => s.Position.X - s.Range);
        var eastX = sensors.Max(s => s.Position.X + s.Range);

        var beaconsInRow = sensors.Select(s => s.ClosestBeacon)
                                  .ToHashSet()
                                  .Count(b => b.Y == y);

        var coveredPoints = Enumerable.Range(westX, eastX - westX)
                                      .AsParallel()
                                      .Where(x => sensors.Any(s => s.InRange(new(x, y))))
                                      .Count();
        
        return coveredPoints - beaconsInRow;
    }

    static long GetTuningFrequency(Sensor[] sensors, int maxDistance)
    {
        foreach (var sensor in sensors)
        {
            // To find the distress beacon we iterate over the edges of the sensor fields / diamonds.
            var xStart = Math.Max(0, sensor.Position.X - sensor.Range - 1);
            var xEnd = Math.Min(sensor.Position.X + sensor.Range + 1, maxDistance);
            var distressBeacon = 
                Enumerable.Range(xStart, xEnd - xStart)
                .SelectMany(x => new[] 
                {
                    new Point(x, Math.Max(0, sensor.Position.Y - (sensor.Range + 1 - Math.Abs(sensor.Position.X - x)))),
                    new Point(x, Math.Min(sensor.Position.Y + (sensor.Range + 1 - Math.Abs(sensor.Position.X - x)), maxDistance))
                })
                .AsParallel()
                .FirstOrDefault(p => sensors.All(s => !s.InRange(p)));

            if (distressBeacon != null)
            {
                return Math.BigMul(distressBeacon.X, maxDistance) + distressBeacon.Y;
            } 
        }
        return 0;
    }
    
    record Point(int X, int Y)
    {
        public int ManhattanDistance(Point other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    record Sensor(Point Position, Point ClosestBeacon)
    {
        public int Range { get; } = Position.ManhattanDistance(ClosestBeacon);

        public bool InRange(Point p) => Position.ManhattanDistance(p) <= Range;

        public static Sensor Parse(string value)
        {
            var match = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)")
                           .Match(value).Groups;
            return new (
                new (int.Parse(match[1].Value), int.Parse(match[2].Value)),
                new (int.Parse(match[3].Value), int.Parse(match[4].Value)));
        }
    }
}