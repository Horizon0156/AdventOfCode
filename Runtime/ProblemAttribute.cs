namespace AdventOfCode.Runtime;

[AttributeUsage(AttributeTargets.Class)]
internal class PuzzleAttribute : Attribute
{
    public PuzzleAttribute(string name, int year, int day)
    {
        Name = name;
        Date = new (year, 12, day);
    }

    public string Name { get; }
    public DateOnly Date { get; }
}