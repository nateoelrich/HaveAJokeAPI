using Flurl.Http;
using HaveAJokeAPI.Configuration;
using HaveAJokeAPI.Models;

namespace HaveAJokeAPI.Repositories;
public interface IDadJokeRepository
{
    Task<Joke?> GetRandomJokeAsync();
    Task<ICollection<Joke>?>  SearchJokesAsync(int page, int limit, string term);
}

public class DadJokeRepository(DadJokeApiConfiguration apiConfiguration) : IDadJokeRepository
{
    public async Task<Joke?> GetRandomJokeAsync()
    {
        try
        {
            Joke? joke = null;
            
            var result = await apiConfiguration.BaseUrl
                .WithHeader("Accept", apiConfiguration.AcceptHeaderValue)
                .GetJsonAsync<Joke>();

            if (result != null)
            {
                joke = result;
            }

            return joke != null && joke.Status != 200 ? null : joke;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<ICollection<Joke>?> SearchJokesAsync(int page, int limit, string term)
    {
        try
        {
            DadJokeApiSearchResponse? searchResponse = null;

            var url = $"{apiConfiguration.BaseUrl}/search?page={page}&limit={limit}&term={term}";
            
            var result = await url
                    .WithHeader("Accept", apiConfiguration.AcceptHeaderValue)
                    .GetJsonAsync<DadJokeApiSearchResponse>();

            if (result != null)
            {
                searchResponse = result;
            }

            if (searchResponse != null && searchResponse.Status == 200)
            {
                return searchResponse.Results;
            }
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}