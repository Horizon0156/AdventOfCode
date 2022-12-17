namespace AdventOfCode.Y2022.Day17;

[Problem("Pyroclastic Flow", 2022, 17)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var jetDirections = input.ToCharArray();

        return new(DropStones(2022, jetDirections).Height, 0);
    }

    static (int Height, int Stones) DropStones(long stones, char[] jetDirections)
    {
        var chamber = Enumerable.Range(0, 7).Select(x => new Point(x, 0)).ToHashSet();
        var currentHeight = 0;
        var stone = Shape.Create(0, currentHeight + 4);
        var stonesSpawned = 1;
        var steps = 0;

        while (stonesSpawned - 1 < stones)
        {
            var jetDirection = jetDirections[steps % jetDirections.Length];
            var movedStone = stone.Move(jetDirection);
            if (movedStone.Points.All(p => !chamber.Contains(p)) && movedStone.OuterLeft >= 0 && movedStone.OuterRight <= 6)
                stone = movedStone;

            movedStone = stone.Move('v');
            if (movedStone.Points.Any(p => chamber.Contains(p)))
            {
                stone.Points.ForEach(p => chamber.Add(p));
                currentHeight = chamber.Select(p => p.Y).Max();
                stone = Shape.Create(stonesSpawned % 5, currentHeight + 4);
                stonesSpawned++;
            }
            else stone = movedStone;
            
            steps++;
        }
        return (currentHeight, stonesSpawned - 1);
    }

    record Shape(Point[] Points)
    {
        public long OuterLeft => Points.Select(p => p.X).Min();
        public long OuterRight => Points.Select(p => p.X).Max();
        public long Lowest => Points.Select(p => p.Y).Min();

        public static Shape Create(int type, int y)
        {
            return type switch
            {
                0 => new (new[] {new Point(2, y), new Point(3, y), new Point(4, y), new Point(5, y)}),
                1 => new (new[] {new Point(2, y + 1), new Point(3, y), new Point(3, y + 1), new Point(3, y + 2), new Point(4, y + 1)}),
                2 => new (new[] {new Point(2, y), new Point(3, y), new Point(4, y), new Point(4, y + 1), new Point(4, y + 2)}),
                3 => new (new[] {new Point(2, y), new Point(2, y + 1), new Point(2, y + 2), new Point(2, y + 3)}),
                4 => new (new[] {new Point(2, y), new Point(3, y), new Point(2, y + 1), new Point(3, y + 1)}),
                _ => throw new ArgumentException("Bad type")
            };
        }

        public Shape Move(char direction)
        {
            return direction switch
            {
                '<' => this with { Points = Points.Select(p => p.Move(Direction.Left)).ToArray()},
                '>' => this with { Points = Points.Select(p => p.Move(Direction.Right)).ToArray()},
                'v' => this with { Points = Points.Select(p => p.Move(Direction.Down)).ToArray()},
                _ => throw new ArgumentException("Bad direction")
            };
        }
    }
}