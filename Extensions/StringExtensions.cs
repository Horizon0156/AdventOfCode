namespace AdventOfCode.Extensions;

internal static class StringExtensions
{
    public static IEnumerable<string> SplitLines(this string s) => s.Split(Environment.NewLine);

    public static IEnumerable<string> ChunkByBlankLine(this string s) => s.Split($"{Environment.NewLine}{Environment.NewLine}");
}