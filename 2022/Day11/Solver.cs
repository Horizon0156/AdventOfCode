namespace AdventOfCode.Y2022.Day11;

[Problem("Monkey in the Middle", 2022, 11)]
internal class Solver : ISolver
{
    public Solution Solve(string input) =>
        new (
            GetMonkeyBusinessLevel(ParseMonkeys(input), 20, reduceWorryLevel: true),
            GetMonkeyBusinessLevel(ParseMonkeys(input), 10000, reduceWorryLevel: false));

    static Monkey[] ParseMonkeys(string input) => 
        input.ChunkByBlankLine().Select(Monkey.Parse).ToArray();

    static ulong GetMonkeyBusinessLevel(Monkey[] monkeys, int rounds, bool reduceWorryLevel)
    {
        for (var r = 0; r < rounds; r++) 
        {
            foreach (var monkey in monkeys)
            {
                monkey.Play(monkeys, reduceWorryLevel);
            }
        }
        return monkeys.Select(m => m.InspectedItems)
                        .OrderByDescending(l => l)
                        .Take(2)
                        .Aggregate(1UL, (p, l) => p * l);
    }

    class Monkey
    {
        private readonly Queue<ulong> _items;
        private ulong _divisor;
        private readonly Func<ulong, ulong> _operation;
        private readonly int _successIndex;
        private readonly int _failIndex;

        public Monkey(
            ulong[] items,
            Func<ulong, ulong> operation,
            ulong divisor,
            int successIndex,
            int failIndex)
        {
            _items = new Queue<ulong>(items);
            _operation = operation;
            _divisor = divisor;
            _successIndex = successIndex;
            _failIndex = failIndex;
        }

        public ulong InspectedItems { get; private set; }

        public void Catch(ulong item) => _items.Enqueue(item);
        
        public void Play(Monkey[] otherMonkeys, bool reduceWorryLevel)
        {
            var commonMultiplier = otherMonkeys.Aggregate(1UL, (p, m) => p * m._divisor);
            while (_items.Count > 0)
            {
                InspectedItems++;
                var item = _items.Dequeue();
                item = _operation(item);
                item = reduceWorryLevel ? item / 3 : item % commonMultiplier;
                if (item % _divisor == 0) otherMonkeys[_successIndex].Catch(item);
                else otherMonkeys[_failIndex].Catch(item);
            }
        }

        public static Monkey Parse(string input)
        {
            var lines = input.SplitLines().ToArray();
            var items = lines[1].Replace("  Starting items: ", string.Empty).Split(", ").Select(ulong.Parse).ToArray();
            Func<ulong, ulong> operation = lines[2].Equals("  Operation: new = old * old") 
                            ? i => i * i 
                            : lines[2].StartsWith("  Operation: new = old * ") 
                                ? i => i * ulong.Parse(lines[2].Replace("  Operation: new = old * ", string.Empty))
                                : i => i + ulong.Parse(lines[2].Replace("  Operation: new = old + ", string.Empty));
            var divisor = ulong.Parse(lines[3].Replace("  Test: divisible by ", string.Empty));
            var successIndex = int.Parse(lines[4].Replace("    If true: throw to monkey ", string.Empty));
            var failIndex = int.Parse(lines[5].Replace("    If false: throw to monkey ", string.Empty));

            return new Monkey(items, operation, divisor, successIndex, failIndex);
        }
    }
}