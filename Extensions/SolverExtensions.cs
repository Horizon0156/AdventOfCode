
using System.Reflection;

namespace AdventOfCode.Extensions;

internal static class SolverExtensions
{
    public static string? GetProblemName(this ISolver solver) => 
        solver.GetType().GetCustomAttribute<ProblemAttribute>()?.Name;
}