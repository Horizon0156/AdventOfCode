#!/usr/bin/env dotnet-script
#nullable enable

class Scanner
{
    public Vector? Position { get; set; }
    public HashSet<Vector> ScanResults { get; set; } = new HashSet<Vector>();

    public int? ManhattanDistance(Scanner b) => 
        Position.HasValue && b.Position.HasValue 
            ? Position.Value.ManhattanDistance(b.Position.Value)
            : null;
}

record struct Vector(int X, int Y, int Z)
{
    public Vector Add(Vector b) => new (X + b.X, Y + b.Y, Z + b.Z);

    public int ManhattanDistance(Vector b)
    {
        var c = Subtract(b);
        return Math.Abs(c.X) + Math.Abs(c.Y) + Math.Abs(c.Z);
    }

    public Vector Subtract(Vector b) => new (X - b.X, Y - b.Y, Z - b.Z);

    public Vector Rotate(Vector ordinate, int rotation) 
    {
        Vector transformedOrdinate = ordinate switch
        {
            (1, 0, 0) => new (Y, X, -Z),
            (-1, 0, 0) => new (Y, -X, Z),
            (0, 1, 0) => new (X, Y, Z),
            (0, -1, 0) => new (X, -Y, -Z),
            (0, 0, 1) => new (Y, Z, X),
            (0, 0, -1) => new (Y, -Z, -X),
            _ => throw new ArgumentException(nameof(ordinate))
        };

        return rotation switch
        {
            0 => transformedOrdinate,
            1 => new (transformedOrdinate.Z, transformedOrdinate.Y, -transformedOrdinate.X),
            2 => new (-transformedOrdinate.X, transformedOrdinate.Y, -transformedOrdinate.Z),
            3 => new (-transformedOrdinate.Z, transformedOrdinate.Y, transformedOrdinate.X),
            _ => throw new ArgumentException(nameof(rotation))
        };
    }
}

List<Scanner> LoadData(string[] data)
{
    var scanner = new List<Scanner>();
    Scanner? currentScanner = null;

    for (var i = 0; i < data.Length; i++)
    {
        var currentLine = data[i];

        if (string.IsNullOrEmpty(currentLine)) continue;
        if (currentLine.StartsWith("---"))
        {
            if (currentScanner != null) scanner.Add(currentScanner);
            currentScanner = new Scanner();
            continue;
        }
        var points = currentLine.Split(",").Select(int.Parse).ToArray();
        currentScanner?.ScanResults?.Add(new Vector(points[0], points[1], points[2]));
    }
    if (currentScanner != null) scanner.Add(currentScanner);
    
    return scanner;
}

(Vector translation, HashSet<Vector> alignedScan)? AlignScan(
    HashSet<Vector> referenceScan,
    HashSet<Vector> scanToAlign)
{
    // For every ordinate (X, Y, Z facing either up or down)
    // we have to rotate 4 times (360°) to cover all possible orientations
    var ordinates = new Vector[] 
    {
        new (1, 0, 0),
        new (-1, 0, 0),
        new (0, 1, 0),
        new (0, -1, 0),
        new (0, 0, 1),
        new (0, 0, -1)
    };

    foreach (var ordinate in ordinates)
    {
        for (int rotation = 0; rotation < 4; rotation++)
        {
            var rotatedScans = scanToAlign.Select(b => b.Rotate(ordinate, rotation)).ToHashSet();
            foreach (var scan in referenceScan)
            {
                foreach (var rotatedScan in rotatedScans)
                {
                    var translation = scan.Subtract(rotatedScan);
                    var transformedScan = rotatedScans.Select(s => s.Add(translation)).ToHashSet();
                    if (transformedScan.Intersect(referenceScan).Count() >= 12) 
                    {
                        return (translation, transformedScan);
                    }
                }
            }
        }
    }
    return null;
}

bool LocateScanners(List<Scanner> scanners, HashSet<Vector> knownBeacons)
{
    foreach (var scanner in scanners)
    {
        if (scanner.Position != null) continue;

        var result = AlignScan(knownBeacons, scanner.ScanResults);
        if (result != null)
        {
            scanner.Position = result.Value.translation;
            scanner.ScanResults = result.Value.alignedScan;
            knownBeacons.UnionWith(result.Value.alignedScan);
        }
    }
    return scanner.All(s => s.Position != null);
}

var scanner = LoadData(File.ReadAllLines("Data/Day19.txt").ToArray());

scanner.First().Position = new (0, 0, 0);
var knownBeacons = new HashSet<Vector>(scanner.First().ScanResults);
while (!LocateScanners(scanner, knownBeacons));

var beacons = knownBeacons.Count();
var maxDistance = scanner.SelectMany(s1 => scanner.Select(s2 => s1.ManhattanDistance(s2)))
                         .Max();
    
Console.WriteLine("Day 19: Beacon Scanner");
Console.WriteLine($"{beacons}, {maxDistance}");