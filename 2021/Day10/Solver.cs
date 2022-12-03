namespace AdventOfCode.Y2021.Day10;

[Problem("Syntax Scoring", 2021, 10)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var data = input.SplitLines();

        // Part 1
        var errorScore = data.Sum(l => CheckSyntax(l).Score);

        // Part 2
        var incompletenessScores = data.Select(d => CheckSyntax(d).GetIncompletenessScore())
                                    .Where(s => s != 0)
                                    .OrderBy(s => s)
                                    .ToArray();
        var incompletenessScore = incompletenessScores[incompletenessScores.Length / 2];

        return new (errorScore, incompletenessScore);
    }

    record SyntaxCheckResult(int Score, List<char> PendingChunks)
    {
        public ulong GetIncompletenessScore() 
        {
            ulong score = 0;
            foreach (var pendingChunk in PendingChunks)
            {
                score *= 5;
                score += pendingChunk switch
                    {
                        '(' => 1,
                        '[' => 2,
                        '{' => 3,
                        '<' => 4,
                        _ => throw new ArgumentOutOfRangeException(nameof(pendingChunk))
                    };
            }
            return score;
        }
    }

    bool IsClosingProperly(char openingChunk, char closingChunk) {

        return (openingChunk == '(' && closingChunk == ')')
            || (openingChunk == '[' && closingChunk == ']')
            || (openingChunk == '{' && closingChunk == '}')
            || (openingChunk == '<' && closingChunk == '>');
    }

    SyntaxCheckResult CheckSyntax(string line)
    {
        var openingChunks = new[] { '(', '[', '{', '<' };
        var closingChunks = new[] { ')', ']', '}', '>' };
        var chunkStack = new Stack<char>();

        foreach (var chunk in line)
        {
            if (openingChunks.Contains(chunk))
            {
                chunkStack.Push(chunk);
            }
            else if (closingChunks.Contains(chunk))
            {
                if (!chunkStack.TryPop(out var lastOpenedChunk) 
                || !IsClosingProperly(lastOpenedChunk, chunk))
                {
                    return chunk switch
                    {
                        ')' => new (3, new List<char>()),
                        ']' => new (57, new List<char>()),
                        '}' => new (1197, new List<char>()),
                        '>' => new (25137, new List<char>()),
                        _ => throw new ArgumentOutOfRangeException(nameof(chunk))
                    };
                }
            }
        }
        return new (0,  chunkStack.ToList());
    }
}