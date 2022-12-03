namespace AdventOfCode.Runtime;

[AttributeUsage(AttributeTargets.Class)]
internal class ProblemAttribute : Attribute
{
    public ProblemAttribute(string name, int year, int day)
    {
        Name = name;
        Date = new (year, 12, day);
    }

    public string Name { get; }
    public DateOnly Date { get; }
}