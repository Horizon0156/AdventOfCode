namespace AdventOfCode.Y2021.Day17;

[Problem("Trick Shot", 2021, 17)]
internal class Solver : ISolver
{
    public Solution Solve(string input) => new ("-", FindBestShot(new TargetPosition(48, 70, -189, -148)));

    record ProbePosition(int X, int Y);

    record ProbeVelocity(int X, int Y);

    record TargetPosition(int X1, int X2, int Y1, int Y2);

    record Result(bool HitsTarget, int HighestPosition);

    Result ShootProbe(ProbeVelocity velocity, TargetPosition target)
    {
        var position = new ProbePosition(0, 0);
        var highestPosition = 0;

        while (position.Y >= Math.Min(target.Y1, target.Y2))
        {
            position = position with { X = position.X + velocity.X, Y = position.Y + velocity.Y };
            highestPosition = Math.Max(highestPosition, position.Y);
            
            if (target.X1 <= position.X && position.X <= target.X2 
            && target.Y1 <= position.Y && position.Y <= target.Y2) return new(true, highestPosition); 
            
            velocity = velocity with { X = velocity.X > 0 ? velocity.X - 1 : 0, Y = velocity.Y - 1 };
        }
        return new(false, highestPosition); 
    }

    (ProbeVelocity, Result, int) FindBestShot(TargetPosition target)
    {
        var bestVelocity = new ProbeVelocity(0, 0);
        var bestResult = new Result(false, 0);
        var hittingShots = 0;

        for (var x = 0; x < 80; x++)
        {
            for (var y = -200; y < 200; y++)
            {
                var velocity = new ProbeVelocity(x, y);
                var result = ShootProbe(velocity, target);
                if (result.HitsTarget) 
                {
                    hittingShots++;
                    if (y > bestVelocity.Y) 
                    {
                        bestVelocity = new (x, y);
                        bestResult = result;
                    }
                }
            }
        }
        return new (bestVelocity, bestResult, hittingShots);
    }
}