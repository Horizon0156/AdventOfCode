#!/usr/bin/env dotnet-script
#nullable enable
 
public enum Direction
{
    Forward,
    Down,
    Up
}

public record Instruction(Direction Direction, int Unit)
{
    public static Instruction Parse(string instruction)
    {
        var instructionParts = instruction.Split(" ");

        return new(
            Enum.Parse<Direction>(instructionParts[0], ignoreCase: true),
            int.Parse(instructionParts[1]));
    } 
}

public record Position(int Horizontal, int Depth, int Aim)
{
    public int Product => Horizontal * Depth;
}

public class Submarine
{
    public Position Position { get; private set; } = new (0, 0, 0);

    public void Move(IEnumerable<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            Move(instruction);
        }
    }
    public void Move(Instruction instruction)
    {
        Position = instruction.Direction switch 
        {
            Direction.Forward => Position with 
            {
                Horizontal = Position.Horizontal + instruction.Unit,
                Depth = Position.Depth + Position.Aim * instruction.Unit
            },
            Direction.Up => Position with { Aim = Position.Aim - instruction.Unit },
            Direction.Down => Position with { Aim = Position.Aim + instruction.Unit },
            _ => throw new ArgumentException("Direction not supported")
        };
    }
}

var data = await File.ReadAllLinesAsync("Data/Day2.txt");
var instructions = data.Select(Instruction.Parse);

var submarine = new Submarine();
submarine.Move(instructions);

Console.WriteLine("Day 2: Dive!");
Console.WriteLine($"{submarine.Position}");
