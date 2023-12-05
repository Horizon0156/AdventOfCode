namespace AdventOfCode.Y2022.Day08;

[Puzzle("Treetop Tree House", 2022, 8)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = input.SplitLines()
                       .Select(r => r.Select(r => r - '0'))
                       .To2DArray();
        
        var height = map.GetLength(0);
        var width = map.GetLength(1);
        var visibleTrees = (2 * height) + (2 * width) - 4;
        var bestScenicScore = 0;

        for (var y = 1; y < height - 1; y++) 
        {
            for (var x = 1; x < width - 1; x++) 
            {
                if (IsTreeVisible(map, x, y)) visibleTrees++;
                bestScenicScore = Math.Max(bestScenicScore, GetScenicScore(map, x, y));
            }
        }
        return new (visibleTrees, bestScenicScore);
    }

    static bool IsTreeVisible(int[,] map, int x, int y)
    {
        // Visible from left
        if (Enumerable.Range(0, x)
                      .Select(xl => map[y, xl])
                      .All(t => t < map[y, x])) return true;
        // Visible from top
        if (Enumerable.Range(x + 1, map.GetLength(1) - x - 1)
                      .Select(xr => map[y, xr])
                      .All(t => t < map[y, x])) return true;
        // Visible from right
        if (Enumerable.Range(0, y)
                      .Select(yt => map[yt, x])
                      .All(t => t < map[y, x])) return true;
        // Visible from bottom
        return Enumerable.Range(y + 1, map.GetLength(0) - y - 1)
                         .Select(yb => map[yb, x])
                         .All(t => t < map[y, x]);
    }

    static int GetScenicScore(int[,] map, int x, int y)
    {
        var leftScore = Math.Abs(Enumerable.Range(0, x)
                                           .Reverse()
                                           .FirstOrDefault(xl => xl == 0 || map[y, xl] >= map[y, x]) - x);
        var topScore = Math.Abs(Enumerable.Range(0, y)
                                          .Reverse()
                                          .FirstOrDefault(yt => yt == 0 || map[yt, x] >= map[y, x]) - y);
        var rightScore = Enumerable.Range(x + 1, map.GetLength(1) - x - 1)
                                   .FirstOrDefault(xr => xr == map.GetLength(1) - 1 || map[y, xr] >= map[y, x]) - x;
        var bottomScore = Enumerable.Range(y + 1, map.GetLength(0) - y - 1)
                                    .FirstOrDefault(yb => yb == map.GetLength(0) - 1 || map[yb, x] >= map[y, x]) - y;

        return leftScore * topScore * rightScore * bottomScore;
    }
}