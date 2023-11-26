namespace AdventOfCode.Common;

internal record Point(int X, int Y)
{
    public double EuclideanDistance(Point target) => Math.Sqrt(Math.Pow(target.X - X, 2) + Math.Pow(target.Y - Y, 2));

    public IEnumerable<Point> Get4Neighbourhood()
    {
        yield return this with { Y = Y - 1 };
        yield return this with { X = X - 1 };
        yield return this with { X = X + 1};
        yield return this with { Y = Y + 1 };
    }

    public IEnumerable<Point> Get8Neighbourhood()
    {
        yield return this with { X = X - 1, Y = Y - 1 };
        yield return this with { Y = Y - 1 };
        yield return this with { X = X + 1, Y = Y - 1 };
        yield return this with { X = X - 1 };
        yield return this with { X = X + 1};
        yield return this with { X = X - 1, Y = Y + 1 };
        yield return this with { Y = Y + 1 };
        yield return this with { X = X + 1, Y = Y + 1 };
    }
    
    public IEnumerable<Point> Interpolate(Point end, int step = 1)
    {
        var xStep = X <= end.X ? step : -step;
        var yStep = Y <= end.Y ? step : -step;

        for(var x = X; xStep > 0 ? x <= end.X : x >= end.X; x += xStep)
            for(var y = Y; yStep > 0 ? y <= end.Y : y >= end.Y; y += yStep)
                yield return new (x, y);
    }

    public int ManhattanDistance(Point target) => Math.Abs(X - target.X) + Math.Abs(Y - target.Y);

    public Point Move(Direction direction, int step = 1)
    {
        return direction switch
        {
            Direction.Up => this with { Y = this.Y + step },
            Direction.Down => this with { Y = this.Y - step },
            Direction.Left => this with { X = this.X - step },
            Direction.Right => this with { X = this.X + step },
            _ => throw new ArgumentException("Bad direction")
        };
    }

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
