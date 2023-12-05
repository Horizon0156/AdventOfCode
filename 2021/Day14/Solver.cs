namespace AdventOfCode.Y2021.Day14;

[Puzzle("Extended Polymerization", 2021, 14)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var data = input.SplitLines();
        var polymerTemplate = data.First();
        var insertionByPair = data.Skip(2)
                                .Select(d => d.Split(" -> "))
                                .ToDictionary(i => i[0], i => i[1]);

        var countByPair = insertionByPair.ToDictionary(p => p.Key, _ => (long) 0);
        Enumerable.Range(0, polymerTemplate.Length - 1)
                .ToList()
                .ForEach(i => countByPair[polymerTemplate.Substring(i, 2)]++);

        Enumerable.Range(0, 40)
                .ToList()
                .ForEach(_ => countByPair = TransformPolymer(countByPair, insertionByPair));

        var countByChar = countByPair.Select(kv => (kv.Key[0], kv.Value))
                                    .GroupBy(t => t.Item1)
                                    .ToDictionary(g => g.Key, g => g.Sum(t => t.Value));

        // The last char of the template is the last char of every polymer
        countByChar[polymerTemplate.Last()]++;

        return new ("-", countByChar.Max(kv => kv.Value) - countByChar.Min(kv => kv.Value));
    }

    Dictionary<string, long> TransformPolymer(
    Dictionary<string, long> countByPair,
    Dictionary<string, string> insertionByPair)
    {
        var newCountByPair = countByPair.ToDictionary(kv => kv.Key, _ => (long) 0);
        
        foreach(var pair in countByPair.Where(p => p.Value > 0))
        {
            var insertion = insertionByPair[pair.Key];
            newCountByPair[pair.Key[0] + insertion] += pair.Value;
            newCountByPair[insertion + pair.Key[1]] += pair.Value;
        }
        
        return newCountByPair;
    }
}