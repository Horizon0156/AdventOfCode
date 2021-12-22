#!/usr/bin/env dotnet-script
#nullable enable

class Player
{
    private const int DIRAC_BOARD_SIZE = 10;

    public Player(int startPosition)
    {
        ZeroBasedPosition = startPosition - 1;
        Score = 0;
    }

    private Player(int zeroBasedPosition, int score)
    {
        ZeroBasedPosition = zeroBasedPosition;
        Score = score;
    }

    public Player Clone()
    {
        return new Player(ZeroBasedPosition, Score);
    }

    public void Move(int steps)
    {
        ZeroBasedPosition = (ZeroBasedPosition + steps) % DIRAC_BOARD_SIZE;
        Score += ZeroBasedPosition + 1;
    }

    public int ZeroBasedPosition { get; private set; }

    public int Score { get; private set; }
}

(Player Winner, Player Looser, int DieRolls) GetWinner(Player player1, Player player2)
{
    var round = 0;
    var dieRoll = 0; 
    var players = new [] { player1, player2 };

    while (true)
    {
        var currentPlayer = players[round % 2];
        currentPlayer.Move(++dieRoll + ++dieRoll + ++dieRoll);
        if (currentPlayer.Score >= 1000)
        {
            return (currentPlayer, players.First(p => p != currentPlayer), dieRoll);
        }
        round++;
    }
}

Dictionary<int, int> FindUniversesByRoll()
{
    var universesByRoll = new Dictionary<int, int>();
    for (var a = 1; a <= 3; a++)
    {
        for (var b = 1; b <= 3; b++)
        {
            for (var c = 1; c <= 3; c++)
            {
                var roll = a + b + c;
                if (universesByRoll.ContainsKey(roll)) universesByRoll[roll]++;
                else universesByRoll.Add(roll, 1);
            }
        }
    }
    return universesByRoll;
}

readonly Dictionary<int, int> _universesByRoll = FindUniversesByRoll();

(long WinsPlayer1, long WinsPlayer2) GetWinsPerUniverse(Player player1, Player player2)
{
    if (player1.Score >= 21) return (1, 0);
    if (player2.Score >= 21) return (0, 1);

    var winsPlayer1 = 0L;
    var winsPlayer2 = 0L;
    foreach (var universeByRoll in _universesByRoll)
    {
        var activePlayer = player1.Clone();
        activePlayer.Move(universeByRoll.Key);
        var result = GetWinsPerUniverse(player2.Clone(), activePlayer);
        winsPlayer1 += result.WinsPlayer2 * universeByRoll.Value;
        winsPlayer2 += result.WinsPlayer1 * universeByRoll.Value;
    }
    return (winsPlayer1, winsPlayer2);
}

Console.WriteLine("Day 21: Dirac Dice");

var player1 = new Player(6);
var player2 = new Player(2);

var matchResult = GetWinner(player1.Clone(), player2.Clone());
System.Console.WriteLine($"Part 1: {matchResult.Looser.Score * matchResult.DieRolls}");

var wins = GetWinsPerUniverse(player1, player2);
System.Console.WriteLine($"Part 2: {Math.Max(wins.WinsPlayer1, wins.WinsPlayer2)}");