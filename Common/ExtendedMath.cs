namespace AdventOfCode.Common;

internal static class ExtendedMath
{
    public static int Modulo(int a, int b) => (a % b) < 0 ? (a % b) + b : (a % b);
    
    public static long Modulo(long a, long b) => (a % b) < 0 ? (a % b) + b : (a % b);
}