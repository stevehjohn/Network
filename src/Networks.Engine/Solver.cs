using Networks.Engine.Board;

namespace Networks.Engine;

public class Solver
{
    private Grid _grid;
    
    public Action<Grid> StepCallback { get; set; }
    
    public Action<(Piece Piece, int X, int Y)> DeltaStepCallback { get; init; }

    public bool Solve(Grid grid)
    {
        _grid = grid;
        
        var result = ProcessCell(grid.PowerSource);
        
        return result;
    }

    private bool ProcessCell(Point position)
    {
        var x = position.X;

        var y = position.Y;
        
        var cell = _grid[x, y];

        var directions = cell.Piece == Piece.Straight ? 2 : 4;
        
        for (var direction = 0; direction < directions; direction++)
        {
        }

        return false;
    }
}