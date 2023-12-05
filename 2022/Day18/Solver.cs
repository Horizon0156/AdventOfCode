namespace AdventOfCode.Y2022.Day18;

[Puzzle("Boiling Boulders", 2022, 18)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var droplets = input.SplitLines().Select(Point3D.Parse).ToHashSet();
        var part1 = droplets.SelectMany(d => GetNeighbours(d)).Where(n => !droplets.Contains(n)).Count();
        
        var flooded = Flood(droplets);
        var part2 = droplets.SelectMany(d => GetNeighbours(d)).Where(n => flooded.Contains(n)).Count();

        return new (part1, part2);
    }

    HashSet<Point3D> Flood(HashSet<Point3D> droplets) 
    {
        var flooded = new HashSet<Point3D>();
        var queue = new Queue<Point3D>();
        var virtualBorder = 
        (
            new Point3D(droplets.Min(d => d.X) - 1, droplets.Min(d => d.Y) - 1, droplets.Min(d => d.Z) - 1),
            new Point3D(droplets.Max(d => d.X) + 1, droplets.Max(d => d.Y) + 1, droplets.Max(d => d.Z) + 1)
        );

        flooded.Add(virtualBorder.Item1);
        queue.Enqueue(virtualBorder.Item1);

        while (queue.Count > 0) 
        {
            var current = queue.Dequeue();
            foreach (var neighbour in GetNeighbours(current)) 
            {
                if (!flooded.Contains(neighbour)
                 && !droplets.Contains(neighbour)
                 && neighbour.X >= virtualBorder.Item1.X && neighbour.X <= virtualBorder.Item2.X
                 && neighbour.Y >= virtualBorder.Item1.Y && neighbour.Y <= virtualBorder.Item2.Y
                 && neighbour.Z >= virtualBorder.Item1.Z && neighbour.Z <= virtualBorder.Item2.Z)
                {
                    flooded.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }
        }
        return flooded;
    }

    IEnumerable<Point3D> GetNeighbours(Point3D droplet)
    {
        yield return droplet with {X = droplet.X - 1};
        yield return droplet with {X = droplet.X + 1};
        yield return droplet with {Y = droplet.Y - 1};
        yield return droplet with {Y = droplet.Y + 1};
        yield return droplet with {Z = droplet.Z - 1};
        yield return droplet with {Z = droplet.Z + 1};
    }

    record Point3D(int X, int Y, int Z)
    {
        public static Point3D Parse(string value)
        {
            var parts = value.Split(",");
            return new (int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }
        
    }
}