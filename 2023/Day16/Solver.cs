namespace AdventOfCode.Y2023.Day16;

using Beam = (Point point, Direction direction);

[Puzzle("The Floor Will Be Lava", 2023, 16)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = input.SplitLines().To2DArray();

        var startBeams = new List<Beam>();
        startBeams.AddRange(Enumerable.Range(0, map.GetWidth()).Select(x => new Beam(new (x, 0), Direction.Up)));
        startBeams.AddRange(Enumerable.Range(0, map.GetWidth()).Select(x => new Beam(new (x, map.GetHeight() - 1), Direction.Down)));
        startBeams.AddRange(Enumerable.Range(0, map.GetHeight()).Select(y => new Beam(new (0, y), Direction.Right)));
        startBeams.AddRange(Enumerable.Range(0, map.GetHeight()).Select(y => new Beam(new (map.GetWidth() - 1, y), Direction.Left)));

        return new (
            CountEnergizedCells(map, new(new(0, 0), Direction.Right)),
            startBeams.Select(b => CountEnergizedCells(map, b)).Max());
    }

    private int CountEnergizedCells(char[,] map, Beam firstBeam)
    {
        var queue = new Queue<Beam>([firstBeam]);
        var visited = new HashSet<Beam>();

        while (queue.TryDequeue(out var beam))
        {
            if (!map.InBounds(beam.point) || !visited.Add(beam)) continue;

            var tile = map[beam.point.Y, beam.point.X];

            if ((tile == '/' && beam.direction == Direction.Up)
             || (tile == '\\' && beam.direction == Direction.Down))
                queue.Enqueue(new (beam.point.Move(Direction.Left), Direction.Left));

            else if ((tile == '/' && beam.direction == Direction.Down)
             || (tile == '\\' && beam.direction == Direction.Up))
                queue.Enqueue(new (beam.point.Move(Direction.Right), Direction.Right));

            else if ((tile == '/' && beam.direction == Direction.Left)
             || (tile == '\\' && beam.direction == Direction.Right))
                queue.Enqueue(new (beam.point.Move(Direction.Up), Direction.Up));

            else if ((tile == '/' && beam.direction == Direction.Right)
             || (tile == '\\' && beam.direction == Direction.Left))
                queue.Enqueue(new (beam.point.Move(Direction.Down), Direction.Down));

            else if (tile == '|' && (beam.direction == Direction.Left || beam.direction == Direction.Right))
            {
                queue.Enqueue(new (beam.point.Move(Direction.Up), Direction.Up));
                queue.Enqueue(new (beam.point.Move(Direction.Down), Direction.Down));
            }
            else if (tile == '-' && (beam.direction == Direction.Up || beam.direction == Direction.Down))
            {
                queue.Enqueue(new (beam.point.Move(Direction.Left), Direction.Left));
                queue.Enqueue(new (beam.point.Move(Direction.Right), Direction.Right));
            }
            else
            {
                queue.Enqueue(new (beam.point.Move(beam.direction), beam.direction));
            }
        }
        return visited.Select(b => b.point).Distinct().Count();
    }
}