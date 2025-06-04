using Networks.Engine.Infrastructure;
using static System.Console;

namespace Networks.Console;

public static class EntryPoint
{
    public static void Main()
    {
        PuzzleManager.Path = "Data/Puzzles.json";

        var grid = PuzzleManager.Instance.GetPuzzle(0);
        
        WriteLine(grid.ToString());
    }
}