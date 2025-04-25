using System.Text.Json.Serialization;

namespace HaveAJokeAPI.Models;

public class DadJokeApiSearchResponse
{
    [JsonPropertyName("results")]
    public ICollection<Joke>? Results { get; set; }
    
    [JsonPropertyName("status")]
    public int Status { get; set; }
}