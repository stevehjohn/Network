using System.Text.Json;
using Networks.Engine.Board;
using Networks.Engine.Models;

namespace Networks.Engine.Infrastructure;

public class PuzzleManager
{
    private List<Grid> _puzzles;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Grid GetPuzzle(int puzzleNumber) => _puzzles[puzzleNumber].Clone();
        
    public static string Path { get; set; }

    private static readonly Lazy<PuzzleManager> Lazy = new(GetPuzzleManager);

    public static PuzzleManager Instance => Lazy.Value;

    private PuzzleManager()
    {
    }

    private static PuzzleManager GetPuzzleManager()
    {
        if (Path == null)
        {
            throw new InvalidOperationException("Please set the Path property before using the PuzzleManager.");
        }

        var puzzleJson = File.ReadAllText(Path);

        var puzzles = JsonSerializer.Deserialize<Puzzle[]>(puzzleJson, JsonSerializerOptions);

        var grids = new List<Grid>();

        foreach (var puzzle in puzzles)
        {
            var grid = new Grid(puzzle);

            grids.Add(grid);
        }
        
        var instance = new PuzzleManager
        {
            _puzzles = grids
        };
        
        return instance;
    }
}