#!/usr/bin/env dotnet-script
#nullable enable

/************************************************
 * Advent of Code Day 9: Smoke Basin
 ***********************************************/
record Point(int X, int Y, int Value);

IEnumerable<Point> Get4Neighboorhood(int[][] data, Point p)
{
    var neighboors = new List<Point>();
    if (p.X - 1 >= 0) neighboors.Add(p with { X = p.X - 1, Value = data[p.Y][p.X - 1]});
    if (p.X + 1 < data[p.Y].Length) neighboors.Add(p with { X = p.X + 1, Value = data[p.Y][p.X + 1]});
    if (p.Y - 1 >= 0) neighboors.Add(p with { Y = p.Y - 1, Value = data[p.Y - 1][p.X]});
    if (p.Y + 1 < data.Length) neighboors.Add(p with { Y = p.Y + 1, Value = data[p.Y + 1][p.X]});

    return neighboors;
}

IEnumerable<Point> GetBasin(int[][] data, Point lowPoint)
{
    var basin = new HashSet<Point>() { lowPoint };
    
    Get4Neighboorhood(data, lowPoint)
        .Where(n => n.Value != 9 && n.Value > lowPoint.Value && !basin.Contains(n))
        .SelectMany(n => GetBasin(data, n))
        .ToList()
        .ForEach(b => basin.Add(b));

    return basin;
}

var data = File.ReadAllLines("Data/Day9.txt")
               .Select(y => y.Select(x => int.Parse(x.ToString())).ToArray())
               .ToArray();

// Part 1
var lowPoints = data.SelectMany((row, y) => row.Select((value, x) => new Point(x, y, value)))
                    .Where(p => Get4Neighboorhood(data, p).All(n => n.Value > p.Value));

var riskLevel = lowPoints.Select(p => p.Value + 1)
                         .Sum();

// Part 2
var basins = lowPoints.Select(p => GetBasin(data, p))
                      .OrderByDescending(b => b.Count());
var basinCount = basins.Take(3).Aggregate(1, (c, b) => c * b.Count());

Console.WriteLine($"Day 9 - Smoke Basin: {riskLevel}, {basinCount}");
