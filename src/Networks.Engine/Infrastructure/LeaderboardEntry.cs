// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Networks.Engine.Infrastructure;

[UsedImplicitly]
public class LeaderboardEntry
{
    [JsonPropertyName("position")]
    public int Position { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }
    
    [JsonPropertyName("username")]
    public string Username { get; set; }
}