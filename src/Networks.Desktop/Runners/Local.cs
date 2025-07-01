using System.Diagnostics;
using Networks.Desktop.Presentation;
using Networks.Engine;
using Networks.Engine.Board;
using Networks.Engine.Infrastructure;
using static System.Console;

namespace Networks.Desktop.Runners;

public class Local
{
    private long _count;

    private Stopwatch _stopwatch;
    
    public void Run(int puzzleNumber)
    {
        PuzzleManager.Path = "Data/Puzzles.json";
        
        using var renderer = new PuzzleRenderer();

        renderer.Grid = PuzzleManager.Instance.GetPuzzle(int.Parse(arguments[0]));
        
        renderer.Run();        var solver = new Solver
        {
            StepCallback = VisualiseStep
        };

        PuzzleManager.Path = "Data/Puzzles.json";
        
        var puzzle = PuzzleManager.Instance.GetPuzzle(puzzleNumber);
        
        WriteLine($"Puzzle number: {puzzleNumber} ({puzzle.Width}x{puzzle.Height})");
        
        WriteLine();

        WriteLine(puzzle.ToString());
        
        _stopwatch = Stopwatch.StartNew();
        
        var result = solver.Solve(puzzle);
        
        CursorTop = puzzle.Height + 3;
        
        WriteLine(puzzle.ToString());
        
        WriteLine($"Solve state: {result}                 ");
        
        WriteLine($"Steps:       {_count:N0}              ");
                
        WriteLine($@"Elapsed:     {_stopwatch.Elapsed:h\:mm\:ss\.fff}");
        
        WriteLine();
    }

    private void VisualiseStep(Grid grid)
    {
        _count++;

        if (_count % 1_000_000 != 0)
        {
            return;
        }

        CursorTop = grid.Height + 3;
        
        WriteLine(grid.ToString());
        
        WriteLine($"Steps:       {_count:N0}");
        
        WriteLine($@"Elapsed:     {_stopwatch.Elapsed:h\:mm\:ss\.fff}");
    }
}