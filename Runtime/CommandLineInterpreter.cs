using System.CommandLine;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace AdventOfCode.Runtime;

internal class CommandLineInterpreter
{
    private readonly RootCommand _rootCommand;
    private readonly ISolverLocator _solverLocator;
    private readonly IPuzzleLoader _puzzleLoader;
    private readonly Settings _settings;

    public CommandLineInterpreter(
        ISolverLocator solverLocator,
        IPuzzleLoader puzzleLoader,
        IOptions<Settings> settings)
    {
        _solverLocator = solverLocator;
        _puzzleLoader = puzzleLoader;
        _settings = settings.Value;
        
        var dayArgument = new Argument<int>("day", () => DateTime.Now.Day, "The day of the puzzle");
        var yearArgument = new Argument<int>("year", () => DateTime.Now.Year, "The year of the puzzle");
        
        var initCommand = new Command("init", "Grabs the puzzle input and prepares the solution for the given day");
        var forceOption = new Option<bool>("--force", "Force an update of the puzzle input");
        forceOption.AddAlias("-f");
        initCommand.AddArgument(dayArgument);
        initCommand.AddArgument(yearArgument);
        initCommand.AddOption(forceOption);
        initCommand.SetHandler(
            async (year, day, forceUpdate) => await InitializeSolutionAsync(new(year, 12, day), forceUpdate),
            yearArgument,
            dayArgument,
            forceOption);

        var solveCommand = new Command("solve", "Solves the puzzle for the given day");
        solveCommand.AddArgument(dayArgument);
        solveCommand.AddArgument(yearArgument);
        solveCommand.SetHandler(
            async (year, day) => await SolvePuzzleAsync(new(year, 12, day)),
            yearArgument,
            dayArgument);

        _rootCommand = new RootCommand("Advent of Code - CLI 🎄");
        _rootCommand.AddCommand(initCommand);
        _rootCommand.AddCommand(solveCommand);
    }
    
    public Task<int> RunAsync(string[] args) => _rootCommand.InvokeAsync(args);

    private static string GetDirectoryPath(DateOnly date) => $"{date.Year}/Day{date.Day:D2}";

    private async Task CreateTemplateAsync(DateOnly date)
    {
        var templateFilepath = Path.Combine(GetDirectoryPath(date), _settings.SolverFilename);
        if (File.Exists(templateFilepath))
        {
            return;
        }

        var content = $$"""
        namespace AdventOfCode.Y{{date.Year}}.Day{{date.Day:D2}};

        [Problem("Problem", {{date.Year}}, {{date.Day}})]
        internal class Solver : ISolver
        {
            public Solution Solve(string input) => new (0, 0);
        }
        """;

        Directory.CreateDirectory(GetDirectoryPath(date));
        await File.WriteAllTextAsync(templateFilepath, content);
    }

    private async Task InitializeSolutionAsync(DateOnly date, bool forceUpdate)
    {
        if (date.Year < 2015 || date.Year > DateTime.UtcNow.Year || date.Day < 1 || date.Day > 25)
        {
            Console.WriteLine($"The Elves are done for now... {date} is out of Advent season.");
            return;
        }
        await CreateTemplateAsync(date);

        var puzzleFilepath = Path.Combine(GetDirectoryPath(date), _settings.PuzzleFilename);
        if (!File.Exists(puzzleFilepath) || forceUpdate) 
        {
            try
            {
                var input = await _puzzleLoader.LoadPuzzleAsync(date, CancellationToken.None);
                await File.WriteAllTextAsync(puzzleFilepath, input);
            }
            catch (ApiException e)
            {
                // We know, that ApiException are meaningful ;)
                Console.WriteLine(e);
            }
        }
    }

    private async Task SolvePuzzleAsync(DateOnly date)
    {
        var solver = _solverLocator.LocateSolver(date);
        if (solver is null) 
        {
            Console.WriteLine($"No solver could be found for {date}. The elves are getting nervous...");
            return;
        }

        var puzzleFilepath = Path.Combine(GetDirectoryPath(date), _settings.PuzzleFilename);
        if (!File.Exists(puzzleFilepath))
        {
            Console.WriteLine($"No puzzle could be found for {date}. " + 
                               "The elves can help you, try to init. your puzzle for today.");
            return ;
        }

        var input = await File.ReadAllTextAsync(puzzleFilepath);
        var stopwatch = new Stopwatch();

        stopwatch.Start();
        var solution = solver.Solve(input);
        stopwatch.Stop();

        Console.WriteLine($"Advent of Code {date.Year} 🎄");
        Console.WriteLine($"Day {date.Day}: {solver.GetProblemName()}");
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine($"Part 1: {solution.Part1}");
        Console.WriteLine($"Part 2: {solution.Part2}");
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine($"Computation time: {stopwatch.ElapsedMilliseconds} ms");
    }
}