namespace AdventOfCode.Y2023.Day08;

using ConnectedNodes = (string left, string right);

[Puzzle("Haunted Wasteland", 2023, 8)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var lines = input.SplitLines();
        var instructions = lines.First();
        var nodes = lines.Skip(2)
                         .Select(line => line.Match(@"(.{3}) = \((.{3}), (.{3})\)"))
                         .ToDictionary(kv => kv[0], kv => new ConnectedNodes(kv[1], kv[2]));

        var numberOfStepsA = 0;
        var currentNode = "AAA";

        while (currentNode != "ZZZ")
        {
            var instruction = instructions[numberOfStepsA % instructions.Length];
            currentNode = instruction == 'L' ? nodes[currentNode].left : nodes[currentNode].right;
            numberOfStepsA++;
        }

        var currentNodes = nodes.Keys.Where(k => k.EndsWith("A")).ToArray();
        var nodeMoves = new int[currentNodes.Length];

        Parallel.For(0, nodeMoves.Length, i => 
        {
            var currentNode = currentNodes[i];
            var currentMoves = 0;
            while (!currentNode.EndsWith("Z"))
            {
                var instruction = instructions[currentMoves % instructions.Length];
                currentNode = instruction == 'L' ? nodes[currentNode].left : nodes[currentNode].right;
                currentMoves++;
            }
            nodeMoves[i] = currentMoves;
        });

        return new (numberOfStepsA, ExtendedMath.LeastCommonMultiple(nodeMoves.Select(n => (long) n)));
    }
}