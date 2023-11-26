# Advent of Code 🎄
My contributions for [Advent of Code](https://adventofcode.com/)

## Advent of Code CLI
The Advent of Code CLI supports in creating the solution code files and loads the puzzle input from AoC. In addition, the CLI will run the puzzle, measure and print the outputs. 

### Usage
Simply call the CLI by running `dotnet run` or `AdventOfCode` in case you have bundled the application and either pass `init` or `solve` to process today's challenge. In order to process older puzzles the `day` and / or `year` parameter needs to be set. 

```
Advent of Code - CLI

Usage:
  AdventOfCode [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  init <day> <year>   Grabs the puzzle input and prepares the solution for the given day [day: 3, year: 2022]
  solve <day> <year>  Solves the puzzle for the given day [day: 3, year: 2022]
```

The `init` step will download the current puzzle input and prepare a Solution for the current day. 

#### AoC Session Cookie
To download a puzzle you need to set your AoC Session Cookie. Simply grab the value from AoC while you are signed in and update your user secrets. You can also store this clear-text value in your unencrypted `appsettings.json` file... But you wouldn't do that, right?! 

```
dotnet user-secrets set Settings:SessionToken "567abc..."
```

#### Solution example
Every solution needs to implement `ISolver` and declare itself as a `Solver` for the given day using the `Problem` attribute.
```C#
[Problem("Rock Paper Scissors", 2022, 2)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        // ...
        return new (42, 137);
    }
}
```