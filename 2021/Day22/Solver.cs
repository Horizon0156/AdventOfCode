namespace AdventOfCode.Y2021.Day22;

[Puzzle("Reactor Reboot", 2021, 22)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var instructions = ParseInstructions(input.SplitLines().ToArray());
        return new (ProcessInstructions(instructions.Take(20)), ProcessInstructions(instructions));
    }

    record Instruction(bool TurnOn, int MinX, int MaxX, int MinY, int MaxY, int MinZ, int MaxZ)
    {
        public bool DoesIntersect(Instruction other) =>
                !(MaxX < other.MinX || MinX > other.MaxX 
            || MaxY < other.MinY || MinY > other.MaxY 
            || MaxZ < other.MinZ || MinZ > other.MaxZ);

        public Instruction Intersect(Instruction other) => new Instruction(
                !other.TurnOn,
                Math.Max(MinX, other.MinX), Math.Min(MaxX, other.MaxX),
                Math.Max(MinY, other.MinY), Math.Min(MaxY, other.MaxY),
                Math.Max(MinZ, other.MinZ), Math.Min(MaxZ, other.MaxZ));

        public long Volume() => (MaxX - MinX + 1L) * (MaxY - MinY + 1L) * (MaxZ - MinZ + 1L);

        public long EffectiveVolume() => TurnOn ? Volume() : -Volume();
    }

    static IEnumerable<Instruction> ParseInstructions(string[] data)
    {
        foreach (var d in data)
        {
            var match = d.Match(@"(off|on) x=(\-*\d*)..(\-*\d*),y=(\-*\d*)..(\-*\d*),z=(\-*\d*)..(\-*\d*)");
            yield return new (
                string.Equals(match[0], "on"),
                int.Parse(match[1]),
                int.Parse(match[2]),
                int.Parse(match[3]),
                int.Parse(match[4]),
                int.Parse(match[5]),
                int.Parse(match[6])
            );
        }
    }

    static long ProcessInstructions(IEnumerable<Instruction> instructions)
    {
        var effectiveInstruction = new List<Instruction>();

        foreach (var i in instructions) 
        {
            var overlappingInstructions = new List<Instruction>();
            foreach (var other in effectiveInstruction) 
            {
                if (i.DoesIntersect(other)) overlappingInstructions.Add(i.Intersect(other));
            }
            effectiveInstruction.AddRange(overlappingInstructions);
            if (i.TurnOn) effectiveInstruction.Add(i);
        }
        return effectiveInstruction.Sum(i => i.EffectiveVolume());
    }
}