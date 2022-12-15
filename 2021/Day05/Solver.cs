namespace AdventOfCode.Y2021.Day05;

[Problem("Hydrothermal Venture", 2021, 5)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var lines = input.SplitLines().Select(Line.Parse);
        var overlappingPoints = lines
                          //.Where(l => l.IsHorizontal || l.IsVertical) // Part 1
                          .SelectMany(l => l.GetAllPoints())
                          .GroupBy(p => p)
                          .Count(g => g.Count() > 1);

        return new ("-", overlappingPoints);
    }

    private record Line(Point Start, Point End)
    {
        public bool IsHorizontal => Start.Y == End.Y;

        public bool IsVertical => Start.X == End.X;

        public List<Point> GetAllPoints() 
        {
            var points = new List<Point>() { Start };
            var currentPoint = Start;

            for (var i = 1; currentPoint != End; i++)
            {
                var xIncreasing = (End.X - Start.X) >= 0;
                var yIncreasing = (End.Y - Start.Y) >= 0;

                currentPoint = IsHorizontal
                    ? new (xIncreasing ? Start.X + i : Start.X - i, Start.Y)
                    : IsVertical
                        ? new (Start.X, yIncreasing ? Start.Y + i : Start.Y - i)
                        : new (xIncreasing ? Start.X + i : Start.X - i,
                            yIncreasing ? Start.Y + i : Start.Y - i);
                points.Add(currentPoint);
            }
            return points;
        }

        public static Line Parse(string line)
        {
            var match = line.Match<int>(@"(\d+),(\d+) (?:->) (\d+),(\d+)");
            return new (new (match[0], match[1]), new (match[2], match[3]));
        }
    }
}