using System.Runtime.CompilerServices;
using System.Text;
using Networks.Engine.Infrastructure;
using Networks.Engine.Models;

namespace Networks.Engine.Board;

public class Grid
{
    private Cell[] _cells;

    private readonly Random _random = Random.Shared;

    public int Width { get; private set; }

    public int Height { get; private set; }

    public int Bottom { get; private set; }

    public int Right { get; private set; }

    public Point PowerSource { get; private set; }

    public Cell this[int x, int y]
    {
        get
        {
            if (! IsInBounds(x, y))
            {
                return new Cell(Piece.OutOfBounds, Rotation.Zero);
            }

            return _cells[x + y * Width];
        }
        private set
        {
            if (! IsInBounds(x, y))
            {
                throw new PuzzleException($"{x}, {y} is outside of the grid's bounds (0..{Right}, 0..{Bottom}).");
            }

            _cells[x + y * Width] = value;
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

        _cells = new Cell[Width * Height];

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                _cells[x + y * Width] = new Cell(puzzle.Data.GridLayout[x + y * Width], Rotation.Zero);
            }
        }

        PowerSource = new Point(puzzle.Data.PowerCell % Width, puzzle.Data.PowerCell / Width);
        
        Randomise();
    }

    private void Randomise()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                _cells[x + y * Width] = new Cell(_cells[x + y * Width].Piece, (Rotation) _random.Next(4));
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                switch (_cells[x + y * Width].Piece)
                {
                    case Piece.Straight:
                        builder.Append('─');
                        break;

                    case Piece.Corner:
                        builder.Append('│');
                        break;

                    case Piece.Tee:
                        builder.Append('┌');
                        break;

                    case Piece.Terminator:
                        builder.Append('┌');
                        break;
                }
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}