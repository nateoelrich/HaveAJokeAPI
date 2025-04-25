namespace HaveAJokeAPI.Configuration;

public class DadJokeApiConfiguration
{
    public required string BaseUrl { get; init; }
    public string AcceptHeader { get; init; } = "application/json";
}