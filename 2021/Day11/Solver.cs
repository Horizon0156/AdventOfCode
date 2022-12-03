namespace AdventOfCode.Y2021.Day11;

[Problem("Dumbo Octopus", 2021, 11)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var octopuses = input.SplitLines()
                             .Select(y => y.Select(x => int.Parse(x.ToString())))
                             .SelectMany((row, y) => row.Select((value, x) => new Octopus(x, y, value)))
                             .ToList();
        octopuses.ForEach(o => o.FindNeighboors(octopuses));

        // Part 1
        for (var i = 0; i < 100; i++)
        {
            octopuses.ForEach(o => o.GainEnergy());
            octopuses.ForEach(o => o.TryToFlash());
            octopuses.ForEach(o => o.Recover());
        }
        var numberOfFlashes = octopuses.Sum(o => o.FlashCounter);

        // Part 2
        var step = 1;
        octopuses.ForEach(o => o.Reset());

        while (true)
        {
            octopuses.ForEach(o => o.GainEnergy());
            octopuses.ForEach(o => o.TryToFlash());
            if (octopuses.All(o => o.HasFlashed)) break;
            octopuses.ForEach(o => o.Recover());
            step++;
        }

        return new (numberOfFlashes, step);
    }

    private class Octopus
    {   
        private int _initialEnergyLevel;

        public Octopus(int x, int y, int energyLevel)
        {
            X = x;
            Y = y;
            EnergyLevel = energyLevel;
            _initialEnergyLevel = energyLevel;
            Neighboors = new List<Octopus>();
        }

        public int X { get; }

        public int Y { get; }

        public int EnergyLevel { get; private set; }

        public int FlashCounter { get; private set; }

        public bool HasFlashed { get; private set; }

        public List<Octopus> Neighboors { get; }

        public void FindNeighboors(IEnumerable<Octopus> octopuses)
        {
            foreach (var octopus in octopuses)
            {
                if (octopus.X == X - 1 && octopus.Y == Y - 1) Neighboors.Add(octopus);
                if (octopus.X == X  && octopus.Y == Y - 1) Neighboors.Add(octopus);
                if (octopus.X == X + 1 && octopus.Y == Y - 1) Neighboors.Add(octopus);
                if (octopus.X == X - 1 && octopus.Y == Y) Neighboors.Add(octopus);
                if (octopus.X == X + 1 && octopus.Y == Y) Neighboors.Add(octopus);
                if (octopus.X == X - 1 && octopus.Y == Y + 1) Neighboors.Add(octopus);
                if (octopus.X == X && octopus.Y == Y + 1) Neighboors.Add(octopus);
                if (octopus.X == X + 1 && octopus.Y == Y + 1) Neighboors.Add(octopus);
            }
        }

        public void GainEnergy()
        {
            EnergyLevel++;
        }

        public void Recover()
        {
            if (HasFlashed)
            {
                HasFlashed = false;
                EnergyLevel = 0;
            }
        }

        public void Reset()
        {
            EnergyLevel = _initialEnergyLevel;
            HasFlashed = false;
            FlashCounter = 0;
        }

        public void TryToFlash()
        {
            if (EnergyLevel <= 9 || HasFlashed) return;

            FlashCounter++;
            HasFlashed = true;
            Neighboors.ForEach(n => 
            {   
                n.GainEnergy();
                n.TryToFlash();
            });
        }
    }
}