using System.Diagnostics.CodeAnalysis;

namespace Networks.Engine.Board;

public readonly record struct Direction
{
    public int Dx { get; }
    
    public int Dy { get; }

    public Direction(int dX, int dY)
    {
        Dx = dX;
        
        Dy = dY;
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"({Dx}, {Dy})";
    }
}