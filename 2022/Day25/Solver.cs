namespace AdventOfCode.Y2022.Day25;

[Problem("Full of Hot Air", 2022, 25)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var snafuNumbers = input.SplitLines().Select(l => new SnafuNumber(l)).ToArray();
        var sum = snafuNumbers.Sum(n => n.ToDecimal());
        
        return new (SnafuNumber.FromDecimal(sum), "-");
    } 

    // SNAFU = Special Numeral-Analogue Fuel Units
    record SnafuNumber(string Number)
    {
        public static SnafuNumber FromDecimal(long @decimal)
        {
            var snafu = string.Empty;
            while (@decimal > 0) 
            {
                var digit = 0L;
                switch (digit = @decimal % 5)
                {
                    case 3:
                        snafu = "=" + snafu;
                        @decimal += 5;
                        break;
                    case 4:
                        snafu = "-" + snafu;
                        @decimal += 5;
                        break;
                    default:
                        snafu = $"{digit}" + snafu;
                        break;
                }
                @decimal /= 5;
            }
            return new SnafuNumber(snafu);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Snafu has ... 625 125 25 5 1 (*5)
        // 0 1 2 are decimals, - is -1, = is -2
        public long ToDecimal()
        {
            var @decimal = 0L;
            for (var i = 0; i < Number.Length; i++)
            {
                var digit = Number[Number.Length - 1 - i] switch
                {
                    '0' => 0,
                    '1' => 1,
                    '2' => 2,
                    '-' => -1,
                    '=' => -2,
                    _ => throw new ArgumentException("Invalid SNAFU Number")
                };
                @decimal += (long) Math.Pow(5, i) * digit;
            }
            return @decimal;
        }

        public override string? ToString() => Number;
    }
}