using System.Text.Json.Serialization;

namespace RandomUserShifts.Infrastructure.Http.Models;

public sealed class RandomUserResponse
{
    [JsonPropertyName("results")]
    public List<RandomUserResult> Results { get; set; } = new();
}

public sealed class RandomUserResult
{
    [JsonPropertyName("name")]
    public RandomUserName Name { get; set; } = new();

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("nat")]
    public string Nationality { get; set; } = string.Empty;

    [JsonPropertyName("login")]
    public RandomUserLogin Login { get; set; } = new();
}

public sealed class RandomUserName
{
    [JsonPropertyName("first")]
    public string First { get; set; } = string.Empty;

    [JsonPropertyName("last")]
    public string Last { get; set; } = string.Empty;
}

public sealed class RandomUserLogin
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = string.Empty;
}
