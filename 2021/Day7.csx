#!/usr/bin/env dotnet-script
#nullable enable

int GetFuelToPosition(int target, IEnumerable<int> positions)
{
    return positions.Select(p => Math.Abs(p - target)).Sum();
}

int TriangularNumber(int n) => n * (n + 1) / 2;

int GetFuelToPosition2(int target, IEnumerable<int> positions)
{
    return positions.Select(p => TriangularNumber(Math.Abs(p - target))).Sum();
}

var positions = File.ReadAllText("Data/Day7.txt").Split(",").Select(int.Parse);
var range = Enumerable.Range(positions.Min(), positions.Max() - positions.Min());
var part1 = range.Select(i => GetFuelToPosition(i, positions)).Min();
var part2 = range.Select(i => GetFuelToPosition2(i, positions)).Min();

Console.WriteLine("Day 7: The Treachery of Whales");
Console.WriteLine($"{part1}, {part2}");
