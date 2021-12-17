#!/usr/bin/env dotnet-script
#nullable enable

/*************************************************
 * Advent of Code Day 16: Packet Decoder
 ************************************************/

record Packet(int Version, long Value);

string HexToBinary(string word) => string.Join(
    string.Empty,
    word.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

long ParseLiteralValue(string binaryPacket, ref int index)
{
    var literal = string.Empty;
    
    while (binaryPacket[index] == '1')
    {
        literal += binaryPacket.Substring(index + 1, 4);
        index += 5;
    }
    literal += binaryPacket.Substring(index + 1, 4);
    index += 5;

    return Convert.ToInt64(literal, 2);
}

Packet ParsePacket(string binaryPacket, ref int index)
{
    var version = ParseInteger(binaryPacket, ref index, 3);
    var type = ParseInteger(binaryPacket, ref index, 3);

    if (type == 4) return new (version, ParseLiteralValue(binaryPacket, ref index));

    var mode = binaryPacket[index++];
    var subPackets = new List<Packet>();
    if (mode == '0') 
    {
        var subPackageLength = ParseInteger(binaryPacket, ref index, 15);
        var stopIndex = index + subPackageLength;
        while (index != stopIndex) subPackets.Add(ParsePacket(binaryPacket, ref index));
    }
    else 
    {
        var packetCount = ParseInteger(binaryPacket, ref index, 11);
        for (var i = 0; i < packetCount; i++) subPackets.Add(ParsePacket(binaryPacket, ref index));
    }

    version += subPackets.Sum(p => p.Version);

    switch (type)
    {
        case 0: return new (version, subPackets.Sum(p => p.Value));
        case 1: return new (version, subPackets.Aggregate((long) 1, (r, p) => r * p.Value));
        case 2: return new (version, subPackets.Min(p => p.Value));
        case 3: return new (version, subPackets.Max(p => p.Value));
        case 5: return new (version, (subPackets.First().Value > subPackets.Last().Value) ? 1 : 0);
        case 6: return new (version, (subPackets.First().Value < subPackets.Last().Value) ? 1 : 0);
        case 7: return new (version, (subPackets.First().Value == subPackets.Last().Value) ? 1 : 0);
        default: throw new ArgumentException(nameof(type));
    }
}

int ParseInteger(string binaryPacket, ref int index, int bitLength)
{
    var value = Convert.ToInt32(binaryPacket.Substring(index, bitLength), 2);
    index += bitLength;

    return value;
} 

var word = File.ReadAllText("Data/Day16.txt");
var index = 0;
var packet = ParsePacket(HexToBinary(word), ref index);

Console.WriteLine($"Day 16 - Packet Decoder: {packet}");