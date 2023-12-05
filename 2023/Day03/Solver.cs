using System.Text.RegularExpressions;

using PositionalNumber = (int value, int xStart, int xEnd, int y);

namespace AdventOfCode.Y2023.Day03;

[Puzzle("Gear Ratios", 2023, 3)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var numbers = FindNumbers(input).ToArray();
        var partNumbers = FindSymbols(input)
            .SelectMany(s => s.Get8Neighbourhood())
            .Distinct()
            .SelectMany(s => numbers.Where(n => n.y == s.Y && n.xStart <= s.X && s.X <= n.xEnd))
            .Distinct();
        
        var gearRatios = FindSymbols(input, '*')
            .Select(g => g.Get8Neighbourhood()
                          .SelectMany(g => numbers.Where(n => n.y == g.Y && n.xStart <= g.X && g.X <= n.xEnd))
                          .Distinct())
            .Where(g => g.Count() == 2)
            .Select(g => g.First().value * g.Last().value);
            
        return new (partNumbers.Sum(n => n.value), gearRatios.Sum());
    }

    private static IEnumerable<PositionalNumber> FindNumbers(string input) => 
        input.SplitLines()
             .SelectMany((l, y) => 
                Regex.Matches(l, @"\d+")
                     .Select(m => new PositionalNumber(int.Parse(m.Value), m.Index, m.Index + m.Length - 1, y)));

    private static IEnumerable<Point> FindSymbols(string input, char? symbol = null) =>
        input.SplitLines()
             .SelectMany((l, y) => l.Select((v, x) => (v, x))
                                    .Where(c => symbol.HasValue ? c.v == symbol : !char.IsDigit(c.v) && c.v != '.')
                                    .Select(c => new Point(c.x, y)));
}