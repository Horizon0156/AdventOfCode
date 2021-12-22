#!/usr/bin/env dotnet-script
#nullable enable

public enum FoldAxis
{
    X,
    Y
}

public record Dot(int X, int Y);

public record FoldInstruction(FoldAxis Axis, int Position);

public HashSet<Dot> FoldPaper(HashSet<Dot> paper, FoldInstruction fold)
{
    return paper.Select(d => fold.Axis == FoldAxis.X 
                                ? (d with { X = d.X < fold.Position ? d.X : fold.Position - (d.X - fold.Position) }) 
                                : (d with { Y = d.Y < fold.Position ? d.Y : fold.Position - (d.Y - fold.Position) }))
                .ToHashSet();
}

public IEnumerable<string> PrintPaper(HashSet<Dot> paper)
{
    var printedPaper = new bool[paper.Max(d => d.Y) + 1, paper.Max(d => d.X) + 1];
    paper.ToList()
         .ForEach(d => printedPaper.SetValue(true, d.Y, d.X));
    
    return Enumerable.Range(0, printedPaper.GetLength(0))
                     .Select(y => string.Join(string.Empty, Enumerable.Range(0, printedPaper.GetLength(1))
                                                                      .Select(x => printedPaper[y, x] == true ? '#' : '.')));
}

var data = File.ReadAllLines("Data/Day13.txt");
var dataSeparator = Array.IndexOf(data, string.Empty);
var paper = data.Take(dataSeparator)
                .Select(d => d.Split(","))
                .Select(d => new Dot(int.Parse(d[0]), int.Parse(d[1])))
                .ToHashSet();

var instructions = data.Skip(dataSeparator + 1)
                       .Select(i => i.Substring(11))
                       .Select(i => i.Split("="))
                       .Select(i => new FoldInstruction(Enum.Parse<FoldAxis>(i[0], ignoreCase: true), int.Parse(i[1])));

Console.WriteLine("Day 13: Transparent Origami");

// Part 1
var foldPaper1 = FoldPaper(paper, instructions.First());
Console.WriteLine($"{foldPaper1.Count}");

// Part 2
var foldPaper = paper;
foreach (var instruction in instructions) 
{
    foldPaper = FoldPaper(foldPaper, instruction);
}
var printedPaper = PrintPaper(foldPaper);
printedPaper.ToList().ForEach(l => System.Console.WriteLine(l));
