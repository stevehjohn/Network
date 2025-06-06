using System.Diagnostics.CodeAnalysis;

namespace Networks.Engine.Board;

public readonly record struct Point
{
    public int X { get; }
    
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        
        Y = y;
    }

    public static Point operator +(Point point, Direction direction) => new(point.X + direction.Dx, point.Y + direction.Dy);

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}