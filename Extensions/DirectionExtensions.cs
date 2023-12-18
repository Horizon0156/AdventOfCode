namespace AdventOfCode;

internal static class DirectionExtensions
{
    public static Direction TurnLeft(this Direction direction) => direction switch 
    {
        Direction.Up => Direction.Right,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
        Direction.Right => Direction.Down,
        _ => throw new ArgumentException("Bad direction")
    };

    public static Direction TurnRight(this Direction direction) => direction switch 
    {
        Direction.Up => Direction.Left,
        Direction.Down => Direction.Right,
        Direction.Left => Direction.Down,
        Direction.Right => Direction.Up,
        _ => throw new ArgumentException("Bad direction")
    };
}
