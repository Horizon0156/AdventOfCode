namespace AdventOfCode.Common;

internal record Point(int X, int Y)
{
    public double EuclideanDistance(Point target) => Math.Sqrt(Math.Pow(target.X - X, 2) + Math.Pow(target.Y - Y, 2));

    public IEnumerable<Point> Interpolate(Point end, int step = 1)
    {
        var xStep = this.X <= end.X ? step : -step;
        var yStep = this.Y <= end.Y ? step : -step;

        for(var x = this.X; xStep > 0 ? x <= end.X : x >= end.X; x += xStep)
            for(var y = this.Y; yStep > 0 ? y <= end.Y : y >= end.Y; y += yStep)
                yield return new (x, y);
    }

    public int ManhattanDistance(Point target) => Math.Abs(X - target.X) + Math.Abs(Y - target.Y);

    public Point MoveTowards(Point point, int step = 1)
    {
        return this with 
        {
            X = X + Math.Clamp(point.X - X, -step, step),
            Y = Y + Math.Clamp(point.Y - Y, -step, step)
        };
    }
    
    public static Point Parse(string value)
    {
        var parts = value.Split(",");
        return new (int.Parse(parts[0]), int.Parse(parts[1]));
    }
}

internal record Point<T>(int X, int Y, T Value) : Point(X, Y);
