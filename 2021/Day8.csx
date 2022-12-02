#!/usr/bin/env dotnet-script
#nullable enable

static T TakeOut<T>(this List<T> list, Func<T, bool> predicate)
{
    var value = list.Single(predicate);
    list.Remove(value);

    return value;
}

string OrderSequence(string sequence) => new string(sequence.OrderBy(c => c).ToArray());

Dictionary<string, int> AnalyzeSequence(string[] sequence)
{
    var normalizedSequence = sequence.Select(OrderSequence);
    var numbersBySequence = new Dictionary<int, string>();

    // Unique digit patterns
    numbersBySequence.Add(1, normalizedSequence.Single(d => d.Length == 2));
    numbersBySequence.Add(7, normalizedSequence.Single(d => d.Length == 3));
    numbersBySequence.Add(4, normalizedSequence.Single(d => d.Length == 4));
    numbersBySequence.Add(8, normalizedSequence.Single(d => d.Length == 7));

    // 3, 2, 5
    var fiveDigitPatterns = normalizedSequence.Where(d => d.Length == 5).ToList();
    
    // 6, 9, 0
    var sixDigitPatterns = normalizedSequence.Where(d => d.Length == 6).ToList();

    // Deducation by segment inclusion
    numbersBySequence.Add(3, fiveDigitPatterns.TakeOut(d => numbersBySequence[1].All(s => d.Contains(s))));
    numbersBySequence.Add(9, sixDigitPatterns.TakeOut(d => numbersBySequence[3].All(s => d.Contains(s))));
    numbersBySequence.Add(0, sixDigitPatterns.TakeOut(d => numbersBySequence[7].All(s => d.Contains(s))));
    numbersBySequence.Add(6, sixDigitPatterns.Last());
    numbersBySequence.Add(5, fiveDigitPatterns.TakeOut(d => d.All(s => numbersBySequence[6].Contains(s))));
    numbersBySequence.Add(2, fiveDigitPatterns.Last());

    return numbersBySequence.ToDictionary(x => x.Value, x => x.Key);
}

int DecodeSequence(string[] signalPattern, string[] digitSequence)
{
    var normalizedSequence = digitSequence.Select(OrderSequence);
    var numberBySequence = AnalyzeSequence(signalPattern);
    var decodedSignal = string.Join(string.Empty, normalizedSequence.Select(s => numberBySequence[s]));
    
    return int.Parse(decodedSignal);;
}

var data = File.ReadAllLines("Data/Day8.txt")
               .Select(l => l.Split("|"))
               .Select(p => new Tuple<string[], string[]>(
                   p[0].Split(" ", StringSplitOptions.RemoveEmptyEntries),
                   p[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)));

// Part 1
var uniqueNumbers = data.Sum(s => s.Item2.Count(i => i.Length == 2 || i.Length == 3 || i.Length == 4 || i.Length == 7));

// Part 2
var sumOfDecodedNumbers = data.Sum(s => DecodeSequence(s.Item1, s.Item2));

Console.WriteLine("Day 8: Seven Segment Search");
Console.WriteLine($"{uniqueNumbers}, {sumOfDecodedNumbers}");
