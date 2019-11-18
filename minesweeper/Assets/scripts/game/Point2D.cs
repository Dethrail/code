using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Point2D
{
    public bool Equals(Point2D other)
    {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is Point2D && Equals((Point2D) obj);
    }

    public override int GetHashCode()
    {
        unchecked {
            return (x * 397) ^ y;
        }
    }

    public static Point2D zero { get { return new Point2D(0, 0); } }

    public float sqrMagnitude { get { return x * x + y * y; } }

    public int x;
    public int y;

    public Point2D(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Point2D(Vector2 vector)
    {
        x = (int) vector.x;
        y = (int) vector.y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    public static bool operator ==(Point2D a, Point2D b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Point2D a, Point2D b)
    {
        return !(a == b);
    }

    public static Point2D operator -(Point2D a, Point2D b)
    {
        return new Point2D(a.x - b.x, a.y - b.y);
    }

    public static Point2D operator +(Point2D a, Point2D b)
    {
        return new Point2D(a.x + b.x, a.y + b.y);
    }

    public float magnitude { get { return Mathf.Sqrt(x * x + y * y); } }

    public Vector2 normalized { get { return new Vector2(x, y).normalized; } }

    public override string ToString()
    {
        return string.Format("[Point2D: x {0}, y {1}]", x, y);
    }

}

