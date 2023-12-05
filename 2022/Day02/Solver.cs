namespace AdventOfCode.Y2022.Day02;

[Puzzle("Rock Paper Scissors", 2022, 2)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var rounds = input.SplitLines();
        var strateyGuide1 = new Dictionary<string, int> 
        {
            {"A X", 4}, {"A Y", 8}, {"A Z", 3},
            {"B X", 1}, {"B Y", 5}, {"B Z", 9},
            {"C X", 7}, {"C Y", 2}, {"C Z", 6},
        };
        var strateyGuide2 = new Dictionary<string, int> 
        {
            {"A X", 3}, {"A Y", 4}, {"A Z", 8},
            {"B X", 1}, {"B Y", 5}, {"B Z", 9},
            {"C X", 2}, {"C Y", 6}, {"C Z", 7},
        };
        
        return new (
            rounds.Select(r => strateyGuide1[r]).Sum(),
            rounds.Select(r => strateyGuide2[r]).Sum());
    }
}