using Networks.Engine.Infrastructure;
using Networks.Engine.Models;

namespace Networks.Engine.Board;

public class Grid
{
    private Piece[] _pieces;

    public int Width { get; private set; }
    
    public int Height { get; private set; }
    
    public int Bottom { get; private set; }

    public int Right { get; private set; }
    
    public Point PowerSource { get; private set; }

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
        set
        {
            if (x < 0 || x > Right || y < 0 || y > Bottom)
            {
                throw new PuzzleException($"{x}, {y} is outside of the grid's bounds ({Width}x{Height}).");
            }

            _pieces[x + y * Width] = value;
        }
    }

    public Grid(Puzzle puzzle)
    {
        Initialise(puzzle);
    }

    private void Initialise(Puzzle puzzle)
    {
        Width = puzzle.GridWidth;

        Height = puzzle.GridHeight;

        Right = Width - 1;

        Bottom = Height - 1;

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                this[x, y] = puzzle.Data.GridLayout[y * Width + x];
            }
        }

        PowerSource = new Point(puzzle.Data.PowerCell % Width, puzzle.Data.PowerCell / Width);
    }
}