using Networks.Desktop.Presentation;
using Networks.Engine.Infrastructure;

namespace Networks.Desktop;

public static class EntryPoint
{
    public static void Main(string[] arguments)
    {
        PuzzleManager.Path = "Data/Puzzles.json";
        
        using var renderer = new PuzzleRenderer();

        renderer.Grid = PuzzleManager.Instance.GetPuzzle(int.Parse(arguments[0]));
        
        renderer.Run();
    }
}