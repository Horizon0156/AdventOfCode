﻿namespace AdventOfCode;

internal static class MapExtensions
{
    public static T ElementAt<T>(this T[,] map, Point point) => map[point.Y, point.X];

    public static int GetHeight<T>(this T[,] map) => map.GetLength(0);
    
    public static int GetWidth<T>(this T[,] map) => map.GetLength(1);

    public static IEnumerable<Point> GetPoints<T>(this T[,] map)
    {
        for (var y = 0; y < map.GetLength(0); y++)
            for (var x = 0; x < map.GetLength(1); x++)
                yield return new (x, y);
    }

    public static IEnumerable<Point> FindPoints<T>(this T[,] map, T searchValue) where T : IEquatable<T>
    {
        var points = new List<Point>();

        for (var y = 0; y < map.GetLength(0); y++)
            for (var x = 0; x < map.GetLength(1); x++)
                if (map[y, x].Equals(searchValue)) points.Add(new (x, y));

        return points;
    }
}
