namespace AdventOfCode.Runtime;

internal interface ISolverLocator
{
    ISolver? LocateSolver(DateOnly date);
}
