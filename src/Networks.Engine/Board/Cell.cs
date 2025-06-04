namespace Networks.Engine.Board;

public record struct Cell
{
    public Piece Piece { get; private set; }
    
    public Rotation Rotation { get; set; }

    public Cell(Piece piece, Rotation rotation)
    {
        Piece = piece;

        Rotation = rotation;
    }
}