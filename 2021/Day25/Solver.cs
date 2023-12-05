namespace AdventOfCode.Y2021.Day25;

[Puzzle("Sea Cucumber", 2021, 25)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = ParseMap(input.SplitLines().ToArray());
        var moves = MoveToEnd(map);

        return new ("-", moves);
    }

    enum FieldState
    {
        Free,
        EastFacing, 
        SouthFacing
    }

    static FieldState[,] ParseMap(string[] data)
    {
        var height = data.Length;
        var width = data[0].Length;
        var map = new FieldState[height, width];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                map[y, x] = data[y][x] switch
                {
                    '.' => FieldState.Free,
                    '>' => FieldState.EastFacing,
                    'v' => FieldState.SouthFacing,
                    _ => throw new NotSupportedException("Invalid field")
                };
            }
        }
        return map;
    }

    int MoveToEnd(FieldState[,] map)
    {
        var numberOfSteps = 0;
        var hasMoved = true;
        var height = map.GetLength(0);
        var width = map.GetLength(1);

        while (hasMoved)
        {
            numberOfSteps++;
            hasMoved = false;
            var mapState = (FieldState[,]) map.Clone();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var nextX = x + 1 < width ? x + 1 : 0;
                    if (map[y, x] == FieldState.EastFacing
                    && map[y, nextX] == FieldState.Free)
                    {
                        mapState[y, x] = FieldState.Free;
                        mapState[y, nextX] = FieldState.EastFacing;
                        hasMoved = true;
                    }
                }
            }
            map = mapState;
            mapState = (FieldState[,]) map.Clone();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var nextY = y + 1 < height ? y + 1 : 0;
                    if (map[y, x] == FieldState.SouthFacing 
                    && map[nextY, x] == FieldState.Free)
                    {
                        mapState[y, x] = FieldState.Free;
                        mapState[nextY, x] = FieldState.SouthFacing;
                        hasMoved = true;
                    }
                }
            }
            map = mapState;
        }
        return numberOfSteps;
    }
}