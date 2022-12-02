#!/usr/bin/env dotnet-script
#nullable enable

long[] ProcessPopulation(long[] populationByAge)
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

var data = await File.ReadAllTextAsync("Data/Day6.txt");
var population = data.Split(",").Select(int.Parse);
var populationByAge = Enumerable.Range(0, 9)
                                .Select(age => population.LongCount(p => p == age))
                                .ToArray();

for (var i = 0; i < 256; i++)
{
    populationByAge = ProcessPopulation(populationByAge);
}

Console.WriteLine("Day 6: Lanternfish");
Console.WriteLine($"{populationByAge.Sum()}");
