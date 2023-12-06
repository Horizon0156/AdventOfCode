using System.Text.RegularExpressions;

namespace AdventOfCode.Extensions;

internal static class StringExtensions
{
    public static IEnumerable<string> SplitLines(this string s) => s.Split(Environment.NewLine);

    public static IEnumerable<string> ChunkByBlankLine(this string s) =>
        s.Split($"{Environment.NewLine}{Environment.NewLine}");

    public static string[] Match(this string s, string regEx) =>
        Regex.Match(s, regEx).Groups.Values.Skip(1).Select(g => g.Value).ToArray();

    public static T[] Match<T>(this string s, string regEx) =>
        s.Match(regEx).Select(v => Convert.ChangeType(v, typeof(T))).Cast<T>().ToArray();

    public static string[] Matches(this string s, string regEx) =>
        Regex.Matches(s, regEx).Select(g => g.Value).ToArray();

    public static T[] Matches<T>(this string s, string regEx) =>
        s.Matches(regEx).Select(v => Convert.ChangeType(v, typeof(T))).Cast<T>().ToArray();
}