using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Point point1, Point point2)
    {
        return point1.X == point2.X && point1.Y == point2.Y;
    }

    public static bool operator !=(Point point1, Point point2)
    {
        return point1.X != point2.X || point1.Y != point2.Y;
    }

    public static Point operator +(Point point1, Point point2)
    {
        return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point operator -(Point point1, Point point2)
    {
        return new Point(point1.X - point2.X, point1.Y - point2.Y);
    }

    public override bool Equals(object obj)
    {
        return this == (Point)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
