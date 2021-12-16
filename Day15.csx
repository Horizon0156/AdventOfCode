#!/usr/bin/env dotnet-script
#nullable enable

/*************************************************
 * Advent of Code Day 15: Chiton
 ************************************************/

record Point(int X, int Y);

IEnumerable<Point> Get4Neighboorhood(int[][] map, Point p)
{
    var neighboors = new List<Point>();
    if (p.X - 1 >= 0) neighboors.Add(p with { X = p.X - 1});
    if (p.X + 1 < map[p.Y].Length) neighboors.Add(p with { X = p.X + 1,});
    if (p.Y - 1 >= 0) neighboors.Add(p with { Y = p.Y - 1});
    if (p.Y + 1 < map.Length) neighboors.Add(p with { Y = p.Y + 1});

    return neighboors;
}

int LinearDistance(Point a, Point b) 
    => (int) Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));

int AStar(int[][] map, Point start, Point end)
{
    var openPoints = new HashSet<Point>() { start };
    var costsByNode = new Dictionary<Point, int>() {{ start, 0 }};
    var heuristicByNode = new Dictionary<Point, int>() {{ start, LinearDistance(start, end) }};
    while (openPoints.Count > 0)
    {
        var nearestPoint = openPoints.OrderBy(p => heuristicByNode[p]).First();
        openPoints.Remove(nearestPoint);

        if (nearestPoint == end) return costsByNode[end];

        foreach (var n in Get4Neighboorhood(map, nearestPoint))
        {
            var newCosts = costsByNode[nearestPoint] + map[n.Y][n.X];
            if (!costsByNode.TryGetValue(new (n.X, n.Y), out var costs) || newCosts < costs)
            {
                costsByNode[n] = newCosts;
                heuristicByNode[n] = newCosts + LinearDistance(n, end);
                openPoints.Add(n);
            }
        }
    }
    return -1;
}

int[][] ResizeMap(int[][] map, int factor)
{
    var width = map[0].Length;
    var height = map.Length;
    var newMap = new int[height * factor][];
    
    for (var y = 0; y < newMap.Length; y++)
    {
        newMap[y] = new int[width * factor];
        for (var x = 0; x < newMap[y].Length; x++)
        {
            var newValue = map[y % height][x % width] + (y / height) + (x / width);
            newMap[y][x] = newValue <= 9 ? newValue : newValue - 9;
        }
    }
    return newMap;
}

var map = File.ReadAllLines("Data/Day15.txt")
               .Select(y => y.Select(x => int.Parse(x.ToString())).ToArray())
               .ToArray();

var part1 = AStar(map, new Point(0, 0), new Point(map.Length - 1, map[map.Length - 1].Length - 1));

var biggerMap = ResizeMap(map, 5);
var part2 = AStar(biggerMap, new Point(0, 0), new Point(biggerMap[biggerMap.Length - 1].Length - 1, biggerMap.Length - 1));

Console.WriteLine($"Day 15 - Chiton: {part1}, {part2}");
