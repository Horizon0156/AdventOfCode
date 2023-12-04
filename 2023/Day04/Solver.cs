namespace AdventOfCode.Y2023.Day04;

[Problem("Scratchcards", 2023, 4)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var cards = input.SplitLines()
                         .Select(l => new Card(l))
                         .ToList();

        for (var i = 0; i < cards.Count; i++)
            for (var j = i + 1; j < Math.Min(i + 1 + cards[i].NumberOfWins, cards.Count); j++)
                cards[j].NumberOfCopies += cards[i].NumberOfCopies;

        return new (cards.Sum(c => c.TotalPoints), cards.Sum(c => c.NumberOfCopies));
    }

    private sealed class Card
    {
        public int NumberOfCopies { get; set; } = 1;

        public int TotalPoints { get; init; }

        public int NumberOfWins { get; init; }

        public Card(string card)
        {
            var cardParts = card.Split(":")[1].Split("|");
            var winningNumbers = cardParts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                             .Select(int.Parse)
                                             .ToList();
            var chosenNumbers = cardParts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                            .Select(int.Parse)
                                            .ToList();
            NumberOfWins = winningNumbers.Intersect(chosenNumbers).Count();
            TotalPoints = NumberOfWins > 0 ? (int) Math.Pow(2, NumberOfWins - 1) : 0;
        }
    }
}