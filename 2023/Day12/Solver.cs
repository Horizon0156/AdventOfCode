namespace AdventOfCode.Y2023.Day12;

[Puzzle("Hot Springs", 2023, 12)]
internal class Solver : ISolver
{
    private readonly Dictionary<string, long> _cache = [];

    public Solution Solve(string input)
    {
        var conditionRecords = input.SplitLines()
                                    .Select(ParseConditionRecord);

        var part1 = conditionRecords
                         .Select(r => GetArrangements(r.Conditions, r.ContiguousGroups))
                         .Sum();
        
        var part2 = conditionRecords
                         .Select(r => Unfold(r.Conditions, r.ContiguousGroups))
                         .Select(r => GetArrangements(r.Conditions, r.ContiguousGroups))
                         .Sum();

        return new (part1, part2);
    }

    private long GetArrangements(string conditions, int[] contiguousGroups) 
    {
        var key = $"{conditions} {string.Join(',', contiguousGroups)}";

        if (!_cache.TryGetValue(key, out var arrangements))
        {
            arrangements = FindArrangements(conditions, contiguousGroups);
            _cache[key] = arrangements;
        }
        return arrangements;
    }

    private long FindArrangements(string conditions, int[] contiguousGroups)
    {
        conditions = conditions.Trim('.');

        // No conditions? No arrangements!
        if (string.IsNullOrEmpty(conditions)) return 0; 

        if (conditions.StartsWith('?'))
            return GetArrangements("." + conditions[1..], contiguousGroups) 
                 + GetArrangements("#" + conditions[1..], contiguousGroups);
    
        // We have a #! Group not long enough or any dots in # group? No arrangement!
        if (conditions.Length < contiguousGroups[0] || conditions[..contiguousGroups[0]].Contains('.')) return 0; 

        if (contiguousGroups.Length > 1)
        {
            // Not enough conditions for next group or group followed by another #? No arrangement!
            if (conditions.Length < contiguousGroups[0] + 1 || conditions[contiguousGroups[0]] == '#') return 0;
            return GetArrangements(conditions[(contiguousGroups[0] + 1)..], contiguousGroups[1..]);
        }

        // No # after last group? Found an arrangement!
        return conditions[contiguousGroups[0]..].Contains('#') ? 0 : 1; 
    }

    private static (string Conditions, int[] ContiguousGroups) ParseConditionRecord(string conditionRecord)
    {
        var conditionRecordParts = conditionRecord.Split(" ");
        return (conditionRecordParts[0], conditionRecordParts[1].Split(',').Select(int.Parse).ToArray());
    }

    private static (string Conditions, int[] ContiguousGroups) Unfold(string conditions, int[] contiguousGroups)
    {
        return (
            string.Join('?', Enumerable.Repeat(conditions, 5)),
            Enumerable.Repeat(contiguousGroups, 5).SelectMany(g => g).ToArray()
        );
    }
}