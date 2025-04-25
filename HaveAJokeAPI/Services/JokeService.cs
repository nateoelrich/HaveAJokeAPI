using HaveAJokeAPI.Exceptions;
using HaveAJokeAPI.Models;
using HaveAJokeAPI.Repositories;

namespace HaveAJokeAPI.Services;

public interface IJokeService
{
    Task<Joke> GetRandomJokeAsync();
    
    Task<List<Joke>> SearchJokesAsync(int page, int limit, string token);
}

/// <summary>
/// The best service around who's a good service, you are!
/// </summary>
public class JokeService : IJokeService
{
    private readonly IDadJokeRepository _repository;

    /// <summary>
    /// Inits a JokeService
    /// </summary>
    /// <param name="repository"><see cref="IDadJokeRepository"/></param>
    public JokeService(IDadJokeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a random dad joke with a random rating
    /// </summary>
    /// <returns><see cref="JokeResponse"/></returns>
    /// <exception cref="JokeNotFoundException">When no joke is found</exception>
    public async Task<Joke> GetRandomJokeAsync()
    {
        var joke = await _repository.GetRandomJokeAsync();

        if (joke == null)
        {
            throw new JokeNotFoundException("Ain't nobody got time for this!");
        }

        return joke;
    }

    public async Task<List<Joke>> SearchJokesAsync(int page, int limit, string token)
    {
        var jokes = await _repository.SearchJokesAsync(page, limit, token);

        if (jokes == null)
        {
            throw new JokeNotFoundException("try again later!");
        }

        return CategorizeJokes(jokes);
    }

    private List<Joke> CategorizeJokes(ICollection<Joke> jokes)
    {
        // TODO: add all the sorting and token highlight here
        var jokies = new List<Joke>();
        foreach (var joke in jokes)
        {
            jokies.Add(joke);
        }

        return jokies;
    }
}