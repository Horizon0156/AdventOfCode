namespace AdventOfCode.Y2022.Day23;

[Puzzle("Unstable Diffusion", 2022, 23)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var elves = input.SplitLines()
                         .SelectMany((row, y) => row.Select((column, x) => (column, x))
                                                    .Where(c => c.column == '#')
                                                    .Select(c => new Point(c.x, y)))
                         .ToHashSet();        

        return new (
            GetCoveredArea(MoveElves(elves.ToHashSet(), 10, stopWhenAllAreIdle: false).Elves),
            MoveElves(elves, int.MaxValue, stopWhenAllAreIdle: true).Rounds);
    }

    int GetCoveredArea(HashSet<Point> elves)
    {
        var width = elves.Max(e => e.X) - elves.Min(e => e.X) + 1;
        var height = elves.Max(e => e.Y) - elves.Min(e => e.Y) + 1;

        return (width * height) - elves.Count;
    }

    (HashSet<Point> Elves, int Rounds) MoveElves(HashSet<Point> elves, int rounds, bool stopWhenAllAreIdle)
    {
        var round = 0;
        var options = new [] 
        {
            new Option(
                (neighbours, elve) => neighbours.Where(n => n.Y == elve.Y - 1).All(n => !elves.Contains(n)),
                elve => elve with {Y = elve.Y - 1}),
            new Option(
                (neighbours, elve) => neighbours.Where(n => n.Y == elve.Y + 1).All(n => !elves.Contains(n)),
                elve => elve with {Y = elve.Y + 1}),
            new Option(
                (neighbours, elve) => neighbours.Where(n => n.X == elve.X - 1).All(n => !elves.Contains(n)),
                elve => elve with {X = elve.X - 1}),
            new Option(
                (neighbours, elve) => neighbours.Where(n => n.X == elve.X + 1).All(n => !elves.Contains(n)),
                elve => elve with {X = elve.X + 1}),
        };
        
        while (round < rounds)
        {
            // First phase
            var idleElves = new List<Point>();
            var proposals = new List<(Point Current, Point Desired)>();
            foreach (var elve in elves)
            {
                var neighbours = elve.Get8Neighbourhood().ToArray();
                if (neighbours.All(n => !elves.Contains(n)))
                {
                    idleElves.Add(elve);
                    continue;
                }
                for (var i = 0; i < options.Length; i++)
                {
                    var iRound = (i + round) % options.Length;
                    if (options[iRound].CheckNeighboars(neighbours, elve))
                    {
                        proposals.Add((elve, options[iRound].Move(elve)));
                        goto EndOfCurrentElve;
                    }
                }
                idleElves.Add(elve);
                EndOfCurrentElve: ;
            }

            // Second phase
            var proposalsByDesire = proposals.GroupBy(a => a.Desired);
            var movedElves = proposalsByDesire.Where(g => g.Count() == 1).Select(g => g.First().Desired).ToArray();
            if (movedElves.Length ==  0 && stopWhenAllAreIdle) return (elves, round + 1);
            var stuckedElves = proposalsByDesire.Where(g => g.Count() != 1).SelectMany(g => g.Select(a => a.Current));
            elves = idleElves.Concat(movedElves).Concat(stuckedElves).ToHashSet();
            round++;
        }
        return (elves, rounds + 1);
    }

    record Option(Func<Point[], Point, bool> CheckNeighboars, Func<Point, Point> Move);
}