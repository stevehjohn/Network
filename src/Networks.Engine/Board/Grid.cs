namespace Networks.Engine.Board;

public class Grid
{
    private Piece[] _pieces;

    public int Width { get; private set; }
    
    public int Height { get; private set; }
    
    public int Bottom { get; private set; }

    public int Right { get; private set; }

    public Piece this[int x, int y]
    {
        get
        {
            if (x < 0 || x > Right || y < 0 || y > Bottom)
            {
                return Piece.OutOfBounds;
            }

            return _pieces[x + y * Width];
        }
    }
}