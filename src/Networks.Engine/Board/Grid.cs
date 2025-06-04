using System.Runtime.CompilerServices;
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
            if (! IsInBounds(x, y))
            {
                return Piece.OutOfBounds;
            }

            return _pieces[x + y * Width];
        }
        set
        {
            if (! IsInBounds(x, y))
            {
                throw new PuzzleException($"{x}, {y} is outside of the grid's bounds (0..{Right}, 0..{Bottom}).");
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

        _pieces = new Piece[Width * Height];

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                this[x, y] = puzzle.Data.GridLayout[y * Width + x];
            }
        }

        PowerSource = new Point(puzzle.Data.PowerCell % Width, puzzle.Data.PowerCell / Width);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
}