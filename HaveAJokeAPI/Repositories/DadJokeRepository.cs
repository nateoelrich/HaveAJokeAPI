using Flurl.Http;
using HaveAJokeAPI.Configuration;
using HaveAJokeAPI.Models;

namespace HaveAJokeAPI.Repositories;
public interface IDadJokeRepository
{
    Task<Joke?> GetRandomJoke();
    Task<ICollection<Joke>>  SearchJokes(string jokeToken);
}

public class DadJokeRepository : IDadJokeRepository
{
    private readonly DadJokeApiConfiguration _apiConfiguration;

    public DadJokeRepository(DadJokeApiConfiguration apiConfiguration)
    {
        _apiConfiguration = apiConfiguration;
    }
    
    public async Task<Joke?> GetRandomJoke()
    {
        try
        {
            Joke? joke = null;
            
            var result = await _apiConfiguration.BaseUrl
                .WithHeader("Accept", _apiConfiguration.AcceptHeader)
                .GetJsonAsync<Joke>();

            if (result != null)
            {
                joke = result;
            }

            return joke!.Status != 200 ? null : joke;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public Task<ICollection<Joke>> SearchJokes(string jokeToken)
    {
        throw new NotImplementedException();
    }
}