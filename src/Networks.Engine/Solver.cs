using System.Net.Security;
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

    private bool ProcessPosition(Point position, HashSet<Point> visited)
    {
        if (! visited.Add(position))
        {
            return false;
        }
            
        var cell = _grid[position];

        var rotations = cell.Piece == Piece.Straight ? 2 : 4;
        
        var previousState = _grid[position];

        for (var rotation = 0; rotation < rotations; rotation++)
        {
            var newCell = new Cell(cell.Piece, (Rotation) rotation, true);
                
            _grid[position] = newCell;
                
            _grid.PropagatePower();
                    
            StepCallback?.Invoke(_grid);

            if (_grid.IsSolved)
            {
                return true;
            }

            var directions = Connector.Connections[(cell.Piece, (Rotation) rotation)];

            var valid = true;
            
            foreach (var direction in directions)
            {
                var nextPosition = position + direction;

                var nextCell = _grid[nextPosition];

                if (nextCell.Piece == Piece.OutOfBounds)
                {
                    continue;
                }

                var nextDirections = Connector.Connections[(nextCell.Piece, nextCell.Rotation)];

                foreach (var nextDirection in nextDirections)
                {
                    if (nextDirection == new Direction(-direction.Dx, -direction.Dy))
                    {
                        valid &= ProcessPosition(nextPosition, visited);
                    }
                }
            }

            if (valid && _grid.IsSolved)
            {
                return true;
            }

            _grid[position] = previousState;

            _grid.PropagatePower();
        }

        visited.Remove(position);
        
        return false;
    }
}