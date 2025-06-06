namespace Networks.Engine.Board;

public readonly record struct Cell
{
    public Piece Piece { get; private init; }
    
    public Rotation Rotation { get; private init;  }
    
    public bool IsPowered { get; private init; }

    public Cell(Piece piece, Rotation rotation, bool isPowered)
    {
        Piece = piece;

        Rotation = rotation;

        IsPowered = isPowered;
    }
}