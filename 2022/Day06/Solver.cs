namespace AdventOfCode.Y2022.Day06;

[Problem("Tuning Trouble", 2022, 6)]
internal class Solver : ISolver
{
    public Solution Solve(string input) => new (
        FindDistinctPacket(input, 4),
        FindDistinctPacket(input, 14)
    );

    private static int FindDistinctPacket(string input, int packetSize) => 
        input.SlidingWindow(packetSize)
             .ToList()
             .FindIndex(0, p => p.Distinct().Count() == packetSize) + packetSize;
}