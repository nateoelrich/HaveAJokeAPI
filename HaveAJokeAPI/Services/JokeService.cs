using System.Text.RegularExpressions;
using HaveAJokeAPI.Exceptions;
using HaveAJokeAPI.Helpers;
using HaveAJokeAPI.Models;
using HaveAJokeAPI.Repositories;

namespace HaveAJokeAPI.Services;

public interface IJokeService
{
    Task<Joke> GetRandomJokeAsync();
    
    Task<Dictionary<JokeLengthCategory, List<string>>> SearchJokesAsync(int page, int limit, string token);
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

    public async Task<Dictionary<JokeLengthCategory, List<string>>> SearchJokesAsync(int page, int limit, string token)
    {
        var jokes = await _repository.SearchJokesAsync(page, limit, token);

        if (jokes == null)
        {
            throw new JokeNotFoundException("try again later!");
        }

        return JokeTokenizer.TransformJokes(jokes, token.Split(" "));
    }
}