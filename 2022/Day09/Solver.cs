namespace AdventOfCode.Y2022.Day09;

[Problem("Rope Bridge", 2022, 9)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var instructions = input.SplitLines()
                                .Select(Instruction.Parse);

        return new (
            CountDistinctTailPositions(instructions, 2),
            CountDistinctTailPositions(instructions, 10));
    }

    static int CountDistinctTailPositions(IEnumerable<Instruction> instructions, int ropeLength)
    {
        var rope = new Rope(ropeLength);
        var tailPositions = new HashSet<Point>();
        tailPositions.Add(rope.Tail);

        foreach (var instruction in instructions)
        {
            for (var i = 0; i < instruction.Steps; i++)
            {
                rope.Move(instruction.Direction);
                tailPositions.Add(rope.Tail);
            }
        }
        return tailPositions.Count;
    }

    record Instruction(string Direction, int Steps)
    {
        public static Instruction Parse(string instruction)
        {
            var parts = instruction.Split();
            return new (parts[0], int.Parse(parts[1]));
        }
    }

    class Rope
    {
        private List<Point> _knots;
        
        public Rope(int length)
        {
            _knots = new List<Point>(length);
            for (var i = 0; i < length; i++)
                _knots.Add(new Point(0, 0));
        }

        public Point Head => _knots.First();

        public Point Tail => _knots.Last();

        public void Move(string direction)
        {
            _knots[0] = direction switch 
            {
                "U" => Head with { Y = Head.Y + 1},
                "D" => Head with { Y = Head.Y - 1},
                "L" => Head with { X = Head.X - 1},
                "R" => Head with { X = Head.X + 1},
                _ => throw new ArgumentException("Bad direction")
            };

            for (var i = 1; i < _knots.Count; i++)
            {
                var forerunner = _knots[i - 1];
                var current = _knots[i];

                if (current.EuclideanDistance(forerunner) > 1.5)
                    _knots[i] = current.MoveTowards(forerunner, 1);
            }
        }
    }
}