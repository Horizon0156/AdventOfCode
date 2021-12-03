#!/usr/bin/env dotnet-script
#nullable enable

/*******************************************
 * Advent of Code Day 3: Binary Diagnostic
 ******************************************/

private const int DIAGNOSTIC_WORD_LENGTH = 12;

private int CalculatePowerConsumption(string[] data)
{
    var gammaRate = 0;

    for (var i = 0; i < DIAGNOSTIC_WORD_LENGTH; i++)
    {
        gammaRate |= (IsHighBitDominating(data, i) ? 1 : 0) << DIAGNOSTIC_WORD_LENGTH - 1 - i;
    }

    var epsilonRate = ~gammaRate & 4095; // Mask 12 Bit

    return gammaRate * epsilonRate;
}

private bool IsHighBitDominating(string[] data, int bitPosition)
{
    return data.Count(d => d[bitPosition] == '1') >= data.Length / 2.0;
}

private int CalculateLifeSupportRating(string[] data)
{
    var subSet = (string[]) data.Clone();
    for (var i = 0; i < DIAGNOSTIC_WORD_LENGTH; i++)
    {
        subSet = subSet.Where(d => d[i] == (IsHighBitDominating(subSet, i) ? '1' : '0'))
                       .ToArray();

        if (subSet.Length == 1) break;
    }
    var oxygenGeneratorRating = Convert.ToInt16(subSet.First().ToString(), 2);

    subSet = (string[]) data.Clone();
    for (var i = 0; i < DIAGNOSTIC_WORD_LENGTH; i++)
    {
        subSet = subSet.Where(d => d[i] == (IsHighBitDominating(subSet, i)  ? '0' : '1'))
                       .ToArray();

        if (subSet.Length == 1) break;
    }
    var co2ScrubberRating = Convert.ToInt16(subSet.First().ToString(), 2);

    return oxygenGeneratorRating * co2ScrubberRating;
}

var data = await File.ReadAllLinesAsync("Data/Day3.txt");
            
var powerConsumption = CalculatePowerConsumption(data); // Part 1
var lifeSupportRating = CalculateLifeSupportRating(data); // Part 2

Console.WriteLine($"Day 3 - Binary Diagnostic: {powerConsumption}, {lifeSupportRating}");
