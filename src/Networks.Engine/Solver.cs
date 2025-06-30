using System.Collections.Generic;
using Networks.Engine.Board;

namespace Networks.Engine;

public class Solver
{
    private Grid _grid;

    private List<Point> _positions;

    private Dictionary<Point, int> _indexByPosition;
    
    public Action<Grid> StepCallback { get; init; }
    
    public bool Solve(Grid grid)
    {
        _grid = grid;

        _positions = new List<Point>(grid.Width * grid.Height);

        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                _positions.Add(new Point(x, y));
            }
        }

        _indexByPosition = new Dictionary<Point, int>();

        for (var i = 0; i < _positions.Count; i++)
        {
            _indexByPosition[_positions[i]] = i;
        }

        var result = ProcessPosition(0);

        return result;
    }

    private bool ProcessPosition(int index)
    {
        if (index >= _positions.Count)
        {
            _grid.PropagatePower();

            StepCallback?.Invoke(_grid);

            return _grid.IsSolved;
        }

        var position = _positions[index];

        var cell = _grid[position];

        var rotations = cell.Piece == Piece.Straight ? 2 : 4;

        var previousState = _grid[position];

        for (var rotation = 0; rotation < rotations; rotation++)
        {
            if (! RotationIsValid(position, cell.Piece, (Rotation) rotation, index))
            {
                continue;
            }

            _grid[position] = new Cell(cell.Piece, (Rotation) rotation, false);

            _grid.PropagatePower();

            StepCallback?.Invoke(_grid);

            if (ProcessPosition(index + 1))
            {
                return true;
            }
        }

        _grid[position] = previousState;

        return false;
    }

    private bool RotationIsValid(Point position, Piece piece, Rotation rotation, int index)
    {
        var allDirections = new[] { new Direction(-1, 0), new Direction(1, 0), new Direction(0, -1), new Direction(0, 1) };

        var rotationDirections = Connector.Connections[(piece, rotation)];

        foreach (var direction in allDirections)
        {
            var neighbourPos = position + direction;

            var neighbourCell = _grid[neighbourPos];

            var ourConnection = rotationDirections.Contains(direction);

            if (neighbourCell.Piece == Piece.OutOfBounds)
            {
                if (ourConnection)
                {
                    return false;
                }

                continue;
            }

            var neighbourIndex = _indexByPosition[neighbourPos];

            var reverse = new Direction(-direction.Dx, -direction.Dy);

            if (neighbourIndex < index)
            {
                var neighbourDirections = Connector.Connections[(neighbourCell.Piece, neighbourCell.Rotation)];

                var neighbourConnection = neighbourDirections.Contains(reverse);

                if (neighbourConnection != ourConnection)
                {
                    return false;
                }
            }
            else
            {
                var rotations = neighbourCell.Piece == Piece.Straight ? 2 : 4;

                var possible = false;

                for (var r = 0; r < rotations; r++)
                {
                    var neighbourDirections = Connector.Connections[(neighbourCell.Piece, (Rotation) r)];

                    var neighbourConnection = neighbourDirections.Contains(reverse);

                    if (neighbourConnection == ourConnection)
                    {
                        possible = true;

                        break;
                    }
                }

                if (! possible)
                {
                    return false;
                }
            }
        }

        return true;
    }
}