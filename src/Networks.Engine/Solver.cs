using Networks.Engine.Board;

namespace Networks.Engine;

public class Solver
{
    private Grid _grid;
    
    public Action<Grid> StepCallback { get; init; }
    
    public bool Solve(Grid grid)
    {
        _grid = grid;

        var result = ProcessPosition(_grid.PowerSource, []);
        
        return result;
    }

    private bool ProcessPosition(Point position, HashSet<(Point, Rotation, Direction)> visited)
    {
        var cell = _grid[position];

        var rotations = cell.Piece == Piece.Straight ? 2 : 4;
        
        for (var rotation = 0; rotation < rotations; rotation++)
        {
            var directions = Connector.Connections[(cell.Piece, (Rotation) rotation)];

            var previousState = _grid[position];

            foreach (var direction in directions)
            {
                if (! visited.Add((position, (Rotation) rotation, direction)))
                {
                    continue;
                }
            
                var newCell = new Cell(cell.Piece, (Rotation) rotation, true);
                
                _grid[position] = newCell;
                
                _grid.PropagatePower();
                    
                StepCallback?.Invoke(_grid);

                if (_grid.IsSolved)
                {
                    return true;
                }

                var nextPosition = position + direction;

                var nextCell = _grid[nextPosition];

                if (nextCell.Piece == Piece.OutOfBounds)
                {
                    continue;
                }

                var nextDirections = Connector.Connections[(nextCell.Piece, nextCell.Rotation)];

                if (nextDirections.Contains(new Direction(-direction.Dx, -direction.Dy)))
                {
                    if (ProcessPosition(nextPosition, [..visited]))
                    {
                        return true;
                    }
                }
            }

            _grid[position] = previousState;

            _grid.PropagatePower();
        }

        return false;
    }
}