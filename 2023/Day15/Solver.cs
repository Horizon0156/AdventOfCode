namespace AdventOfCode.Y2023.Day15;

using Lens = (string Label, int FocalLength);

[Puzzle("Lens Library", 2023, 15)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {   
        var steps = input.Split(",");

        return new (steps.Sum(Hash), GetFocusingPower(steps));
    }

    private static int GetFocusingPower(IEnumerable<string> steps)
    {
        var boxes = new List<Lens>[256];
        for (var i = 0; i < boxes.Length; i++) boxes[i] = [];

        foreach (var step in steps)
        {
            if (step.Contains('='))
            {
                var stepParts = step.Split("=");
                var lens = new Lens(stepParts[0], int.Parse(stepParts[1]));
                var boxId = Hash(lens.Label);
                var existingLensId = boxes[boxId].FindIndex(l => l.Label == lens.Label);

                if (existingLensId >= 0) 
                {
                    boxes[boxId].RemoveAt(existingLensId);
                    boxes[boxId].Insert(existingLensId, lens);
                }
                else boxes[boxId].Add(lens);
            } 
            else if (step.Contains('-'))
            {
                var label = step.Trim('-');
                var boxId = Hash(label);
                var existingLabel = boxes[boxId].FirstOrDefault(l => l.Label == label);
                if (existingLabel.Label == label) boxes[boxId].Remove(existingLabel);
            }
        }
        return boxes.Select((lenses, boxId) => (lenses, boxId))
                    .Sum(b => b.lenses.Select((lens, slotId) => (lens, slotId))
                                      .Sum(l => (1 + b.boxId) * (1 + l.slotId) *l.lens.FocalLength));
    }

    private static int Hash(string value) => value.Aggregate(0, (sum, value) => (sum + value) * 17 % 256);
}