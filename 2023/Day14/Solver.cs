namespace AdventOfCode.Y2023.Day14;

[Puzzle("Parabolic Reflector Dish", 2023, 14)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = input.SplitLines().To2DArray();

        return new (
            GetWeight(TiltNorth(map)),
            GetWeight(TiltCycles(map, 1000000000)));
    }

    private static int GetWeight(char[,] map) =>
        Enumerable.Range(0, map.GetHeight())
                  .Sum(y => Enumerable.Range(0, map.GetWidth())
                                      .Count(x => map[y, x] == 'O') * (map.GetHeight() - y));

    private static char[,] RotateLeft(char[,] map)
    {
        var height = map.GetHeight();
        var width = map.GetWidth();
        var rotatedMap = new char[width, height];

        for (var y = 0; y < height; y++) 
            for (var x = 0; x < width; x++) 
                rotatedMap[y, x] = map[width - x - 1, y];

        return rotatedMap;
    }

    private static char[,] TiltCycles(char[,] map, int cycles)
    {
        var history = new List<string>();

        while (cycles > 0) 
        {
            for (var i = 0; i < 4; i++) map = RotateLeft(TiltNorth(map));
            cycles--;
            var printedMap = map.PrintMap();
            var cycleIndex = history.IndexOf(printedMap);
            if (cycleIndex != -1)
            {
                var cycleLength = history.Count - cycleIndex;
                var remainder = cycles % cycleLength; 
                return history[cycleIndex + remainder].SplitLines().To2DArray();
            }
            history.Add(printedMap);
        }
        return map;
    }

    private static char[,] TiltNorth(char[,] map)
    {
        var height = map.GetLength(0);
        var width = map.GetLength(1);

        for (var x = 0; x < width; x++) 
        {
            var newY = 0;
            for (var y = 0; y < height; y++) 
            {
                if (map[y, x] == '#') newY = y + 1;
                else if (map[y, x] == 'O') 
                {
                    map[y, x] = '.'; 
                    map[newY, x] = 'O'; 
                    newY++;
                }
            }
        }
        return map;
    }
}