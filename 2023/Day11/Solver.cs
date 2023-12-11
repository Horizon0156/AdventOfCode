namespace AdventOfCode.Y2023.Day11;

[Puzzle("Cosmic Expansion", 2023, 11)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var map = input.SplitLines().To2DArray();
        var galaxies = map.FindPoints('#');

        var rowsWithoutGalaxies = Enumerable.Range(0, map.GetLength(0))
                                            .Where(y => !galaxies.Any(g => g.Y == y))
                                            .ToHashSet();
        var columnsWithoutGalaxies = Enumerable.Range(0, map.GetLength(1))
                                               .Where(x => !galaxies.Any(g => g.X == x))
                                               .ToHashSet();
        return new(
            GetDistancesBetweenGalaxies(galaxies, rowsWithoutGalaxies, columnsWithoutGalaxies, 1),
            GetDistancesBetweenGalaxies(galaxies, rowsWithoutGalaxies, columnsWithoutGalaxies, 999999));
    }

    private static long GetDistancesBetweenGalaxies(
        IEnumerable<Point> galaxies,
        HashSet<int> rowsWithoutGalaxies,
        HashSet<int> columnsWithoutGalaxies,
        int expansionFactor)
    {
        var sumOfDistances = 0L;
        foreach (var combination in galaxies.Combinations())
        {
            var minX = Math.Min(combination.Item1.X, combination.Item2.X);
            var maxX = Math.Max(combination.Item1.X, combination.Item2.X);
            var minY = Math.Min(combination.Item1.Y, combination.Item2.Y);
            var maxY = Math.Max(combination.Item1.Y, combination.Item2.Y);
            var expansionX = Enumerable.Range(minX, maxX - minX).Intersect(columnsWithoutGalaxies).Count();
            var expansionY = Enumerable.Range(minY, maxY - minY).Intersect(rowsWithoutGalaxies).Count();

            sumOfDistances += combination.Item1.ManhattanDistance(combination.Item2) 
                                + (expansionX * expansionFactor) + (expansionY * expansionFactor);
        }

        return sumOfDistances;
    }
}