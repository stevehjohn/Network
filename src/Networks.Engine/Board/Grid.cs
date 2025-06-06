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
                return new Cell(Piece.OutOfBounds, Rotation.Zero, false);
            }

            return _cells[Index(x, y)];
        }
        set
        {
            if (! IsInBounds(x, y))
            {
                throw new PuzzleException($"{x}, {y} is outside of the grid's bounds (0..{Right}, 0..{Bottom}).");
            }

            _cells[Index(x, y)] = value;
        }
    }

    public Grid(Puzzle puzzle)
    {
        Initialise(puzzle);
    }

    public void SetRotation(int x, int y, Rotation rotation)
    {
        _cells[Index(x, y)] = new Cell(_cells[Index(x, y)].Piece, rotation, false);
    }

    public Grid Clone()
    {
        var clone = new Grid
        {
            Width = Width,
            Height = Height,
            Right = Right,
            Bottom = Bottom,
            _cells = new Cell[Width * Height],
            PowerSource = PowerSource
        };

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var cell = _cells[Index(x, y)];

                clone._cells[Index(x, y)] = new Cell(cell.Piece, cell.Rotation, cell.IsPowered);
            }
        }

        return clone;
    }

    private Grid()
    {
    }

    private int Index(int x, int y)
    {
        return x + y * Width;
    }

    private void Initialise(Puzzle puzzle)
    {
        Width = puzzle.GridWidth;

        Height = puzzle.GridHeight;

        Right = Width - 1;

        Bottom = Height - 1;

        _cells = new Cell[Width * Height];

        PowerSource = new Point(puzzle.Data.PowerCell % Width, puzzle.Data.PowerCell / Width);

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var powered = x == PowerSource.X && y == PowerSource.Y;
                
                _cells[Index(x, y)] = new Cell(puzzle.Data.GridLayout[x + y * Width], Rotation.Zero, powered);
            }
        }
        
        Randomise();
    }

    private void Randomise()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var cell = _cells[Index(x, y)];
                
                _cells[Index(x, y)] = new Cell(cell.Piece, (Rotation) _random.Next(4), cell.IsPowered);
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
                var cell = _cells[Index(x, y)];
                
                switch (cell.Piece)
                {
                    case Piece.Straight:
                        builder.Append(cell.Rotation switch
                        {
                            Rotation.Zero or Rotation.OneEighty => '│',
                            _ => '─'
                        });
                        break;

                    case Piece.Corner:
                        builder.Append(cell.Rotation switch
                        {
                            Rotation.Zero => '┌',
                            Rotation.Ninety => '┐',
                            Rotation.OneEighty => '┘',
                            _ => '└'
                        });
                        break;

                    case Piece.Tee:
                        builder.Append(cell.Rotation switch
                        {
                            Rotation.Zero => '┬',
                            Rotation.Ninety => '┤',
                            Rotation.OneEighty => '┴',
                            _ => '├'
                        });
                        break;

                    case Piece.Terminator:
                        builder.Append(cell.Rotation switch
                        {
                            Rotation.Zero => '╸',
                            Rotation.Ninety => '╹',
                            Rotation.OneEighty => '╺',
                            _ => '╻'
                        });
                        break;
                }
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}