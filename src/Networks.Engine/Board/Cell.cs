namespace Networks.Engine.Board;

public record struct Cell
{
    public Piece Piece { get; private set; }
    
    public Rotation Rotation { get; private set;  }
    
    public bool IsPowered { get; private set; }

    public Cell(Piece piece, Rotation rotation, bool powered)
    {
        Piece = piece;

        Rotation = rotation;
    }
}