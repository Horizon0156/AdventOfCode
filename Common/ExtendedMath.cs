namespace AdventOfCode.Common;

internal static class ExtendedMath
{
    public static int GreatesCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static int LeastCommonMultiple(int a, int b) => (a / GreatesCommonDivisor(a, b)) * b;

    public static int Modulo(int a, int b) => (a % b) < 0 ? (a % b) + b : (a % b);
    
    public static long Modulo(long a, long b) => (a % b) < 0 ? (a % b) + b : (a % b);
}