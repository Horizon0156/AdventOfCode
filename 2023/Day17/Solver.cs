namespace AdventOfCode.Y2023.Day17;

[Puzzle("Clumsy Crucible", 2023, 17)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = input.SplitLines().To2DArray();
        return new (FindShortestPath(map, 0, 3), FindShortestPath(map, 4, 10));
    }

    private static int FindShortestPath(char[,] map, int minimalMovements, int straightMovements)
    {
        // Classic Dijkstra implementation with special case that we have to track 
        // three transitions into the same direction. 
        var queue = new PriorityQueue<Crucible, int>();
        queue.Enqueue(new (new (0, 0), Direction.Right, 0), 0);
        queue.Enqueue(new (new (0, 0), Direction.Up, 0),  0);

        var history = new HashSet<Crucible>();
        var destination = new Point(map.GetWidth() - 1, map.GetHeight() - 1);

        while (queue.TryDequeue(out var crucible, out var heatloss))
        {
            if (crucible.Position == destination && crucible.StraighMoves > minimalMovements) return heatloss;

            foreach (var nextCrucible in GetConnectedNodes(crucible, minimalMovements, straightMovements))
            {
                if (map.InBounds(nextCrucible.Position) && !history.Contains(nextCrucible))
                {
                    history.Add(nextCrucible);
                    queue.Enqueue(nextCrucible, heatloss + map[nextCrucible.Position.Y, nextCrucible.Position.X] - '0');
                }
            }
        }
        return -1;
    }
    private static IEnumerable<Crucible> GetConnectedNodes(Crucible crucible, int minimalMovements, int straightMovements)
    {
        if (crucible.StraighMoves < straightMovements)
            yield return crucible with { Position = crucible.Position.Move(crucible.Alignment), StraighMoves = crucible.StraighMoves + 1 };
        
        if (crucible.StraighMoves >= minimalMovements)
        {
            yield return new (crucible.Position.Move(crucible.Alignment.TurnLeft()), crucible.Alignment.TurnLeft(), 1);
            yield return new (crucible.Position.Move(crucible.Alignment.TurnRight()), crucible.Alignment.TurnRight(), 1);
        }
    }

    private sealed record Crucible(Point Position, Direction Alignment, int StraighMoves);
}