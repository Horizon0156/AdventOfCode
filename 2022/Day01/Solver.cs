namespace AdventOfCode.Y2022.Day01;

[Problem("Calorie Counting", 2022, 1)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var calories = input.ChunkByBlankLine()
                            .Select(e => e.SplitLines()
                                          .Select(int.Parse)
                                          .Sum())
                            .OrderByDescending(s => s);

        return new (calories.ElementAt(0), calories.Take(3).Sum());                      
    }
}