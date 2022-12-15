namespace AdventOfCode.Y2022.Day12;

[Problem("Hill Climbing Algorithm", 2022, 12)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = input.SplitLines().Select(l => l.ToCharArray()).To2DArray();
        var start = FindPoints(map, 'S').First();
        var end = FindPoints(map, 'E').First();
        map[start.Y, start.X] = 'a';
        map[end.Y, end.X] = 'z';

        return new (
            AStar(map, start, end),
            FindPoints(map, 'a').Select(s => AStar(map, s, end)).Where(c => c > 0).Min());
    }

    static IEnumerable<Point> FindPoints(char[,] map, char elevation) 
    {
        var points = new List<Point>();

        for (var y = 0; y < map.GetLength(0); y++)
            for (var x = 0; x < map.GetLength(1); x++)
                if (map[y, x] == elevation) points.Add(new (x, y));
        return points;
    }

    static IEnumerable<Point> Get4Neighboorhood(char[,] map, Point p)
    {
        var neighboors = new List<Point>();
        if (p.X - 1 >= 0) neighboors.Add(p with { X = p.X - 1});
        if (p.X + 1 < map.GetLength(1)) neighboors.Add(p with { X = p.X + 1,});
        if (p.Y - 1 >= 0) neighboors.Add(p with { Y = p.Y - 1});
        if (p.Y + 1 < map.GetLength(0)) neighboors.Add(p with { Y = p.Y + 1});

        var elevation = map[p.Y, p.X];
        return neighboors.Where(n => map[n.Y, n.X] <= elevation + 1);
    }

    static int AStar(char[,] map, Point start, Point end)
    {
        var openPoints = new HashSet<Point>() { start };
        var costsByNode = new Dictionary<Point, int>() {{ start, 0 }};
        var heuristicByNode = new Dictionary<Point, int>() {{ start, (int) start.EuclideanDistance(end) }};
        while (openPoints.Count > 0)
        {
            var nearestPoint = openPoints.OrderBy(p => heuristicByNode[p]).First();
            openPoints.Remove(nearestPoint);

            if (nearestPoint == end) return costsByNode[end];

            foreach (var n in Get4Neighboorhood(map, nearestPoint))
            {
                var newCosts = costsByNode[nearestPoint] + 1;
                if (!costsByNode.TryGetValue(new (n.X, n.Y), out var costs) || newCosts < costs)
                {
                    costsByNode[n] = newCosts;
                    heuristicByNode[n] = newCosts + (int) n.EuclideanDistance(end);
                    openPoints.Add(n);
                }
            }
        }
        return -1;
    }
}