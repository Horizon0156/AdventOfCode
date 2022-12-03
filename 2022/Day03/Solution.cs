namespace Horizon0156.AdventOfCode.Y2022.Day03;

[Problem("Rucksack Reorganization", 2022, 3)]
internal class Solution : ISolver
{
    public object SolvePart1(string input) => 
        input.Split(Environment.NewLine)
             .Select(b => b.Chunk(b.Length / 2))
             .Sum(b => GetPriorityOfCommonItem(b));

    public object SolvePart2(string input) =>
        input.Split(Environment.NewLine)
             .Chunk(3)
             .Sum(b => GetPriorityOfCommonItem(b));

    private static int GetPriorityOfCommonItem(IEnumerable<IEnumerable<char>> backpacks)
    {
        var commonItem = backpacks.ElementAt(0).First(i => backpacks.Skip(1).All(b => b.Contains(i)));
        return Char.IsLower(commonItem) 
            ? commonItem - 'a' + 1
            : commonItem - 'A' + 27;
    }
}