namespace AdventOfCode.Y2023.Day07;

[Puzzle("Camel Cards", 2023, 7)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var hands = input.SplitLines().Select(Hand.Parse);

        var part1 = hands.OrderBy(h => h.GetHandType())
                         .ThenBy(h => h.GetHandScore())
                         .Select((hand, rank) => (rank + 1) * hand.Bid)
                         .Sum();

        var part2 = hands.OrderBy(h => h.GetHandType(withJokers: true))
                         .ThenBy(h => h.GetHandScore(withJokers: true))
                         .Select((hand, rank) => (rank + 1) * hand.Bid)
                         .Sum();

        return new (part1, part2);
    }

    private sealed record Hand(string Cards, int Bid)
    {
        private static readonly Dictionary<char, string> CARD_VALUES = new()
        {
            {'A', "13"}, {'K', "12"}, {'Q', "11"}, {'J', "10"}, {'T', "09"}, {'9', "08"}, {'8', "07"},
            {'7', "06"}, {'6', "05"}, {'5', "04"}, {'4', "03"}, {'3', "02"}, {'2', "01"},
        };

        private static readonly Dictionary<char, string> CARD_VALUES_WITH_JOKERS = new()
        {
            {'A', "13"}, {'K', "12"}, {'Q', "11"}, {'T', "10"}, {'9', "09"}, {'8', "08"}, {'7', "07"},
            {'6', "06"}, {'5', "05"}, {'4', "04"}, {'3', "03"}, {'2', "02"}, {'J', "01"},
        };

        public HandType GetHandType(bool withJokers = false)
        {
            var cardsWithCount = Cards.GroupBy(c => c)
                                      .Select(g => (card: g.First(), count: g.Count()));
            var joker = withJokers 
                ? cardsWithCount.Where(c => c.card == 'J').Sum(c => c.count)
                : 0;

            if (withJokers)
                cardsWithCount = cardsWithCount.Where(c => c.card != 'J');

            if (cardsWithCount.Any(c => c.count == 5 - joker) || joker == 5)
                return HandType.FiveOfAKind;
            if (cardsWithCount.Any(c => c.count == 4 - joker))
                return HandType.FourOfAKind;
            if (cardsWithCount.Any(c => c.count == 3) && cardsWithCount.Any(c => c.count== 2)
                || (joker == 1 && cardsWithCount.Where(c => c.count == 2).Count() == 2))
                return HandType.FullHouse;
            if (cardsWithCount.Any(c => c.count == 3 - joker))
                return HandType.ThreeOfAKind;
            if (cardsWithCount.Where(c => c.count == 2).Count() == 2)
                return HandType.TwoPair;
            if (cardsWithCount.Any(c => c.count == 2 - joker))
                return HandType.OnePair;
            return HandType.HighCard;
        }

        public int GetHandScore(bool withJokers = false)
            => withJokers
                ? int.Parse(string.Concat(Cards.Select(c => CARD_VALUES_WITH_JOKERS[c])))
                : int.Parse(string.Concat(Cards.Select(c => CARD_VALUES[c])));

        public static Hand Parse(string hand) 
        {
            var handParts = hand.Split(" ");
            return new (handParts[0], int.Parse(handParts[1]));
        }
    }

    private enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }
}