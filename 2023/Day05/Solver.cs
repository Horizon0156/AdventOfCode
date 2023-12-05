namespace AdventOfCode.Y2023.Day05;

using MapEntry = (long from, long to, long offset);
using Range = (long from, long to);

[Puzzle("If You Give A Seed A Fertilizer", 2023, 5)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var inputSections = input.ChunkByBlankLine().ToArray();

        var seeds = inputSections[0].Split(" ").Skip(1).Select(long.Parse);
        var seedRanges = inputSections[0].Split(" ").Skip(1).Select(long.Parse)
                                         .Pairwise().Select(p => new Range(p.p1, p.p1 + p.p2 - 1));
        var mapChain = inputSections.Skip(1).Select(ParseMap).ToList();

        return new (
            seeds.Select(s => GetLocation(s, mapChain)).Min(),
            GetLocationRanges(seedRanges.ToList(), mapChain).Min(r => r.from));
    }

    private static long GetLocation(long seed, List<MapEntry[]> mapChain)
    {
        var location = seed;
        foreach(var map in mapChain) 
        {
            var effectiveMap = map.FirstOrDefault(m => location >= m.from && location <= m.to);
            location += effectiveMap != default ? effectiveMap.offset : 0; 
        }
        return location;
    }

    private static List<Range> GetLocationRanges(List<Range> seedRanges, List<MapEntry[]> mapChain)
    {
        foreach (var map in mapChain)
        {
            var locationRanges = new List<Range>();
            foreach (var range in seedRanges)
            {
                var mutableRange = range;
                foreach (var (mapFrom, mapTo, offset) in map.OrderBy(m => m.from))
                {
                    if (mutableRange.from < mapFrom) // Unmapped Range til next map section
                    {
                        locationRanges.Add((mutableRange.from, Math.Min(mutableRange.to, mapFrom - 1)));
                        mutableRange.from = mapFrom;
                        if (mutableRange.from > mutableRange.to)
                            break;
                    }

                    if (mutableRange.from <= mapTo) // Range til current's map section end 
                    {
                        locationRanges.Add((mutableRange.from + offset, Math.Min(mutableRange.to, mapTo) + offset));
                        mutableRange.from = mapTo + 1;
                        if (mutableRange.from > mutableRange.to)
                            break;
                    }
                }
                if (mutableRange.from <= mutableRange.to) // Missing unmapped range after all map sections
                    locationRanges.Add(mutableRange);
            }
            seedRanges = locationRanges;
        }
        return seedRanges;
    }

    private static MapEntry[] ParseMap(string map)
    {
        return map.SplitLines()
                  .Skip(1)
                  .Select(line =>
                   {
                        var parts = line.Split(" ").Select(long.Parse).ToArray();
                        return new MapEntry(parts[1], parts[1] + parts[2] - 1, parts[0] - parts[1]);
                   })
                   .ToArray();
    }
}