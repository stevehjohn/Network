using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Networks.Engine.Infrastructure;

public class PuzzleSolvedResponse
{
    [JsonPropertyName("global_leaderboard")]
    public LeaderboardEntry[] GlobalLeaderboard { get; set; }
}