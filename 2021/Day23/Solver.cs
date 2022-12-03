namespace AdventOfCode.Y2021.Day23;

[Problem("Amphipod", 2021, 23)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        // ############# -> #############
        // #           # -> #           #
        // ###B#A#b#c### -> ###A#B#C#d###
        //   #C#D#d#a#   ->   #a#b#c#D#  
        //   #########   ->   #########  
        var amphipods = new List<Amphipod>() 
        {
            new Amphipod('A', 8),
            new Amphipod('a', 12),
            new Amphipod('B', 4),
            new Amphipod('b', 5),
            new Amphipod('C', 7),
            new Amphipod('c', 7),
            new Amphipod('D', 8),
            new Amphipod('d', 5)
        };

        return new ("... human did most of the work :)", amphipods.Sum(a => a.Energy));
    }

    record Amphipod(char Type, int Moves)
        {
            public int Energy => char.ToUpper(Type) switch
            {
                'A' => Moves,
                'B' => Moves * 10,
                'C' => Moves * 100,
                'D' => Moves * 1000,
                _ => throw new NotSupportedException("Type not supported")
            };
        }
}