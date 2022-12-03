using System.Reflection;

namespace AdventOfCode.Runtime;

internal class LocalSolverLocator : ISolverLocator
{
    public ISolver? LocateSolver(DateOnly date)
    {
        var solverType = Assembly.GetEntryAssembly()!
                                 .GetTypes()
                                 .FirstOrDefault(t => t.IsClass
                                                   && typeof(ISolver).IsAssignableFrom(t)
                                                   && t.GetCustomAttribute<ProblemAttribute>()?.Date == date);

        return solverType != null
            ? Activator.CreateInstance(solverType) as ISolver
            : null;
    }
}
