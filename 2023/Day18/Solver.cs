namespace AdventOfCode.Y2023.Day18;

using Polygon = (List<Point> points, long length);

[Puzzle("Lavaduct Lagoon", 2023, 18)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        return new (
            CountPoints(GetDiggedHolesPart1(input)),
            CountPoints(GetDiggedHolesPart2(input)));
    }

    private static long CountPoints(Polygon polygon)
    {
        // Use shoelace formula to get area (https://de.wikipedia.org/wiki/Gau%C3%9Fsche_Trapezformel)
        var area = Enumerable.Range(0, polygon.points.Count - 1)
                             .Sum(n => ((long) polygon.points[n].Y + polygon.points[n + 1].Y) * 
                                       ((long) polygon.points[n].X - polygon.points[n + 1].X) / 2);

        // Use pick theorem to get inner points (https://de.wikipedia.org/wiki/Satz_von_Pick)
        var r = polygon.length;
        var i =  area - (r / 2) + 1;

        return i + r;
    }    

    private static Polygon GetDiggedHolesPart1(string input)
    {
        var currentPosition = new Point(0, 0);
        var holes = new List<Point>([currentPosition]);
        var length = 0;

        foreach (var instruction in input.SplitLines())
        {
            var instructionParts = instruction.Split(" ");
            var direction = instructionParts[0] switch
            {
                "R" => Direction.Right,
                "L" => Direction.Left,
                "D" => Direction.Up,
                "U" => Direction.Down,
                _ => throw new ArgumentException("Bad direction")
            };
            var stepLength = int.Parse(instructionParts[1]);
            currentPosition = currentPosition.Move(direction, stepLength);
            length += stepLength;
            holes.Add(currentPosition);
        }
        return (holes, length);
    }

    private static Polygon GetDiggedHolesPart2(string input)
    {
        var currentPosition = new Point(0, 0);
        var holes = new List<Point>([currentPosition]);
        var length = 0;
        
        foreach (var instruction in input.SplitLines())
        {
            var hexCode = instruction.Split(" ")[2];
            var direction = hexCode[^2] switch
            {
                '0' => Direction.Right,
                '1' => Direction.Up,
                '2' => Direction.Left,
                '3' => Direction.Down,
                _ => throw new ArgumentException("Bad direction")
            };
            var stepLength = Convert.ToInt32(hexCode[2..^2], 16);
            currentPosition = currentPosition.Move(direction, stepLength);
            length += stepLength;
            holes.Add(currentPosition);
        }
        return (holes, length);
    }
}