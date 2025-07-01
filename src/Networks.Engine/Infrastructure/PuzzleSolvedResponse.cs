using System.Text.Json.Serialization;

namespace Networks.Engine.Infrastructure;

public class PuzzleSolvedResponse
{
    [JsonPropertyName("global_leaderboard")]
    public LeaderboardEntry[] GlobalLeaderboard { get; init; }
}