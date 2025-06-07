using System.Runtime.CompilerServices;
using System.Text;
using Networks.Engine.Infrastructure;
using Networks.Engine.Models;

namespace Networks.Engine.Board;

public class Grid
{
    private Cell[] _cells;

    private readonly Random _random = Random.Shared;

    private int Bottom { get; set; }

    private int Right { get; set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public Point PowerSource { get; private set; }

    public bool IsSolved { get; private set; } 

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
        private set
        {
            if (! IsInBounds(x, y))
            {
                throw new PuzzleException($"{x}, {y} is outside of the grid's bounds (0..{Right}, 0..{Bottom}).");
            }

            _cells[Index(x, y)] = value;
        }
    }

    public Cell this[Point position]
    {
        get => this[position.X, position.Y];
        set => this[position.X, position.Y] = value;
    }

    public Grid(Puzzle puzzle)
    {
        Initialise(puzzle);
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

    public void PropagatePower()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (x == PowerSource.X && y == PowerSource.Y)
                {
                    continue;
                }

                _cells[x + y * Width] = new Cell(this[x, y].Piece, this[x, y].Rotation, false);
            }
        }

        var queue = new Queue<(Point Position, Direction Direction)>();

        var cell = this[PowerSource];

        var directions = Connector.Connections[(cell.Piece, cell.Rotation)];

        foreach (var direction in directions)
        {
            queue.Enqueue((PowerSource, direction));
        }

        var visited = new HashSet<(Point, Direction)>();

        var poweredCount = 1;
        
        while (queue.Count > 0)
        {
            var move = queue.Dequeue();

            if (! visited.Add((move.Position, move.Direction)))
            {
                continue;
            }

            var nextPosition = move.Position + move.Direction;

            var nextCell = this[nextPosition];

            if (nextCell.Piece == Piece.OutOfBounds)
            {
                continue;
            }

            var nextDirections = Connector.Connections[(nextCell.Piece, nextCell.Rotation)];

            if (nextDirections.Contains(new Direction(-move.Direction.Dx, -move.Direction.Dy)))
            {
                this[nextPosition] = new Cell(nextCell.Piece, nextCell.Rotation, true);

                poweredCount++;

                foreach (var nextDirection in nextDirections)
                {
                    if (nextDirection.Dx == -move.Direction.Dx && nextDirection.Dy == -move.Direction.Dy)
                    {
                        continue;
                    }

                    queue.Enqueue((nextPosition, nextDirection));
                }
            }
        }

        IsSolved = poweredCount == Width * Height;
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
                
                _cells[Index(x, y)] = new Cell(cell.Piece, (Rotation) _random.Next(4), x == PowerSource.X && y == PowerSource.Y);
            }
        }
        
        PropagatePower();
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