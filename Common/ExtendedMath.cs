namespace AdventOfCode.Common;

internal static class ExtendedMath
{
    public static int GreatestCommonDivisor(int a, int b) => b == 0 ? a : GreatestCommonDivisor(b, a % b);

    public static long GreatestCommonDivisor(long a, long b) => b == 0 ? a : GreatestCommonDivisor(b, a % b);

    public static int LeastCommonMultiple(int a, int b) => a * b / GreatestCommonDivisor(a, b);

    public static long LeastCommonMultiple(long a, long b) => a * b / GreatestCommonDivisor(a, b);

    public static int LeastCommonMultiple(IEnumerable<int> numbers) => 
        numbers.Aggregate((a, b) => a * b / GreatestCommonDivisor(a, b));

    public static long LeastCommonMultiple(IEnumerable<long> numbers) => 
        numbers.Aggregate((a, b) => a * b / GreatestCommonDivisor(a, b));

    public static int Modulo(int a, int b) => (a % b) < 0 ? (a % b) + b : (a % b);
    
    public static long Modulo(long a, long b) => (a % b) < 0 ? (a % b) + b : (a % b);
}