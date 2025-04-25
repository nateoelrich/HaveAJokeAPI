using Flurl.Http;
using HaveAJokeAPI.Configuration;
using HaveAJokeAPI.Models;

namespace HaveAJokeAPI.Repositories;

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

            if (string.IsNullOrEmpty(joke!.Status) || joke.Status == "500")
            {
                return null;
            }

            return joke;
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