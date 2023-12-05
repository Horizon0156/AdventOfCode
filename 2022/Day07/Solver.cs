namespace AdventOfCode.Y2022.Day07;

[Puzzle("No Space Left On Device", 2022, 7)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var root = ParseTerminalOutput(input);
        var allDirs = root.Flatten();
        var freeSpace = 70000000 - root.Size;

        return new (
            allDirs.Where(d => d.Size <= 100000).Sum(d => d.Size),
            allDirs.Where(d => d.Size >= 30000000 - freeSpace).Min(d => d.Size));
    } 

    private static Directory ParseTerminalOutput(string input)
    {
        var root = new Directory("/");
        var currentDir = root;

        foreach(var line in input.SplitLines().Skip(1))
        {
            var lineParts = line.Split(" ");
            if (lineParts[0] == "$" && lineParts[1] == "ls") 
                continue;
            else if (lineParts[0] == "$" && lineParts[1] == "cd" && lineParts[2] == "..")
                currentDir = currentDir.Parent ?? root;
            else if (lineParts[0] == "$" && lineParts[1] == "cd")
                currentDir = currentDir.Directories.Single(d => d.Name == lineParts[2]);
            else if (lineParts[0] == "dir") 
                currentDir.Directories.Add(new (lineParts[1], currentDir));
            else
                currentDir.Files.Add(new (lineParts[1], int.Parse(lineParts[0])));
        }
        return root;
    }

    private record File(string Name, int Size);

    private class Directory
    {
        public Directory(string name, Directory? parent = null)
        {
            Name = name;
            Parent = parent;
        }

        public List<File> Files { get; } = new List<File>();

        public List<Directory> Directories { get; } = new List<Directory>();

        public string Name { get; }

        public Directory? Parent { get; }

        public int Size => Files.Sum(f => f.Size) + Directories.Sum(d => d.Size);

        public IEnumerable<Directory> Flatten() => Directories.Concat(Directories.SelectMany(d => d.Flatten()));
    }
}