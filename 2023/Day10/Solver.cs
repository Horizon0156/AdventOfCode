namespace AdventOfCode.Y2023.Day10;

[Puzzle("Pipe Maze", 2023, 10)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = input.SplitLines().To2DArray();
        var start = map.FindPoints('S').Single();
        var loop = FindLoop(start, map);
        var part1 = loop.Count / 2;

        // var points = map.FindPoints('.').ToArray();
        var points = map.GetPoints().Except(loop);
        var isStartNorthFaced = (loop[1].X == start.X && loop[1].Y == start.Y - 1)
                             || (loop[^1].X == start.X && loop[^1].Y == start.Y - 1);
        char[] northfacedPipes = isStartNorthFaced ? ['|', 'L', 'J', 'S'] : ['|', 'L', 'J'];
        
        // Something that's inside or surrounded by the loop must have at least an edge to the left.
        // Counting left, northfaced edges will show the face of the polygon => odd = inside, even = outside.
        var xEdgesByY = loop.GroupBy(p => p.Y).ToDictionary(g => g.Key, g => g.Select(p => p.X));
        var enclosedPoints = points.Where(p => xEdgesByY.ContainsKey(p.Y) 
                                            && xEdgesByY[p.Y].Where(e => 
                                                e < p.X 
                                             && northfacedPipes.Contains(map[p.Y, e])).Count() % 2 != 0);

        return new (loop.Count() / 2, enclosedPoints.Count());
    }

    private static List<Point> FindLoop(Point startPosition, char[,] map)
    {
        List<Point> loopPositions = [startPosition];
        var nextPosition = FindConnectedPipe(map, startPosition, null);
        while (nextPosition != null)
        {
            if (nextPosition == startPosition)
                return loopPositions;
            loopPositions.Add(nextPosition);
            nextPosition = FindConnectedPipe(map, nextPosition, loopPositions[^2]);
        }
        throw new ArgumentException("Map does not have any loops");
    }

    private static Point? FindConnectedPipe(char[,] map, Point position, Point? previousPosition)
    {
        var currentPipe = map[position.Y, position.X];
        var up = position.Move(Direction.Up);
        var down = position.Move(Direction.Down);
        var left = position.Move(Direction.Left);
        var right = position.Move(Direction.Right);
        char[] accessibleFromNorth = ['|', 'L', 'J', 'S'];
        char[] accessibleFromSouth = ['|', '7', 'F', 'S'];
        char[] accessibleFromEast = ['-', 'L', 'F', 'S'];
        char[] accessibleFromWest = ['-', '7', 'J', 'S'];

        if (up.Y >= 0 && up != previousPosition
            && accessibleFromNorth.Contains(currentPipe)
            && accessibleFromSouth.Contains(map.ElementAt(up)))
            return up;
        if (down.Y < map.GetLength(0) && down != previousPosition
            && accessibleFromSouth.Contains(currentPipe)
            && accessibleFromNorth.Contains(map.ElementAt(down)))
            return down;
        if (left.X >= 0 && left != previousPosition
            && accessibleFromWest.Contains(currentPipe)
            && accessibleFromEast.Contains(map.ElementAt(left)))
            return left;
        if (right.X < map.GetLength(1) && right != previousPosition
            && accessibleFromEast.Contains(currentPipe)
            && accessibleFromWest.Contains(map.ElementAt(right)))
            return right;

        return null;
    }
}