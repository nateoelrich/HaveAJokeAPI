namespace HaveAJokeAPI.Configuration;

public class DadJokeApiConfiguration
{
    public required string BaseUrl { get; init; }
    public string AcceptHeaderValue { get; init; } = "application/json";
    
    public string SearchEndpoint { get; } = "/search";
}