namespace AdventOfCode.Y2023.Day19;

[Puzzle("Aplenty", 2023, 19)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var inputParts = input.ChunkByBlankLine().ToArray();
        var workflowsByName = inputParts[0].SplitLines()
                                           .Select(Workflow.Parse)
                                           .ToDictionary(k => k.Name);
        var ratings = inputParts[1].SplitLines().Select(Rating.Parse);
        
        return new (
            ratings.Where(r => r.IsAccepted(workflowsByName)).Sum(w => w.Sum()),
            FindDistinctAcceptableCombinations(ratings.First(r => r.IsAccepted(workflowsByName)), workflowsByName));
    }

    private long FindDistinctAcceptableCombinations(Rating validRating, Dictionary<string, Workflow> workflowsByName)
    {
        var test = Enumerable.Range(1, 4000)
                  .Sum(x => Enumerable.Range(1, 4000)
                                         .Sum(m => Enumerable.Range(1, 4000)
                                                                .Sum(a => Enumerable.Range(1, 4000)
                                                                                      .Count(s => new Rating(x, m, a, s).IsAccepted(workflowsByName)))));
        var validXs = Enumerable.Range(1, 4000)
                                .Where(x => (validRating with {X = x}).IsAccepted(workflowsByName))
                                .Count();
        
        var validMs = Enumerable.Range(1, 4000)
                                .Where(m => (validRating with {M = m}).IsAccepted(workflowsByName))
                                .Count();
        
        var validAs = Enumerable.Range(1, 4000)
                                .Where(a => (validRating with {A = a}).IsAccepted(workflowsByName))
                                .Count();
        
        var validSs = Enumerable.Range(1, 4000)
                                .Where(s => (validRating with {S = s}).IsAccepted(workflowsByName))
                                .Count();
        
        return (long) validXs * validMs * validAs * validSs;
    }

    private sealed record Workflow(string Name, string[] Rules)
    {
        public static Workflow Parse(string workflow)
        {
            var rulesStart = workflow.IndexOf('{');
            var name = workflow[..rulesStart];
            var rules = workflow[rulesStart..].Trim('{','}');

            return new (name, rules.Split(',').ToArray());
        }
    }

    private sealed record Rating(int X, int M, int A, int S)
    {
        public bool IsAccepted(Dictionary<string, Workflow> workflowsByName)
        {
            var currentWorkflow = workflowsByName["in"];
            
            while (true)
            {
                for (var i = 0; i < currentWorkflow.Rules.Length; i++)
                {
                    var currentRule = currentWorkflow.Rules[i];
                    if (currentRule.Contains(':'))
                    {
                        var ruleParts = currentRule.Split(':').ToArray();
                        var part = ruleParts[0][0] switch
                        {
                            'x' => X,
                            'm' => M,
                            'a' => A,
                            's' => S,
                            _ => throw new ArgumentException("Bad part index")
                        };
                        var value = int.Parse(ruleParts[0][2..]);
                        var isRuleFulfilled = ruleParts[0][1] == '<'
                            ? part < value : part > value;

                        if (isRuleFulfilled)
                        {
                            var target = ruleParts[1];
                            if (target == "A") return true;
                            if (target == "R") return false;
                            currentWorkflow = workflowsByName[target];
                            break;
                        }
                        continue;
                    }
                    if (currentRule == "A") return true;
                    if (currentRule == "R") return false;
                    currentWorkflow = workflowsByName[currentRule];
                    break;
                }
            }
        }

        public int Sum() => X + M + A + S;

        public static Rating Parse(string record)
        {
            var parts = record.Trim('{', '}').Split(',').ToArray();

            return new (int.Parse(parts[0][2..]), int.Parse(parts[1][2..]),
                        int.Parse(parts[2][2..]), int.Parse(parts[3][2..]));
        }
    }
}