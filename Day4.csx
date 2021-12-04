#!/usr/bin/env dotnet-script
#nullable enable

/*******************************************
 * Advent of Code Day 4: Giant Squid
 ******************************************/

using System.Collections;

public record BingoNumber(int RowIndex, int ColumnIndex, int Number)
{
    public bool IsMarked { get; set; }
}

public class BingoBoard
{
    public BingoBoard() => Numbers = new List<BingoNumber>();

    public List<BingoNumber> Numbers { get; }

    public int GetScore(int winningNumber)
    {
        return Numbers.Where(n => !n.IsMarked).Sum(n => n.Number) * winningNumber;
    }

    public bool HasBingo()
    {
        var numbersByRow = Numbers.GroupBy(n => n.RowIndex);
        var numbersByColumn = Numbers.GroupBy(n => n.ColumnIndex);

        return numbersByRow.Any(r => r.All(n => n.IsMarked))
            || numbersByColumn.Any(c => c.All(n => n.IsMarked));
    }

    public void MarkNumber(int number)
    {
        Numbers.Where(n => n.Number == number)
               .ToList()
               .ForEach(n => n.IsMarked = true);
    }

    public static BingoBoard Parse(string[] data)
    {
        var board = new BingoBoard();

        board.Numbers.AddRange(
            data.SelectMany((numbers, row) => 
                numbers.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                       .Select(int.Parse)
                       .Select((number, column) => new BingoNumber(row, column, number))));
        
        return board;
    }

    public static List<BingoBoard> ParseMany(string[] data)
    {
        var boards = new List<BingoBoard>();
        var boardStartIndex = 0;
        for (var i = boardStartIndex; i < data.Length; i++)
        {
            // An empty line will mark a new board
            if (string.IsNullOrWhiteSpace(data[i]))
            {
                boards.Add(BingoBoard.Parse(data.Skip(boardStartIndex)
                                                .Take(i - boardStartIndex)
                                                .ToArray()));
                boardStartIndex = i + 1;
            }
        } 
        return boards;
    }
}

var data = await File.ReadAllLinesAsync("Data/Day4.txt");

// Line 1 holds the game numbers
var gameNumbers = data.First()
                      .Split(",", StringSplitOptions.TrimEntries)
                      .Select(int.Parse);

// Boards start in line 3
var boards = BingoBoard.ParseMany(data.Skip(2).ToArray());

var winnerScores = new List<int>();
foreach (var number in gameNumbers)
{
    boards.ForEach(b => b.MarkNumber(number));

    var winnerBoards = boards.Where(b => b.HasBingo()).ToList();
    if (winnerBoards.Count > 0)
    {
        winnerScores.AddRange(winnerBoards.Select(b => b.GetScore(number)));
        winnerBoards.ForEach(b => boards.Remove(b));
    };
}

Console.WriteLine($"Day 4 - Giant Squid: {winnerScores.First()}, {winnerScores.Last()}");
