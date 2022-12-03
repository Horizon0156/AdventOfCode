namespace AdventOfCode.Y2021.Day06;

[Problem("Lanternfish", 2021, 6)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var population = input.Split(",").Select(int.Parse);
        var populationByAge = Enumerable.Range(0, 9)
                                        .Select(age => population.LongCount(p => p == age))
                                        .ToArray();

        for (var i = 0; i < 256; i++)
        {
            populationByAge = ProcessPopulation(populationByAge);
        }
        return new ("-", populationByAge.Sum());
    }

    private long[] ProcessPopulation(long[] populationByAge)
    {
        return Enumerable.Range(0, 9)
                        .Select(age => age switch
                        {
                            6 => populationByAge[0] + populationByAge[7],
                            8 => populationByAge[0],
                            _ => populationByAge[age + 1]
                        })
                        .ToArray();
    }
}