namespace AdventOfCode.Runtime;

internal interface IPuzzleLoader
{
    Task<string> LoadPuzzleAsync(DateOnly date, CancellationToken cancellationToken);
}
