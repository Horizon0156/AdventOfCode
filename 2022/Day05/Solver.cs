namespace AdventOfCode.Y2022.Day05;

[Problem("Supply Stacks", 2022, 5)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        // The lazy part: Hard coded stack model with inst. starting at line 11
        var stacks = new List<List<char>>
        {
            new List<char> { 'F', 'T', 'C', 'L', 'R', 'P', 'G', 'Q' },
            new List<char> { 'N', 'Q', 'H', 'W', 'R', 'F', 'S', 'J' },
            new List<char> { 'F', 'B', 'H', 'W', 'P', 'M', 'Q' },
            new List<char> { 'V', 'S', 'T', 'D', 'F' },
            new List<char> { 'Q', 'L', 'D', 'W', 'V', 'F', 'Z' },
            new List<char> { 'Z', 'C', 'L', 'S' },
            new List<char> { 'Z', 'B', 'M', 'V', 'D', 'F' },
            new List<char> { 'T', 'J', 'B' },
            new List<char> { 'Q', 'N', 'B', 'G', 'L', 'S', 'P', 'H' }
        };

        var instructions = input.SplitLines().Skip(10).Select(Instruction.Parse);

        foreach (var instruction in instructions)
        {
            var sourceStack = stacks.ElementAt(instruction.From - 1);
            var targetStack = stacks.ElementAt(instruction.To - 1);
            var targetCrate = sourceStack.Count - instruction.Amount; // Part 2

            for (var i = 0; i < instruction.Amount; i++)
            {
                //var targetCrate = sourceStack.Count - 1; // Part 1
                var item = sourceStack.ElementAt(targetCrate);
                sourceStack.RemoveAt(targetCrate);
                targetStack.Add(item);
            }
        }
        
        var topCrates = new string(stacks.Select(s => s.ElementAt(s.Count - 1)).ToArray());
        return new ("-", topCrates);
    }

    private record Instruction(int Amount, int From, int To)
    {
        public static Instruction Parse(string instruction)
        {
            var parts = instruction.Match<int>(@"move (\d*) from (\d*) to (\d*)");

            return new Instruction(parts[0], parts[1], parts[2]);
        }
    }
}