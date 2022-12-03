namespace AdventOfCode.Y2021.Day18;

[Problem("Snailfish", 2021, 18)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var pairs = input.SplitLines()
                         .Select(l => NumberPair.Parse(l))
                         .ToList();

        var part1 = pairs.Aggregate((a, b) => a.Add(b))
                         .GetMagnitude();

        var data = input.SplitLines().ToArray();
        var magnitudes = new List<int>();

        for (var i = 0; i < pairs.Count; i++)
        {
            for (var j = 0; j < pairs.Count; j++)
            {
                if (i == j) continue;
                magnitudes.Add(NumberPair.Parse(data[i]).Add(NumberPair.Parse(data[j])).GetMagnitude());
            }
        }
        return new (part1, magnitudes.Max());
    }

    interface ISnailfishNumber 
    {
        int GetMagnitude();
    }

    class RegularNumber : ISnailfishNumber
    {
        public RegularNumber(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public int GetMagnitude() => Value;

        public override string? ToString() => Value.ToString();
    }

    class NumberPair : ISnailfishNumber
    {
        public NumberPair(
            ISnailfishNumber left,
            ISnailfishNumber right,
            int level = 0,
            NumberPair? parent = null)
        {
            Left = left;
            Right = right;
            Level = level;
            Parent = parent;
        }

        public ISnailfishNumber Left { get; private set; }

        public int Level { get; private set;}

        public NumberPair? Parent { get; set; }

        public ISnailfishNumber Right { get; private set; }

        public NumberPair Add(NumberPair pair)
        {
            IncreaseLevel();
            pair.IncreaseLevel();

            var sum = new NumberPair(this, pair);
            this.Parent = sum;
            pair.Parent = sum;
            sum.Reduce();

            return sum;
        }

        public int GetMagnitude() => 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();

        public override string? ToString() => $"[{Left.ToString()},{Right.ToString()}]";

        public static NumberPair Parse(string s, int level = 0, NumberPair? parent = null)
        {
            var openBrackets = 0;
            var separatorIndex = -1;
            for (var i = 1; i < s.Length - 1 && separatorIndex == -1; i++) 
            {
                if (s[i] == '[') openBrackets++;
                if (s[i] == ']') openBrackets--;
                if (s[i] == ',' && openBrackets == 0) separatorIndex = i;
            }

            var leftNumber = s.Substring(1, separatorIndex - 1);
            var rightNumber = s.Substring(separatorIndex + 1, s.Length - separatorIndex - 2);

            var pair = new NumberPair(
                leftNumber.StartsWith('[') 
                    ? NumberPair.Parse(leftNumber, level + 1) 
                    : new RegularNumber(int.Parse(leftNumber)),
                rightNumber.StartsWith('[') 
                    ? NumberPair.Parse(rightNumber, level + 1) 
                    : new RegularNumber(int.Parse(rightNumber)),
                level);

            if (pair.Left is NumberPair pl) pl.Parent = pair;
            if (pair.Right is NumberPair pr) pr.Parent = pair;

            return pair;
        }

        private void IncreaseLevel()
        {
            Level++;
            if (Left is NumberPair lp) lp.IncreaseLevel();
            if (Right is NumberPair rp) rp.IncreaseLevel();
        }

        private bool Explode(List<RegularNumber> numbers)
        {
            if (Level >= 4 && Parent != null && Left is RegularNumber rl && Right is RegularNumber rr)
            {
                var leftIndex = numbers.IndexOf(rl) - 1;
                var leftNumber = leftIndex >= 0 ? numbers.ElementAt(leftIndex) : null;
                var rightIndex = numbers.IndexOf(rr) + 1;
                var rightNumber = rightIndex < numbers.Count ? numbers.ElementAt(rightIndex) : null;
                if (leftNumber != null) leftNumber.Value += rl.Value;
                if (rightNumber != null) rightNumber.Value += rr.Value;

                if (Parent.Left == this) Parent.Left = new RegularNumber(0);
                else if (Parent.Right == this) Parent.Right = new RegularNumber(0);
                
                return true;
            }
            if (Left is NumberPair pl && pl.Explode(numbers)) return true;
            return (Right is NumberPair pr && pr.Explode(numbers));
        }

        private List<RegularNumber> GetAllNumbers()
        {
            var numbers = new List<RegularNumber>();
            
            if (Left is RegularNumber ln) numbers.Add(ln);
            if (Left is NumberPair lp) numbers.AddRange(lp.GetAllNumbers());
            if (Right is RegularNumber rn) numbers.Add(rn);
            if (Right is NumberPair rp) numbers.AddRange(rp.GetAllNumbers());

            return numbers;
        }

        private void Reduce()
        {
            var actionTookPlace = true;
            
            while (actionTookPlace)
            {
                if (Explode(GetAllNumbers())) continue;
                if (Split()) continue;
                actionTookPlace = false;
            }
        }

        private bool Split()
        {
            if (Left is RegularNumber lr && lr.Value >= 10)
            {
                Left = new NumberPair(
                    new RegularNumber(lr.Value / 2),
                    new RegularNumber((int) Math.Ceiling(lr.Value / (decimal) 2)),
                    Level + 1,
                    this);
                return true;
            }
            if (Left is NumberPair pl && pl.Split()) return true;
            if (Right is RegularNumber rr && rr.Value >= 10) 
            {
                Right = new NumberPair(
                    new RegularNumber(rr.Value / 2),
                    new RegularNumber((int) Math.Ceiling(rr.Value / (decimal) 2)),
                    Level + 1,
                    this);
                return true;
            }
            return Right is NumberPair pr && pr.Split();
        }
    }
}