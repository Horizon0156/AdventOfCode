namespace AdventOfCode.Y2021.Day07;

[Puzzle("The Treachery of Whales", 2021, 7)]
internal class Solver : ISolver
{
    public Solution Solve(string input) 
    {
        var positions = input.Split(",").Select(int.Parse);
        var range = Enumerable.Range(positions.Min(), positions.Max() - positions.Min());

        return new (
            range.Select(i => GetFuelToPosition(i, positions)).Min(),
            range.Select(i => GetFuelToPosition2(i, positions)).Min());
    }

    private int GetFuelToPosition(int target, IEnumerable<int> positions)
    {
        return positions.Select(p => Math.Abs(p - target)).Sum();
    }

    private int TriangularNumber(int n) => n * (n + 1) / 2;

    private int GetFuelToPosition2(int target, IEnumerable<int> positions)
    {
        return positions.Select(p => TriangularNumber(Math.Abs(p - target))).Sum();
    }
}