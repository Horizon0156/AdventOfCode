namespace AdventOfCode.Y2021.Day01;

[Problem("Sonar Sweep", 2021, 1)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var measurements = input.SplitLines().Select(int.Parse);
        var p1 = measurements.Zip(measurements.Skip(1))
                             .Where(p => p.First < p.Second)
                             .Count();

        var flattenMeasurements =  measurements.SlidingWindow(3)
                                               .Select(w => w.Sum());
        var p2 = flattenMeasurements.Zip(flattenMeasurements.Skip(1))
                                    .Where(p => p.First < p.Second)
                                    .Count();
        return new (p1, p2);
    }
}