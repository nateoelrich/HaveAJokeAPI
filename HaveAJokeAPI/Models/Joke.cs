using System.Text.Json.Serialization;

namespace HaveAJokeAPI.Models;

public record Joke()
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("joke")]
    public string Text { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
}