namespace AdventOfCode.Y2022.Day13;

// A packet is just an array of arrays and values
using Packet = System.Text.Json.Nodes.JsonNode;
using Array = System.Text.Json.Nodes.JsonArray;
using Value = System.Text.Json.Nodes.JsonValue;

[Problem("Distress Signal", 2022, 13)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var packets = input.SplitLines()
                           .Where(l => !string.IsNullOrEmpty(l))
                           .Select(l => Packet.Parse(l));
        
        var divider1 = Packet.Parse("[[2]]");
        var divider2 = Packet.Parse("[[6]]");
        var packetList = packets.Concat(new [] { divider1, divider2 }).ToList();
        packetList.Sort((a, b) => a!.Compare(b!));
        
        return new(
            packets.Chunk(2).Select((p, i) => p[0]!.IsSmaller(p[1]!) ? i + 1 : 0).Sum(),
            (packetList.IndexOf(divider1) + 1) * (packetList.IndexOf(divider2) + 1));
    }
}

internal static class PacketExtensions
{
    public static int Compare(this Packet a, Packet b)
    {
        if (a is Value && b is Value) return (int) a - (int) b;

        var arrayA = a as Array ?? new Array((int) a);
        var arrayB = b as Array ?? new Array((int) b);
        return Enumerable.Zip(arrayA, arrayB)
                         .Select(p => p.First!.Compare(p.Second!))
                         .FirstOrDefault(c => c != 0, arrayA.Count - arrayB.Count);
    }

    public static bool IsSmaller(this Packet a, Packet b) => a.Compare(b) < 0;
}