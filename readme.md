# Advent of Code
My contributions for [Advent of Code](https://adventofcode.com/)

## Advent of Code CLI
The Advent of Code CLI supports in creating the solution code files and loads the puzzle input from AoC. In addition, the CLI will run the puzzle, measure and print the outputs. 

### Usage
Simply call the CLI by running `dotnet run` or the executeable filename and either pass `init` or solve to process today's challenge. In order to process older puzzles the `day` and / or `year` parameter needs to be set. 

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
To download a puzzle you need to pass your AoC Session Cookie. Simply grab the value from AoC while you are signed in and store it in your working directory.

```
echo 523xy.............. > AOC_SESSION
```

#### Solution example
Every solution needs to implement `ISolver` and declare itself as a solution for the given day using the `Problem` attribute. 
```
[Problem("Problem", 2022, 2)]
internal class Solution : ISolver
{
    public object SolvePart1(string input) => 0;

    public object SolvePart2(string input) => 0;
}
```


