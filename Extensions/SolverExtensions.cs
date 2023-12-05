
using System.Reflection;

namespace AdventOfCode.Extensions;

internal static class SolverExtensions
{
    public static string? GetPuzzleName(this ISolver solver) => 
        solver.GetType().GetCustomAttribute<PuzzleAttribute>()?.Name;
}