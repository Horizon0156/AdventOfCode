namespace AdventOfCode.Runtime;

internal interface IPuzzleLoader
{
    Task<string> GetPuzzleName(DateOnly date, CancellationToken cancellationToken);

    Task<string> LoadPuzzleInputAsync(DateOnly date, CancellationToken cancellationToken);
}
