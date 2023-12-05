namespace AdventOfCode.Y2023.Day01;

[Puzzle("Trebuchet?!", 2023, 1)]
internal class Solver : ISolver
{
    private static int Part1(string input) => 
        input.SplitLines()
             .Select(line => line.Where(c => Char.IsDigit(c)))
             .Select(digits => int.Parse(string.Concat(digits.First(), digits.Last())))
             .Sum();
    
    private static int Part2(string input) => Part1(
        input.Replace("one", "on1e")
             .Replace("two", "tw2o")
             .Replace("three", "thre3e")
             .Replace("four", "fou4r")
             .Replace("five", "fiv5e")
             .Replace("six", "si6x")
             .Replace("seven", "seve7n")
             .Replace("eight", "eigh8t")
             .Replace("nine", "nin9e"));

    public Solution Solve(string input) => new (Part1(input), Part2(input));
}