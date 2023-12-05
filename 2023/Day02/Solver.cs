namespace AdventOfCode.Y2023.Day02;

[Puzzle("Cube Conundrum", 2023, 2)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var games = input.SplitLines().Select(Game.Parse);

        return new (
            games.Where(g => g.IsPossible).Sum(g => g.Id),
            games.Sum(g => g.Power));
    }

    private sealed record Game(int Id, bool IsPossible, int Power)
    {
        public static Game Parse(string game)
        {
            var gameParts = game.Split(":");
            var rounds = gameParts[1].Split(";").Select(Round.Parse);
            
            return new (
                int.Parse(gameParts[0].Replace("Game ", string.Empty)),
                rounds.All(r => r.IsPossible),
                rounds.Max(r => r.Red) * rounds.Max(r => r.Green) * rounds.Max(r => r.Blue)
            );
        }
    }

    private sealed record Round(int Red, int Green, int Blue)
    {
        public bool IsPossible => Red <= 12 && Green <= 13 && Blue <= 14;

        public static Round Parse(string round)
        {
            var red = 0;
            var green = 0;
            var blue = 0;
            var roundParts = round.Split(",");
            foreach (var roundPart in roundParts)
            {
                if (roundPart.Contains("red"))
                    red += int.Parse(roundPart.Replace(" red", string.Empty));
                if (roundPart.Contains("green"))
                    green += int.Parse(roundPart.Replace(" green", string.Empty));
                if (roundPart.Contains("blue"))
                    blue += int.Parse(roundPart.Replace(" blue", string.Empty));
            }
            return new (red, green, blue);
        }
    }
}