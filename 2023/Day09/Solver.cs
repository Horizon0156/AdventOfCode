namespace AdventOfCode.Y2023.Day09;

[Puzzle("Mirage Maintenance", 2023, 9)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var extrapolations = input.SplitLines()
                             .Select(l => l.Split(" ").Select(int.Parse).ToArray())
                             .Select(Extrapolate);
        return new (extrapolations.Sum(e => e.Forward), extrapolations.Sum(e => e.Backward));
    }

    private static (int Backward, int Forward) Extrapolate(int[] numbers)
    {
        var forwardExtrapolation = 0;
        var backwardNumbers = new List<int>([numbers[0]]);
        var currentPattern = numbers;

        while(currentPattern.Any(c => c != 0))
        {
            var nextPattern = new int[currentPattern.Length - 1];
            for (var i = 0; i < currentPattern.Length - 1; i++)
            {
                nextPattern[i] = currentPattern[i + 1] - currentPattern[i];
            }
            forwardExtrapolation += currentPattern[^1];
            backwardNumbers.Add(nextPattern[0]);
            currentPattern = nextPattern;
        }
        backwardNumbers.Reverse();
        return (backwardNumbers.Aggregate((current, next) => next - current), forwardExtrapolation);
    }
}