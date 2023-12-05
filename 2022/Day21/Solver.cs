namespace AdventOfCode.Y2022.Day21;

[Puzzle("Monkey Math", 2022, 21)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var monkeys = input.SplitLines().Select(IMonkey.Parse).ToList();
        
        // Part 1
        var yellByMonkey = YellAround(monkeys);
        var part1 = yellByMonkey["root"];

        // Part 2 (Paste result to https://www.mathpapa.com/)
        var monkeyByName = monkeys.ToDictionary(m => m.Name, m => m);
        var root = (MathMonkey) monkeyByName["root"] with { Operator = '=' };
        var expression = root.GetExpression(monkeyByName);
        
        return new(part1, expression);
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
        public string GetExpression(Dictionary<string, IMonkey> monkeyByName);
        public long Yell(Dictionary<string, long> yellByMonkey);

        static IMonkey Parse(string value)
        {
            var parts = value.Split(": ");
            if (parts[1].Contains("+"))
            {
                var buddies = parts[1].Split(" + ");
                return new MathMonkey(parts[0], buddies[0], buddies[1], '+');
            }
            else if (parts[1].Contains("-"))
            {
                var buddies = parts[1].Split(" - ");
                return new MathMonkey(parts[0], buddies[0], buddies[1], '-');
            }
            else if (parts[1].Contains("*"))
            {
                var buddies = parts[1].Split(" * ");
                return new MathMonkey(parts[0], buddies[0], buddies[1], '*');
            }
            else if (parts[1].Contains("/"))
            {
                var buddies = parts[1].Split(" / ");
                return new MathMonkey(parts[0], buddies[0], buddies[1], '/');
            }
            return new ValueMonkey(parts[0], int.Parse(parts[1]));
        }
    }

    record ValueMonkey(string Name, long Value): IMonkey
    {
        public bool CanYell(Dictionary<string, long> yellByMonkey) => true;

        public string GetExpression(Dictionary<string, IMonkey> monkeyByName) => Name == "humn" ? "x" : Value.ToString();

        public long Yell(Dictionary<string, long> yellByMonkey) => Value;
    }

    record MathMonkey(string Name, string LeftName, string RightName, char Operator) : IMonkey
    {
        public bool CanYell(Dictionary<string, long> yellByMonkey) =>
            yellByMonkey.ContainsKey(LeftName) && yellByMonkey.ContainsKey(RightName);

        public string GetExpression(Dictionary<string, IMonkey> monkeyByName) =>
            $"({monkeyByName[LeftName].GetExpression(monkeyByName)} {Operator} {monkeyByName[RightName].GetExpression(monkeyByName)})"; 

        public long Yell(Dictionary<string, long> yellByMonkey)
        {
            return Operator switch 
            {
                '+' => yellByMonkey[LeftName] + yellByMonkey[RightName],
                '-' => yellByMonkey[LeftName] - yellByMonkey[RightName],
                '*' => yellByMonkey[LeftName] * yellByMonkey[RightName],
                '/' => yellByMonkey[LeftName] / yellByMonkey[RightName],
                _ => throw new ArgumentException("Bad operator")
            };
        }
    }
}