namespace AdventOfCode.Y2022.Day19;

[Puzzle("Not Enough Minerals", 2022, 19)]
internal class Solver : ISolver
{
    public Solution Solve(string input)
    {
        var blueprints = input.SplitLines().Select(Blueprint.Parse);
        var part1 = blueprints.AsParallel()
                              .Select((b) => b.Id * b.FindBestGeodeOutcome(24))
                              .Sum();
        var part2 = blueprints.Take(3)
                              .AsParallel()
                              .Select(b => b.FindBestGeodeOutcome(32))
                              .Aggregate((a, b) => a * b);
        return new (part1, part2);
    }

    record Resources(int Ore, int Clay, int Obsidian, int Geode, 
                     int OreRobots, int ClayRobots, int ObsidianRobots, int GeodeRobots)
    {
        public Resources Produce() => this with
        {
            Ore = Ore + OreRobots,
            Clay = Clay + ClayRobots,
            Obsidian = Obsidian + ObsidianRobots,
            Geode = Geode + GeodeRobots,
        };
    }

    record Blueprint(int Id, int OreRobotCosts, int ClayRobotCosts, 
                    (int Ore, int Clay) ObsidianRobotCosts, (int Ore, int Obsidian) GeodeRobotCosts)
    {
        public int MaxOreConsumption =  Math.Max(
            Math.Max(OreRobotCosts, ClayRobotCosts),
            Math.Max(ObsidianRobotCosts.Ore, GeodeRobotCosts.Ore));

        public bool CanCraftOreRobot(Resources resources) => 
            OreRobotCosts <= resources.Ore && resources.OreRobots < MaxOreConsumption;

        public bool CanCraftClayRobot(Resources resources) => 
            ClayRobotCosts <= resources.Ore && resources.ClayRobots < ObsidianRobotCosts.Clay
            && resources.ObsidianRobots < GeodeRobotCosts.Obsidian; 

        public bool CanCraftObsidianRobot(Resources resources) => 
            ObsidianRobotCosts.Ore <= resources.Ore && ObsidianRobotCosts.Clay <= resources.Clay
            && resources.ObsidianRobots < GeodeRobotCosts.Obsidian; 

        public bool CanCraftGeodeRobot(Resources resources) => 
            GeodeRobotCosts.Ore <= resources.Ore && GeodeRobotCosts.Obsidian <= resources.Obsidian;

        public Resources CraftOreRobot(Resources resources) => resources with 
        {
            OreRobots = resources.OreRobots + 1,
            Ore = resources.Ore - OreRobotCosts
        };

        public Resources CraftClayRobot(Resources resources) => resources with 
        {
            ClayRobots = resources.ClayRobots + 1,
            Ore = resources.Ore - ClayRobotCosts
        };

        public Resources CraftObisidanRobot(Resources resources) => resources with 
        {
            ObsidianRobots = resources.ObsidianRobots + 1,
            Ore = resources.Ore - ObsidianRobotCosts.Ore,
            Clay = resources.Clay - ObsidianRobotCosts.Clay,
        };

        public Resources CraftGeodeRobot(Resources resources) => resources with 
        {
            GeodeRobots = resources.GeodeRobots + 1,
            Ore = resources.Ore - GeodeRobotCosts.Ore,
            Obsidian = resources.Obsidian - GeodeRobotCosts.Obsidian
        };

        public int FindBestGeodeOutcome(int minutes)
        {
            int maxGeode = 0;
            var queue = new Queue<(int Time, Resources Resources)>();
            var visited = new HashSet<(int Time, Resources Resources)>();
            queue.Enqueue((1, new Resources(0, 0, 0, 0, 1, 0, 0, 0)));

            while(queue.Count > 0) 
            {
                var state = queue.Dequeue();
                if (!visited.Add(state)) continue;
                var afterProduction = state.Resources.Produce();
                var possibleGeode = afterProduction.Geode + ((minutes - state.Time) * state.Resources.GeodeRobots);
                if (state.Time > minutes || possibleGeode < maxGeode) continue;
                maxGeode = possibleGeode;
               
                if (CanCraftGeodeRobot(state.Resources))
                {
                    queue.Enqueue((state.Time + 1, CraftGeodeRobot(afterProduction)));
                    continue;                    
                }
                if (CanCraftObsidianRobot(state.Resources)) 
                    queue.Enqueue((state.Time + 1, CraftObisidanRobot(afterProduction)));
                if (CanCraftClayRobot(state.Resources))
                    queue.Enqueue((state.Time + 1, CraftClayRobot(afterProduction)));
                if (CanCraftOreRobot(state.Resources)) 
                    queue.Enqueue((state.Time + 1, CraftOreRobot(afterProduction)));
                
                queue.Enqueue((state.Time + 1, afterProduction));
            }
            return maxGeode;
        }

        public static Blueprint Parse(string value)
        {
            var match = value.Match<int>(
                            @"Blueprint (\d*): Each ore robot costs (\d*) ore. " + 
                            @"Each clay robot costs (\d*) ore. " + 
                            @"Each obsidian robot costs (\d*) ore and (\d*) clay. " +
                            @"Each geode robot costs (\d*) ore and (\d*) obsidian.");

            return new (match[0], match[1], match[2], (match[3], match[4]), (match[5], match[6]));
        }
    }
}