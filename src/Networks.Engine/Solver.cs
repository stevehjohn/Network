using Networks.Engine.Board;

namespace Networks.Engine;

public class Solver
{
    private Grid _grid;
    
    public Action<Grid> StepCallback { get; init; }
    
    public Action<Cell, int, int> DeltaStepCallback { private get; init; }

    private readonly Stack<(Point Position, Rotation Rotation)> _stack = [];

    private readonly HashSet<(Point, Rotation)> _visited = [];

    public bool Solve(Grid grid)
    {
        _grid = grid;
        
        AddCellToQueue(grid.PowerSource);

        while (_stack.Count > 0)
        {
            var move = _stack.Pop();

            var cell = _grid[move.Position];

            var directions = Connector.Connections[(cell.Piece, move.Rotation)];

            foreach (var direction in directions)
            {
                var previousState = _grid[move.Position];

                var newCell = new Cell(cell.Piece, move.Rotation, true);
                
                _grid[move.Position] = newCell;
                    
                StepCallback?.Invoke(_grid);
                    
                DeltaStepCallback?.Invoke(newCell, move.Position.X, move.Position.Y);
                
                var nextPosition = move.Position + direction;

                var nextCell = _grid[nextPosition];

                if (nextCell.Piece == Piece.OutOfBounds)
                {
                    continue;
                }

                var nextDirections = Connector.Connections[(nextCell.Piece, nextCell.Rotation)];

                if (nextDirections.Contains(new Direction(-direction.Dx, -direction.Dy)))
                {
                    AddCellToQueue(nextPosition);
                }

                _grid[move.Position] = previousState;
            }
        }
        
        return false;
    }

    private void AddCellToQueue(Point position)
    {
        var x = position.X;

        var y = position.Y;
        
        var cell = _grid[x, y];

        var rotations = cell.Piece == Piece.Straight ? 2 : 4;
        
        for (var rotation = 0; rotation < rotations; rotation++)
        {
            if (! _visited.Add((position, (Rotation) rotation)))
            {
                continue;
            }

            _stack.Push((position, (Rotation) rotation));
        }
    }
}