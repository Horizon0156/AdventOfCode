
internal class Settings
{
    public Settings()
    {
        PuzzleFilename = "Puzzle.txt";
        SolverFilename = "Solver.cs";
    }
    
    public required string SessionToken { get; set; }

    public required string PuzzleFilename { get; set; }

    public required string SolverFilename { get; set; }
}