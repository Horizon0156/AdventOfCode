namespace AdventOfCode.Y2022.Day10;

[Puzzle("Cathode-Ray Tube", 2022, 10)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var executor = Register.Executor();
        var registers = input.SplitLines().Select(executor).ToArray();

        return new (
            GetInterestingSignalStrengths(registers),
            PrintCrtOutput(registers));
    }

    static int GetInterestingSignalStrengths(Register[] registers)
    {
        var signal20 = registers.Last(r => r.Cycle <= 20).Value * 20;
        var signal60 = registers.Last(r => r.Cycle <= 60).Value * 60;
        var signal100 = registers.Last(r => r.Cycle <= 100).Value * 100;
        var signal140 = registers.Last(r => r.Cycle <= 140).Value * 140;
        var signal180 = registers.Last(r => r.Cycle <= 180).Value * 180;
        var signal220 = registers.Last(r => r.Cycle <= 220).Value * 220;

        return signal20 + signal60 + signal100 + signal140 + signal180 + signal220;
    }

    static string PrintCrtOutput(Register[] registers)
    {
        var output = string.Empty;
        for (var y = 0; y < 6; y++)
        {
            output += Environment.NewLine;
            for (var x = 0; x < 40; x++)
            {
                var cycle = x + 1 + (y * 40);
                var spritePos = registers.LastOrDefault(r => r.Cycle <= cycle)?.Value ?? 1;
                output += spritePos - 1 <= x && x <= spritePos + 1
                    ? "#"
                    : " "; 
            }
        }
        return output;
    }

    record Register(int Cycle, int Value)
    {
        // Closures <3
        public static Func<string, Register> Executor()
        {
            var cycle = 1;
            var value = 1;

            return (string instruction) =>
                instruction.StartsWith("addx")
                    ? new (cycle += 2, value += int.Parse(instruction.Substring(5)))
                    : new (cycle += 1, value);
        }
    }
}