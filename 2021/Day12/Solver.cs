namespace AdventOfCode.Y2021.Day12;

[Problem("Passage Pathing", 2021, 12)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {                
        var passages = input.SplitLines()
                            .Select(p => p.Split("-"))
                            .Select(p => new Passage(p[0], p[1]));

        var start = Cave.FromPassages(passages)
                        .Single(c => c.Id == "start");
        // Part 1
        var numberOfPaths = CountDistinctPathes(start);

        // Part 2
        var numberOfPaths2 = CountDistinctPathes(start, timeForASecondVisit: true);

        return new (numberOfPaths, numberOfPaths2);
    }

    int CountDistinctPathes(Cave start, bool timeForASecondVisit = false)
    {
        var numberOfPathes = 0;

        var toVisit = new Stack<Step>();
        toVisit.Push(new (start, new List<Cave>() { start }, timeForASecondVisit));

        while (toVisit.Any())
        {
            var step = toVisit.Pop();
            if (step.Position.Id == "end")
            {
                numberOfPathes++;
                continue;
            }

            step.Position
                .ConnectedCaves
                .Where(c => !step.VisitedSmallCaves.Contains(c) 
                        || (step.TimeForASecondVisit && c.IsSmall && c.Id != "start"))
                .ToList()
                .ForEach(c => 
                {
                    var visitedSmallCaves = c.IsSmall
                        ? step.VisitedSmallCaves.Concat(new List<Cave>() { c })
                        : step.VisitedSmallCaves;
                        
                    toVisit.Push(
                        new (
                            c,
                            visitedSmallCaves.ToList(),
                            timeForASecondVisit && !visitedSmallCaves.GroupBy(c => c.Id)
                                                                    .Any(g => g.Count() > 1)));
                });
        }
        
        return numberOfPathes;
    }

    record Passage(string A, string B);

    record Step(Cave Position, List<Cave> VisitedSmallCaves, bool TimeForASecondVisit);

    class Cave
    {
        public string Id { get; private set; }

        public bool IsSmall => Id.All(c => char.IsLower(c));

        public List<Cave> ConnectedCaves { get; private set; }

        public Cave(string id)
        {
            Id = id;
            ConnectedCaves = new List<Cave>();
        }

        public static IEnumerable<Cave> FromPassages(IEnumerable<Passage> passages)
        {
            var cavesById = new Dictionary<string, Cave>();

            foreach(var passage in passages)
            {
                if (!cavesById.TryGetValue(passage.A, out var a))
                {
                    a = new Cave(passage.A);
                    cavesById.Add(passage.A, a);
                }
                if (!cavesById.TryGetValue(passage.B, out var b))
                {
                    b = new Cave(passage.B);
                    cavesById.Add(passage.B, b);
                }
                a.ConnectedCaves.Add(b);
                b.ConnectedCaves.Add(a);
            }
            return cavesById.Values;
        }
    }
}