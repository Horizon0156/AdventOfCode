namespace AdventOfCode.Y2022.Day17;

[Problem("Pyroclastic Flow", 2022, 17)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var jetDirections = input.ToCharArray();

        return new(
            DropStones(2022, jetDirections).Height,
            DropStones(1000000000000, jetDirections).Height);
    }

    static (long Height, long Stones) DropStones(long stones, char[] jetDirections)
    {
        var chamber = Enumerable.Range(0, 7).Select(x => new Point(x, 0)).ToHashSet();
        var currentHeight = 0;
        var stone = Shape.Create(0, currentHeight + 4);
        var stonesDropped = 0L;
        var steps = 0;
        var stateCache = new Dictionary<string, (long Height, long Stones)>();
        var patternApplied = false;
        var cycledHeight = 0L;
        while (stonesDropped < stones)
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
                stonesDropped++;
    
                if (!patternApplied)
                {
                    var state = DrawChamber(chamber, currentHeight - 30);
                    if (stateCache.TryGetValue(state, out var pattern))
                    {
                        var heightPerCycle = currentHeight - pattern.Height;
                        var stonesPerCycle = stonesDropped - pattern.Stones;
                        var remainingStones = stones - stonesDropped;
                        var cyclesToApply = (stones - stonesDropped) / stonesPerCycle;
                        stonesDropped += cyclesToApply * stonesPerCycle;
                        cycledHeight += cyclesToApply * heightPerCycle;
                        patternApplied = true;
                    }
                    else stateCache[state] = (currentHeight, stonesDropped);
                }
                stone = Shape.Create((int) (stonesDropped % 5), currentHeight + 4);
            }
            else stone = movedStone;
            
            steps++;
        }
        return (currentHeight + cycledHeight, stonesDropped);
    }

    static string DrawChamber(HashSet<Point> chamber, int startAt)
    {
        var drawing = string.Empty;
        var currentHeight = chamber.Max(p => p.Y);
        startAt = startAt < 0 ? 0 : startAt;
        for (var y = currentHeight; y >= startAt; y--)
        {
            drawing += "|";
            for (var x = 0; x < 7; x++)
                drawing += chamber.Contains(new (x, y)) ? "#" : " ";
            drawing += "|" + Environment.NewLine;
        }
        return drawing;
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