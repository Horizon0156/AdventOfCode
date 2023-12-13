namespace AdventOfCode.Y2023.Day13;

[Puzzle("Point of Incidence", 2023, 13)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var patterns = input.ChunkByBlankLine()
                            .Select(p => p.SplitLines().To2DArray());

        return new (
            patterns.Sum(p => SummarizePatternNotes(p, withSmudge: false)),
            patterns.Sum(p => SummarizePatternNotes(p, withSmudge: true)));
    }

    public static int SummarizePatternNotes(char[,] pattern, bool withSmudge)
    {
        var height = pattern.GetLength(0);
        var width = pattern.GetLength(1);
        var smudges = withSmudge ? 1 : 0;

        for (var y = 1; y < height; y++)
        {
            var rowsToCompare = Math.Min(y, height - y);
            var matches = Enumerable.Range(1, rowsToCompare)
                                    .Sum(i => Enumerable.Range(0, width)
                                                        .Count(x => pattern[y - i, x] == pattern[y + i - 1, x]));
            var mirrorFound = matches == (rowsToCompare * width) - smudges;
            if (mirrorFound) return y * 100;
        }

        for (var x = 1; x < width; x++)
        {
            var columnsToCompare = Math.Min(x, width - x);
            var matches = Enumerable.Range(1, columnsToCompare)
                                    .Sum(i => Enumerable.Range(0, height)
                                                        .Count(y => pattern[y, x - i] == pattern[y, x + i - 1]));
            var mirrorFound = matches == (columnsToCompare * height) - smudges;
            if (mirrorFound) return x;
        }
        return 0;
    }
}