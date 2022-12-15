namespace AdventOfCode.Y2022.Day04;

[Problem("Camp Cleanup", 2022, 4)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var pairs = input.SplitLines()
                         .Select(p => p.Match<int>(@"(\d*)-(\d*),(\d*)-(\d*)"));
        
        return new (
            pairs.Where(ContainsOneTheOther).Count(),
            pairs.Where(OverlapsOneTheOther).Count());
    }

    private static bool OverlapsOneTheOther(int[] areas) =>
        !(areas[1] < areas[2] || areas[0] > areas[3]);

    private static bool ContainsOneTheOther(int[] areas) =>
        areas[0] <= areas[2] && areas[1] >= areas[3]
        || areas[2] <= areas[0] && areas[3] >= areas[1];
}