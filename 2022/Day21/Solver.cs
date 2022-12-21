namespace AdventOfCode.Y2022.Day21;

[Problem("Monkey Math", 2022, 21)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var monkeys = input.SplitLines().Select(IMonkey.Parse).ToList();
        
        // Part 1
        var yellByMonkey = YellAround(monkeys);
        var part1 = yellByMonkey["root"];
        
        return new(part1, 0);
    }

    private static Dictionary<string, long> YellAround(IEnumerable<IMonkey> monkeys)
    {
        var yellByMonkey = monkeys.OfType<ValueMonkey>().ToDictionary(m => m.Name, m => m.Value);
        var mathMonkeys = new Queue<IMonkey>(monkeys.OfType<MathMonkey>());

        while (mathMonkeys.Count > 0)
        {
            var current = mathMonkeys.Dequeue();
            if (!current.CanYell(yellByMonkey)) mathMonkeys.Enqueue(current);
            else yellByMonkey[current.Name] = current.Yell(yellByMonkey);
        }

        return yellByMonkey;
    }

    interface IMonkey 
    {
        public string Name { get; }
        public bool CanYell(Dictionary<string, long> yellByMonkey);
        public long Yell(Dictionary<string, long> yellByMonkey);

        static IMonkey Parse(string value)
        {
            var parts = value.Split(": ");
            if (parts[1].Contains("+"))
            {
                var buddies = parts[1].Split(" + ");
                return new MathMonkey(parts[0], buddies[0], buddies[1], (a, b) => a + b);
            }
            else if (parts[1].Contains("-"))
            {
                var buddies = parts[1].Split(" - ");
                return new MathMonkey(parts[0], buddies[0], buddies[1], (a, b) => a - b);
            }
            else if (parts[1].Contains("*"))
            {
                var buddies = parts[1].Split(" * ");
                return new MathMonkey(parts[0], buddies[0], buddies[1], (a, b) => a * b);
            }
            else if (parts[1].Contains("/"))
            {
                var buddies = parts[1].Split(" / ");
                return new MathMonkey(parts[0], buddies[0], buddies[1], (a, b) => a / b);
            }
            return new ValueMonkey(parts[0], int.Parse(parts[1]));
        }
    }

    record ValueMonkey(string Name, long Value): IMonkey
    {
        public bool CanYell(Dictionary<string, long> yellByMonkey) => true;

        public long Yell(Dictionary<string, long> yellByMonkey) => Value;
    }

    record MathMonkey(string Name, string LeftName, string RightName, Func<long, long, long> Operation) : IMonkey
    {
        public bool CanYell(Dictionary<string, long> yellByMonkey) =>
            yellByMonkey.ContainsKey(LeftName) && yellByMonkey.ContainsKey(RightName);

        public long Yell(Dictionary<string, long> yellByMonkey) => 
            Operation(yellByMonkey[LeftName], yellByMonkey[RightName]);
    }

}