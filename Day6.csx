#!/usr/bin/env dotnet-script
#nullable enable

/**********************************************
 * Advent of Code Day 6: Lanternfish
 *********************************************/

long GetPopulationCount(int days, int populationTimer, int currentDay = 0)
{
    if (currentDay == days) return 1;

    return populationTimer == 0
        ? GetPopulationCount(days, 6, currentDay + 1) + GetPopulationCount(days, 8, currentDay + 1)
        : GetPopulationCount(days, populationTimer - 1, currentDay + 1);
}

var data = await File.ReadAllTextAsync("Data/Day6.txt");
var population = data.Split(",").Select(int.Parse);

var populationCount = population.Sum(f => GetPopulationCount(256, f));

Console.WriteLine($"Day 6 - Lanternfish: {populationCount}");
