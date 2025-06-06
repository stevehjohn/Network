using System.Net;
using System.Text.Json;
using HtmlAgilityPack;
using Networks.Engine.Board;
using Networks.Engine.Models;

namespace Networks.Engine.Infrastructure;

public sealed class PuzzleClient : IDisposable
{
    private const string BaseUri = "https://puzzlemadness.co.uk/";

    private readonly HttpClientHandler _handler;
    
    private readonly HttpClient _client;

    private readonly int _userId;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PuzzleClient()
    {
        var cookieContainer = new CookieContainer();
        
        _handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer
        };

        var lines = File.ReadAllLines("cookies.txt");

        foreach (var line in lines)
        {
            var parts = line.Split('=');

            if (parts[0].Equals("userid", StringComparison.InvariantCultureIgnoreCase))
            {
                _userId = int.Parse(parts[1]);
            }

            cookieContainer.Add(new Uri(BaseUri), new Cookie(parts[0], parts[1]));
        }
        
        _client = new HttpClient(_handler)
        {
            BaseAddress = new Uri(BaseUri)
        };
        
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 11.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.6998.166 Safari/537.36");
    }
    
    public (DateOnly Date, Grid Grid, int Variant)? GetNextPuzzle(Difficulty difficulty)
    {
        var nextPuzzleDate = GetOldestIncompletePuzzleDate(difficulty);

        if (nextPuzzleDate == null)
        {
            return null;
        }
        
        var year = nextPuzzleDate.Value.Year;
        
        var month = nextPuzzleDate.Value.Month;
        
        var day = nextPuzzleDate.Value.Day;
        
        using var response = _client.GetAsync($"network/{difficulty}/{year}/{month}/{day}").Result;
            
        var page = response.Content.ReadAsStringAsync().Result;

        var puzzleJson = page[(page.IndexOf("puzzleData = ", StringComparison.InvariantCultureIgnoreCase) + 13)..];
        
        puzzleJson = puzzleJson[..puzzleJson.IndexOf(";", StringComparison.InvariantCultureIgnoreCase)];
        
        var puzzle = JsonSerializer.Deserialize<Puzzle>(puzzleJson, _jsonSerializerOptions);

        return (nextPuzzleDate.Value, new Grid(puzzle), puzzle.Source.Variant);
    }

    private DateOnly? GetOldestIncompletePuzzleDate(Difficulty difficulty)
    {
        var now = DateTime.Now;

        for (var year = 2005; year <= now.Year; year++)
        {
            using var response = _client.GetAsync($"/archive/network/{difficulty.ToString().ToLower()}/{year}").Result;
            
            var page = response.Content.ReadAsStringAsync().Result;

            var dom = new HtmlDocument();
            
            dom.LoadHtml(page);
            
            var puzzles = dom.DocumentNode.SelectNodes("//td[@class='puzzleNotDone']");

            if (puzzles != null && puzzles.Count > 0)
            {
                var puzzle = puzzles[0];
                
                var id =puzzle.Attributes["id"].Value;

                var parts = id.Split('-');
                
                return new DateOnly(int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]));
            }
        }

        return null;   
    }

    public void Dispose()
    {
        _handler?.Dispose();
        
        _client?.Dispose();
    }
}