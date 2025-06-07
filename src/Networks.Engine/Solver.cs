using Networks.Engine.Board;

namespace Networks.Engine;

public class Solver
{
    private Grid _grid;
    
    public Action<Grid> StepCallback { get; init; }
    
    public Action<Cell, int, int> DeltaStepCallback { private get; init; }

    private readonly HashSet<(Point, Rotation)> _visited = [];

    public bool Solve(Grid grid)
    {
        _grid = grid;

        ProcessPosition(_grid.PowerSource);
        
        return false;
    }

    private void ProcessPosition(Point position)
    {
        var cell = _grid[position];

        var rotations = cell.Piece == Piece.Straight ? 2 : 4;
        
        for (var rotation = 0; rotation < rotations; rotation++)
        {
            if (! _visited.Add((position, (Rotation) rotation)))
            {
                continue;
            }
            
            var directions = Connector.Connections[(cell.Piece, (Rotation) rotation)];

            var previousState = _grid[position];

            foreach (var direction in directions)
            {
                var newCell = new Cell(cell.Piece, (Rotation) rotation, true);
                
                _grid[position] = newCell;
                    
                StepCallback?.Invoke(_grid);

                DeltaStepCallback?.Invoke(newCell, position.X, position.Y);
                
                var nextPosition = position + direction;

                var nextCell = _grid[nextPosition];

                if (nextCell.Piece == Piece.OutOfBounds)
                {
                    continue;
                }

                var nextDirections = Connector.Connections[(nextCell.Piece, nextCell.Rotation)];

                if (nextDirections.Contains(new Direction(-direction.Dx, -direction.Dy)))
                {
                    ProcessPosition(nextPosition);
                }

                _grid[position] = previousState;
            }
        }
    }
}