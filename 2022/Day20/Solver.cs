namespace AdventOfCode.Y2022.Day20;

[Problem("Grove Positioning System", 2022, 20)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var numbers = input.SplitLines().Select((n, i) => new Number(int.Parse(n), i)).ToArray();
        return new (
            GetGroveCoordinates(numbers, 1, 1),
            GetGroveCoordinates(numbers, 10, 811589153)
        );
    }

    static long GetGroveCoordinates(Number[] numbers, int iterations, int decryptionKey)
    {
        numbers = numbers.Select(n => n with { Value = n.Value * decryptionKey }).ToArray();
        var mixedNumbers = numbers.ToArray();

        for (var i = 0; i < iterations; i++)
        {
            foreach (var number in numbers)
            {
                var indexInMixedNumbers = mixedNumbers.IndexOf(number);
                if (number.Value == 0) continue;
                var newIndex = ExtendedMath.Modulo(indexInMixedNumbers + number.Value, numbers.Length - 1);
                mixedNumbers.Move(indexInMixedNumbers, (int) newIndex);
            }
        }

        var zeroIndex = mixedNumbers.FindIndex(n => n.Value == 0);
        return mixedNumbers[(zeroIndex + 1000) % mixedNumbers.Length].Value +
               mixedNumbers[(zeroIndex + 2000) % mixedNumbers.Length].Value +
               mixedNumbers[(zeroIndex + 3000) % mixedNumbers.Length].Value;
    }

    record Number(long Value, int OriginalPosition);
}