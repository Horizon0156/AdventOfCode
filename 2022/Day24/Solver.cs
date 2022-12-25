namespace AdventOfCode.Y2022.Day24;

[Problem("Blizzard Basin", 2022, 24)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = Map.Parse(input.SplitLines().ToArray());

        var part1 = map.FindShortestWayThroughTheBlizzards(map.Start, map.End);
        var backToStart = map.FindShortestWayThroughTheBlizzards(map.End, map.Start, part1);
        var andToEndAgain = map.FindShortestWayThroughTheBlizzards(map.Start, map.End, part1 + backToStart);

        return new(part1, part1 + backToStart + andToEndAgain);
    }

    class Map
    {
        private readonly Blizzard[] _blizzards;
        private readonly Dictionary<int, Blizzard[]> _blizzardsByTime;
        private readonly Dictionary<int, HashSet<Point>> _occupiedPointsByTime;
        private readonly int _height;
        private readonly int _width;

        public Map(Blizzard[] blizzards, int height, int width, Point start, Point end)
        {
            _blizzards = blizzards;
            _height = height;
            _width = width;

            Start = start;
            End = end;

            _blizzardsByTime = new Dictionary<int, Blizzard[]>();
            _occupiedPointsByTime = new Dictionary<int, HashSet<Point>>();
            _blizzardsByTime[0] = _blizzards;
            _occupiedPointsByTime[0] = _blizzards.Select(b => new Point(b.X, b.Y)).ToHashSet();
        }

        public Point Start { get; }

        public Point End { get; }

        public static Map Parse(string[] lines)
        {
            var height = lines.Length;
            var width = lines[0].Length;
            var start= new Point(0, 0);
            var end = new Point(0, 0);

            var blizzards = new List<Blizzard>();

            for (var y =  0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (y == 0 && lines[y][x] ==  '.') start = new (x,  y);
                    if (y == height - 1 && lines[y][x] ==  '.') end = new (x,  y);
                    if (lines[y][x] == '^') blizzards.Add(new (x, y, Direction.Up));
                    else if (lines[y][x] == 'v') blizzards.Add(new (x, y, Direction.Down));
                    else if (lines[y][x] == '<') blizzards.Add(new (x, y, Direction.Left));
                    else if (lines[y][x] == '>') blizzards.Add(new (x, y, Direction.Right));
                }
            }
            return new (blizzards.ToArray(), height, width, start, end);
        }

        public int FindShortestWayThroughTheBlizzards(Point start, Point end, int currentTime = 0)
        {
            var openStates = new HashSet<(Point Position, int Time)>();
            var costsByState = new Dictionary<(Point Position, int Time), int>();
            var heuristicByState = new Dictionary<(Point Position, int Time), int>();
            
            var initalState = (start, currentTime);
            openStates.Add(initalState);
            costsByState[initalState] = 0;
            heuristicByState[initalState] = start.ManhattanDistance(end);

            while (openStates.Count > 0)
            {
                var nearestState = openStates.OrderBy(p => heuristicByState[p]).First();
                openStates.Remove(nearestState);

                if (nearestState.Position == end) return costsByState[nearestState];
                
                // Patterns repeat every width * height
                if (!_occupiedPointsByTime.TryGetValue(nearestState.Time + 1, out var nextOccupiedPoints))
                {
                    var nextBlizzards = _blizzardsByTime[nearestState.Time].Select(b => b.Move(_height, _width))
                                                                           .ToArray();
                    nextOccupiedPoints = nextBlizzards.Select(b => new Point(b.X, b.Y)).ToHashSet();
                    _blizzardsByTime[nearestState.Time + 1] = nextBlizzards;
                    _occupiedPointsByTime[nearestState.Time + 1] = nextOccupiedPoints;
                }
                
                foreach (var next in nearestState.Position
                                                .Get4Neighbourhood()
                                                .Concat(new [] { nearestState.Position })
                                                .Where(n => InBounds(n) && !nextOccupiedPoints.Contains(n)))
                {
                    var newState = (next, nearestState.Time + 1);
                    var newCosts = costsByState[nearestState] + 1;
                    if (!costsByState.TryGetValue(newState, out var costs) || newCosts < costs)
                    {
                        costsByState[newState] = newCosts;
                        heuristicByState[newState] = newCosts + next.ManhattanDistance(end);
                        openStates.Add(newState);
                    }
                }
            }
            return -1;
        }

        private bool InBounds(Point point) => 
            point == Start 
            || point == End 
            || (point.X > 0 && point.X < _width - 1 && point.Y > 0 &&  point.Y < _height - 1);
    }

    record Blizzard(int X, int Y, Direction Direction) : Point(X, Y)
    {
        public Blizzard Move(int mapHeight, int mapWidth)
        {
            return Direction switch
            {
                Direction.Up => this with { Y = Y - 1 > 0 ? Y - 1 : mapHeight - 2 },
                Direction.Down => this with { Y = Y + 1 < mapHeight - 1 ? Y + 1 : 1 },
                Direction.Left => this with { X = X - 1 > 0 ? X - 1 : mapWidth - 2 },
                Direction.Right => this with { X = X + 1 < mapWidth - 1 ? X + 1 : 1 },
                _ => throw new ArgumentException("Bad direction")
            };
        }
    }
}