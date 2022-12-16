namespace AdventOfCode.Y2022.Day16;

[Problem("Proboscidea Volcanium", 2022, 16)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var valvesById = input.SplitLines()
                              .Select(Valve.Parse)
                              .ToDictionary(v => v.Id);

        // Part 1: 1617 - Part 2: 2171
        return new (FindMaxFlowRate(valvesById, 1), FindMaxFlowRate(valvesById, 2));
    }

    static int CalculateCosts(Dictionary<string, Valve> valves, Valve start, Valve goal)
    {
        // Simple breadth first search
        var queue = new Queue<(Valve Valve, int Costs)>();
        var visited = new HashSet<Valve>();
        queue.Enqueue((start, 0));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            visited.Add(current.Valve);
            if (current.Valve == goal) return current.Costs;
            current.Valve
                   .ConnectsTo
                   .Select(v => valves[v])
                   .Where(v => !visited.Contains(v))
                   .ForEach(v => queue.Enqueue((v, current.Costs + 1)));
        }
        return -1;
    }

    static int FindMaxFlowRate(Dictionary<string, Valve> valvesById, int numberOfPlayers)
    {
        var interestingValves = valvesById.Values.Where(v => v.FlowRate > 0).ToHashSet();
        var flowByCacheKey = new Dictionary<string, int>();
        var distanceByIds = new Dictionary<(string From, string To), int>();
        var start = valvesById["AA"];

        if (numberOfPlayers == 1)
            return FindMaxFlowRate(start, interestingValves, 30, valvesById, flowByCacheKey, distanceByIds);

        return SplitValves(interestingValves.ToArray())
                .Select(v => FindMaxFlowRate(start, v.Player1, 26, valvesById, flowByCacheKey, distanceByIds)
                           + FindMaxFlowRate(start, v.Player2, 26, valvesById, flowByCacheKey, distanceByIds))
                .Max();
    }

    static int FindMaxFlowRate(
        Valve currentValve,
        HashSet<Valve> interestingValves,
        int remainingTime, 
        Dictionary<string, Valve> valvesById, 
        Dictionary<string, int> flowByCacheKey,
        Dictionary<(string From, string To), int> distanceByIds) 
    {
        var cacheKey = $"{remainingTime}-{currentValve.Id}-" +
                       $"{string.Join("-", interestingValves.OrderBy(x => x.Id).Select(x => x.Id))}";

        if (!flowByCacheKey.ContainsKey(cacheKey)) 
        {
            var flowFromCurrent = currentValve.FlowRate * remainingTime;
            var flowFromRest = 0;

            foreach (var valve in interestingValves.ToArray()) 
            {
                if (!distanceByIds.TryGetValue((currentValve.Id, valve.Id), out var distance))
                {
                    distance = CalculateCosts(valvesById, currentValve, valve);
                    distanceByIds[(currentValve.Id, valve.Id)] = distance;
                    distanceByIds[(valve.Id, currentValve.Id)] = distance;
                }

                if (remainingTime >= distance + 1) 
                {
                    interestingValves.Remove(valve);
                    remainingTime -= distance + 1;

                    flowFromRest = Math.Max(
                        flowFromRest,
                        FindMaxFlowRate(valve, interestingValves, remainingTime, valvesById, flowByCacheKey, distanceByIds));

                    remainingTime += distance + 1;
                    interestingValves.Add(valve);
                }
            }
            flowByCacheKey[cacheKey] = flowFromCurrent + flowFromRest;
        }
        return flowByCacheKey[cacheKey];
    }

    static IEnumerable<(HashSet<Valve> Player1, HashSet<Valve> Player2)> SplitValves(Valve[] valves) 
    {
        var maskLength = 1 << (valves.Length - 1);

        for (var mask = 0; mask < maskLength; mask++) 
        {
            var player1 = new HashSet<Valve>();
            var player2 = new HashSet<Valve>();
            player2.Add(valves[0]);

            for (var i = 1; i < valves.Length; i++) 
            {
                if ((mask & (1 << i)) == 0) player1.Add(valves[i]);
                else player2.Add(valves[i]);
            }
            yield return (player1, player2);
        }
    }

    record Valve(string Id, int FlowRate, string[] ConnectsTo)
    {
        public static Valve Parse(string value)
        {
            var match = value.Match(@"Valve (..) has flow rate=(\d*); tunnels? leads? to valves? (.*)");
            return new (match[0], int.Parse(match[1]), match[2].Split(", ").ToArray());
        }
    }
}