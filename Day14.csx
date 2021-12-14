#!/usr/bin/env dotnet-script
#nullable enable

/*************************************************
 * Advent of Code Day 14: Extended Polymerization
 ************************************************/

string TransformPolymer(string polymer, Dictionary<string, string> insertionByPair)
{
    var transformedPolymer = polymer;
    var inserted = 0;
    for (var i = 0; i < polymer.Length - 1; i++)
    {
        var insertion = insertionByPair[polymer.Substring(i, 2)];
        transformedPolymer = transformedPolymer.Insert(i + 1 + inserted, insertion);
        inserted++;
    }
    return transformedPolymer;
}

var data = File.ReadAllLines("Data/Day14.txt");
var polymerTemplate = data.First();
var insertionByPair = data.Skip(2)
                          .Select(d => d.Split(" -> "))
                          .ToDictionary(i => i[0], i => i[1]);

for (var i = 0; i < 10; i++)
{
    polymerTemplate = TransformPolymer(polymerTemplate, insertionByPair);
}

var orderedCount = polymerTemplate.GroupBy(c => c)
                                  .Select(g => g.LongCount())
                                  .OrderByDescending(c => c);
               
Console.WriteLine($"Day 14 - Extended Polymerization: {orderedCount.First() - orderedCount.Last()}");
