using System.Text.RegularExpressions;

namespace AdventOfCode.Y2021.Day22;

[Problem("Reactor Reboot", 2021, 22)]
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
        var regex = new Regex(
            @"(off|on) x=(\-*\d*)..(\-*\d*),y=(\-*\d*)..(\-*\d*),z=(\-*\d*)..(\-*\d*)");

        foreach (var d in data)
        {
            var match = regex.Match(d);
            yield return new (
                string.Equals(match.Groups[1].Value, "on"),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value),
                int.Parse(match.Groups[5].Value),
                int.Parse(match.Groups[6].Value),
                int.Parse(match.Groups[7].Value)
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