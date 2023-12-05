using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day22;

[Puzzle("Monkey Map", 2022, 22)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = new MonkeyMap(input);

        return new (map.Move(isCube: false), map.Move(isCube: true));
    }

    class MonkeyMap
    {
        private Dictionary<int, (int Min, int Max)> _xRangesByY;
        private Dictionary<int, (int Min, int Max)> _yRangesByX;
        private HashSet<Point> _stones;
        private List<Instruction> _instructions;

        public MonkeyMap(string input)
        {
            var lines = input.SplitLines().ToArray();

            _stones = new HashSet<Point>();
            _xRangesByY = new Dictionary<int, (int Min, int Max)>();
            _yRangesByX = new Dictionary<int, (int Min, int Max)>();
            ReadMap(lines[0..^2]);

            _instructions = ParseInstructions(lines[^1]);
        }

        public int Move(bool isCube)
        {
            var currentPosition = new Position(_xRangesByY[0].Min, 0, CardinalDirection.East);

            foreach (var instruction in _instructions)
            {
                if (instruction.Direction == RotationalDirection.None)
                {
                    for (var i = 0; i < instruction.Steps; i++)
                    {
                        var nextPosition = isCube 
                            ? GetNextPositionOnCube(currentPosition)
                            : GetNextPositionOnFlat(currentPosition);
                        if (_stones.Contains(new (nextPosition.X, nextPosition.Y))) break;
                        currentPosition = nextPosition;
                    }
                }
                else if (instruction.Direction == RotationalDirection.Left) 
                {
                    currentPosition = currentPosition with 
                    { 
                        Facing = (CardinalDirection) ExtendedMath.Modulo((int) currentPosition.Facing - 1, 4)
                    };   
                }
                else if (instruction.Direction == RotationalDirection.Right) 
                {
                    currentPosition = currentPosition with 
                    {
                        Facing = (CardinalDirection) ExtendedMath.Modulo((int) currentPosition.Facing + 1, 4)
                    };
                }
            }

            return (1000 * (currentPosition.Y + 1)) + (4 * (currentPosition.X + 1)) + (int) currentPosition.Facing;
        }

        Position GetNextPositionOnFlat(Position currentPosition)
        {
            var xRange = _xRangesByY[currentPosition.Y];
            var yRange = _yRangesByX[currentPosition.X];
            return currentPosition.Facing switch
            {
                CardinalDirection.East => currentPosition with
                {
                    X = currentPosition.X + 1 > xRange.Max ? xRange.Min : currentPosition.X + 1
                },
                CardinalDirection.South => currentPosition with
                {
                    Y = currentPosition.Y + 1 > yRange.Max ? yRange.Min : currentPosition.Y + 1
                },
                CardinalDirection.West => currentPosition with
                {
                    X = currentPosition.X - 1 < xRange.Min ? xRange.Max : currentPosition.X - 1
                },
                CardinalDirection.North => currentPosition with
                {
                    Y = currentPosition.Y - 1 < yRange.Min ? yRange.Max : currentPosition.Y - 1
                },
                _ => throw new ArgumentException("Bad direction")
            };
        }

        Position GetNextPositionOnCube(Position currentPosition)
        {
            var xRange = _xRangesByY[currentPosition.Y];
            var yRange = _yRangesByX[currentPosition.X];
            
            var nextPosition = currentPosition.Facing switch
            {
                CardinalDirection.East => currentPosition with { X = currentPosition.X + 1 },
                CardinalDirection.South => currentPosition with { Y = currentPosition.Y + 1 },
                CardinalDirection.West => currentPosition with { X = currentPosition.X - 1 },
                CardinalDirection.North => currentPosition with { Y = currentPosition.Y - 1 },
                _ => throw new ArgumentException("Bad direction")
            };

            var outOfBounds = nextPosition.X < xRange.Min || nextPosition.X > xRange.Max 
                           || nextPosition.Y < yRange.Min || nextPosition.Y > yRange.Max;

            if (!outOfBounds) return nextPosition;

            var segment = currentPosition.Y < 50
                            ? currentPosition.X < 100 ? 'A' : 'B'
                            : currentPosition.Y < 100
                                ? 'C'
                                : currentPosition.Y < 150
                                    ? currentPosition.X < 50 ? 'E' : 'D'
                                    : 'F';

            if (segment == 'A' && currentPosition.Facing == CardinalDirection.North)
                return currentPosition with { X = 0, Y = currentPosition.X + 100, Facing = CardinalDirection.East};
            if (segment == 'A' && currentPosition.Facing == CardinalDirection.West)
                return currentPosition with { X = 0, Y = 149 - currentPosition.Y, Facing = CardinalDirection.East};

            if (segment == 'B' && currentPosition.Facing == CardinalDirection.North)
                return currentPosition with { X = currentPosition.X - 100, Y = 199, Facing = CardinalDirection.North};
            if (segment == 'B' && currentPosition.Facing == CardinalDirection.East)
                return currentPosition with { X = 99, Y = 149 - currentPosition.Y, Facing = CardinalDirection.West};
            if (segment == 'B' && currentPosition.Facing == CardinalDirection.South)
                return currentPosition with { X = 99, Y = currentPosition.X - 50, Facing = CardinalDirection.West};

            if (segment == 'C' && currentPosition.Facing == CardinalDirection.East)
                return currentPosition with { X = currentPosition.Y + 50, Y = 49, Facing = CardinalDirection.North};
            if (segment == 'C' && currentPosition.Facing == CardinalDirection.West)
                return currentPosition with { X = currentPosition.Y - 50, Y = 100, Facing = CardinalDirection.South};

            if (segment == 'D' && currentPosition.Facing == CardinalDirection.East)
                return currentPosition with { X = 149, Y = 149 - currentPosition.Y, Facing = CardinalDirection.West};
            if (segment == 'D' && currentPosition.Facing == CardinalDirection.South)
                return currentPosition with { X = 49, Y = currentPosition.X + 100, Facing = CardinalDirection.West};

            if (segment == 'E' && currentPosition.Facing == CardinalDirection.North)
                return currentPosition with { X = 50, Y = 50 + currentPosition.X, Facing = CardinalDirection.East};
            if (segment == 'E' && currentPosition.Facing == CardinalDirection.West)
                return currentPosition with { X = 50, Y = 149 - currentPosition.Y, Facing = CardinalDirection.East};

            if (segment == 'F' && currentPosition.Facing == CardinalDirection.South)
                return currentPosition with { X = currentPosition.X +  100, Y = 0, Facing = CardinalDirection.South};
            if (segment == 'F' && currentPosition.Facing == CardinalDirection.East)
                return currentPosition with { X = currentPosition.Y - 100, Y = 149, Facing = CardinalDirection.North};
            if (segment == 'F' && currentPosition.Facing == CardinalDirection.West)
                return currentPosition with { X = currentPosition.Y - 100, Y = 0, Facing = CardinalDirection.South};
            
            throw new ArgumentException("Out of expected bounds");
        }

        void ReadMap(string[] input)
        {
            var map = new HashSet<Point>();

            for (var y = 0; y < input.Length; y++)
            {
                var minX = int.MaxValue;
                var maxX = 0;
                for (var x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == ' ') continue;
                    if (input[y][x] == '#') _stones.Add(new (x, y));
                    map.Add(new (x, y));
                    minX = Math.Min(minX, x);
                    maxX = Math.Max(maxX, x);
                }
                _xRangesByY[y] = (minX, maxX);
            }

            for (var x = 0; x <= map.Max(p => p.X); x++)
            {
                var inRow = map.Where(p => p.X == x).ToArray();
                _yRangesByX[x] = (inRow.Min(p => p.Y), inRow.Max(p => p.Y));
            }
        }

        static List<Instruction> ParseInstructions(string input) =>
            Regex.Split(input, "(L|R)")
                 .Select(i => i switch 
                              {
                                  "L" => new Instruction(0, RotationalDirection.Left),
                                  "R" => new Instruction(0, RotationalDirection.Right),
                                  _ => new Instruction(int.Parse(i), RotationalDirection.None)
                              })
                 .ToList();
    }

    record Position(int X, int Y, CardinalDirection Facing) : Point(X, Y);

    record Instruction(int Steps, RotationalDirection Direction);

    enum CardinalDirection
    {
        East,
        South,
        West,
        North
    }

    enum RotationalDirection
    {
        None,
        Left,
        Right
    }
}